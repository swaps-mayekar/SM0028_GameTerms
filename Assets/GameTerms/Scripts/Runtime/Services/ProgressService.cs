using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace GameTerms
{
    public sealed class ProgressService
    {
        private static readonly int[] ReviewIntervalsDays = { 1, 3, 7, 14, 30 };

        private readonly GlossaryService glossary;
        private readonly UserDataService userData;
        private readonly Dictionary<string, TermProgressRecord> progressById = new(StringComparer.Ordinal);

        public event Action Changed;

        public ProgressService(GlossaryService glossary, UserDataService userData)
        {
            this.glossary = glossary;
            this.userData = userData;
            RebuildIndex();
        }

        public TermProgressRecord GetProgress(string termId)
        {
            if (string.IsNullOrWhiteSpace(termId))
            {
                return null;
            }

            return progressById.TryGetValue(termId, out var record) ? Clone(record) : null;
        }

        public int GetMasteryLevel(string termId)
        {
            return progressById.TryGetValue(termId, out var record) ? record.MasteryLevel : 0;
        }

        public int GetMasteredCount()
        {
            return progressById.Values.Count(record => record.MasteryLevel >= 3 && glossary.GetTerm(record.TermId) != null);
        }

        public int GetTotalTermCount() => glossary.GetAllTerms().Count;

        public IReadOnlyList<string> GetDueTermIds(DateTime? nowUtc = null)
        {
            var now = ToUnix(nowUtc ?? DateTime.UtcNow);
            var due = new List<(string Id, long Next)>();

            foreach (var term in glossary.GetAllTerms())
            {
                if (!progressById.TryGetValue(term.Id, out var record))
                {
                    due.Add((term.Id, 0));
                    continue;
                }

                if (record.NextReviewUnix <= now)
                {
                    due.Add((term.Id, record.NextReviewUnix));
                }
            }

            return due
                .OrderBy(item => item.Next)
                .ThenBy(item => item.Id, StringComparer.Ordinal)
                .Select(item => item.Id)
                .ToList();
        }

        public float GetCategoryMastery(GlossaryCategory category)
        {
            var terms = glossary.GetTermsByCategory(category);
            if (terms.Count == 0)
            {
                return 0f;
            }

            var total = terms.Sum(term => GetMasteryLevel(term.Id));
            return total / (terms.Count * 5f);
        }

        public StudyStats GetOverallStats()
        {
            return new StudyStats
            {
                MasteredCount = GetMasteredCount(),
                TotalTerms = GetTotalTermCount(),
                DueCount = GetDueTermIds().Count,
                TotalQuizSessions = userData.Snapshot.TotalQuizSessions,
                BestQuizScore = userData.Snapshot.BestQuizScore,
                LastQuizScore = userData.Snapshot.LastQuizScore,
                CurrentStreakDays = userData.Snapshot.CurrentStreakDays,
                LastMissedTermIds = userData.Snapshot.LastMissedTermIds.ToList()
            };
        }

        public void RecordAnswer(string termId, bool correct, DateTime? nowUtc = null)
        {
            if (string.IsNullOrWhiteSpace(termId) || glossary.GetTerm(termId) == null)
            {
                return;
            }

            var now = nowUtc ?? DateTime.UtcNow;
            var record = GetOrCreate(termId);
            if (correct)
            {
                record.CorrectCount++;
            }
            else
            {
                record.IncorrectCount++;
            }

            UpdateMasteryAndSchedule(record, correct, now);
            Persist();
        }

        public void MarkKnown(string termId, DateTime? nowUtc = null)
        {
            RecordAnswer(termId, true, nowUtc);
        }

        public void MarkUnknown(string termId, DateTime? nowUtc = null)
        {
            RecordAnswer(termId, false, nowUtc);
        }

        public void RecordQuizSession(int score, int totalQuestions, IEnumerable<string> missedTermIds, DateTime? nowUtc = null)
        {
            var now = nowUtc ?? DateTime.UtcNow;
            var snapshot = userData.Snapshot;
            snapshot.TotalQuizSessions++;
            snapshot.LastQuizScore = Math.Clamp(score, 0, totalQuestions);
            if (snapshot.LastQuizScore > snapshot.BestQuizScore)
            {
                snapshot.BestQuizScore = snapshot.LastQuizScore;
            }

            snapshot.LastMissedTermIds = (missedTermIds ?? Enumerable.Empty<string>())
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct(StringComparer.Ordinal)
                .ToList();

            UpdateStreak(now);
            Persist();
        }

        public void RecordStudyActivity(DateTime? nowUtc = null)
        {
            UpdateStreak(nowUtc ?? DateTime.UtcNow);
            Persist();
        }

        private void UpdateStreak(DateTime nowUtc)
        {
            var today = nowUtc.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var snapshot = userData.Snapshot;
            if (snapshot.LastStudyDate == today)
            {
                return;
            }

            if (DateTime.TryParseExact(snapshot.LastStudyDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var lastDate))
            {
                snapshot.CurrentStreakDays = lastDate.Date.AddDays(1) == nowUtc.Date
                    ? Math.Max(1, snapshot.CurrentStreakDays + 1)
                    : 1;
            }
            else
            {
                snapshot.CurrentStreakDays = 1;
            }

            snapshot.LastStudyDate = today;
        }

        private void UpdateMasteryAndSchedule(TermProgressRecord record, bool correct, DateTime nowUtc)
        {
            if (correct)
            {
                record.MasteryLevel = Math.Min(5, record.MasteryLevel + 1);
            }
            else
            {
                record.MasteryLevel = Math.Max(0, record.MasteryLevel - 1);
            }

            record.LastReviewedUnix = ToUnix(nowUtc);
            var intervalIndex = Math.Clamp(record.MasteryLevel, 0, ReviewIntervalsDays.Length - 1);
            var days = correct ? ReviewIntervalsDays[intervalIndex] : 1;
            record.NextReviewUnix = ToUnix(nowUtc.Date.AddDays(days));
        }

        private TermProgressRecord GetOrCreate(string termId)
        {
            if (progressById.TryGetValue(termId, out var existing))
            {
                return existing;
            }

            var created = new TermProgressRecord { TermId = termId };
            progressById[termId] = created;
            userData.Snapshot.TermProgress.Add(created);
            return created;
        }

        private void RebuildIndex()
        {
            progressById.Clear();
            var cleaned = new List<TermProgressRecord>();
            foreach (var record in userData.Snapshot.TermProgress)
            {
                if (record == null || string.IsNullOrWhiteSpace(record.TermId) || glossary.GetTerm(record.TermId) == null)
                {
                    continue;
                }

                record.MasteryLevel = Math.Clamp(record.MasteryLevel, 0, 5);
                progressById[record.TermId] = record;
                cleaned.Add(record);
            }

            userData.Snapshot.TermProgress = cleaned;
        }

        private void Persist()
        {
            userData.Save();
            Changed?.Invoke();
        }

        private static long ToUnix(DateTime dateTime)
        {
            return new DateTimeOffset(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)).ToUnixTimeSeconds();
        }

        private static TermProgressRecord Clone(TermProgressRecord record)
        {
            return new TermProgressRecord
            {
                TermId = record.TermId,
                CorrectCount = record.CorrectCount,
                IncorrectCount = record.IncorrectCount,
                MasteryLevel = record.MasteryLevel,
                LastReviewedUnix = record.LastReviewedUnix,
                NextReviewUnix = record.NextReviewUnix
            };
        }
    }

    public sealed class StudyStats
    {
        public int MasteredCount;
        public int TotalTerms;
        public int DueCount;
        public int TotalQuizSessions;
        public int BestQuizScore;
        public int LastQuizScore;
        public int CurrentStreakDays;
        public List<string> LastMissedTermIds = new();
    }
}
