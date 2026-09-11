using System;
using System.Collections.Generic;
using System.Linq;

namespace GameTerms
{
    public sealed class LearningPathService
    {
        private readonly UserDataService userData;
        private readonly GlossaryService glossary;
        private readonly Dictionary<string, PathProgressRecord> progressById = new(StringComparer.Ordinal);

        public event Action Changed;

        public LearningPathService(UserDataService userData, GlossaryService glossary)
        {
            this.userData = userData;
            this.glossary = glossary;
            RebuildIndex();
        }

        public IReadOnlyList<LearningPathDefinition> GetPaths() => LearningPathCatalog.All;

        public LearningPathDefinition GetPath(string pathId) => LearningPathCatalog.Get(pathId);

        public PathProgressRecord GetProgress(string pathId)
        {
            if (string.IsNullOrWhiteSpace(pathId))
            {
                return null;
            }

            return progressById.TryGetValue(pathId, out var record) ? Clone(record) : CreateEmpty(pathId);
        }

        public bool IsStepCompleted(string pathId, string stepId)
        {
            var record = GetOrCreate(pathId);
            return record.CompletedStepIds.Contains(stepId);
        }

        public bool IsStepUnlocked(string pathId, string stepId)
        {
            var path = GetPath(pathId);
            if (path == null)
            {
                return false;
            }

            var index = path.Steps.FindIndex(step => step.Id == stepId);
            if (index < 0)
            {
                return false;
            }

            if (index == 0)
            {
                return true;
            }

            var previous = path.Steps[index - 1];
            return IsStepCompleted(pathId, previous.Id);
        }

        public string GetResumeStepId(string pathId)
        {
            var path = GetPath(pathId);
            if (path == null)
            {
                return null;
            }

            foreach (var step in path.Steps)
            {
                if (!IsStepCompleted(pathId, step.Id) && IsStepUnlocked(pathId, step.Id))
                {
                    return step.Id;
                }
            }

            return path.Steps.Count > 0 ? path.Steps[^1].Id : null;
        }

        public float GetCompletionRatio(string pathId)
        {
            var path = GetPath(pathId);
            if (path == null || path.Steps.Count == 0)
            {
                return 0f;
            }

            var completed = path.Steps.Count(step => IsStepCompleted(pathId, step.Id));
            return (float)completed / path.Steps.Count;
        }

        public int GetCompletedStepCount(string pathId)
        {
            var path = GetPath(pathId);
            if (path == null)
            {
                return 0;
            }

            return path.Steps.Count(step => IsStepCompleted(pathId, step.Id));
        }

        public bool IsPathComplete(string pathId)
        {
            var path = GetPath(pathId);
            return path != null && path.Steps.All(step => IsStepCompleted(pathId, step.Id));
        }

        public bool CompleteLesson(string pathId, string stepId)
        {
            var path = GetPath(pathId);
            var step = path?.Steps.FirstOrDefault(entry => entry.Id == stepId);
            if (step == null || step.Type != LearningPathStepType.Lesson)
            {
                return false;
            }

            if (!IsStepUnlocked(pathId, stepId))
            {
                return false;
            }

            if (glossary.GetTerm(step.TermId) == null)
            {
                return false;
            }

            MarkStepComplete(pathId, stepId);
            return true;
        }

        public bool RecordAssessmentResult(string pathId, string stepId, QuizSessionResult result)
        {
            var path = GetPath(pathId);
            var step = path?.Steps.FirstOrDefault(entry => entry.Id == stepId);
            if (step == null || result == null)
            {
                return false;
            }

            if (step.Type is not (LearningPathStepType.Checkpoint or LearningPathStepType.FinalAssessment))
            {
                return false;
            }

            if (!IsStepUnlocked(pathId, stepId))
            {
                return false;
            }

            var passed = result.CorrectCount >= step.PassScore;
            var record = GetOrCreate(pathId);
            if (step.Type == LearningPathStepType.FinalAssessment)
            {
                record.FinalQuizAttempts++;
                if (result.CorrectCount > record.FinalQuizBestScore ||
                    (result.CorrectCount == record.FinalQuizBestScore && result.TotalQuestions > record.FinalQuizBestTotal))
                {
                    record.FinalQuizBestScore = result.CorrectCount;
                    record.FinalQuizBestTotal = result.TotalQuestions;
                }
            }

            if (passed)
            {
                MarkStepComplete(pathId, stepId);
                return true;
            }

            Touch(record);
            Persist();
            return false;
        }

        public StudyConfig CreateQuizConfig(string pathId, string stepId)
        {
            var path = GetPath(pathId);
            var step = path?.Steps.FirstOrDefault(entry => entry.Id == stepId);
            if (step == null || step.Type == LearningPathStepType.Lesson)
            {
                return null;
            }

            return new StudyConfig
            {
                Mode = StudyMode.Quiz,
                Scope = StudyScope.TermList,
                TermIds = new List<string>(step.QuizTermIds ?? new List<string>()),
                QuestionCount = Math.Max(1, step.QuestionCount),
                PathId = pathId,
                PathStepId = stepId
            };
        }

        private void MarkStepComplete(string pathId, string stepId)
        {
            var record = GetOrCreate(pathId);
            if (!record.CompletedStepIds.Contains(stepId))
            {
                record.CompletedStepIds.Add(stepId);
            }

            Touch(record);
            Persist();
        }

        private void Touch(PathProgressRecord record)
        {
            record.LastActivityUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        private PathProgressRecord GetOrCreate(string pathId)
        {
            if (progressById.TryGetValue(pathId, out var existing))
            {
                return existing;
            }

            var created = CreateEmpty(pathId);
            progressById[pathId] = created;
            userData.Snapshot.PathProgress.Add(created);
            return created;
        }

        private void RebuildIndex()
        {
            progressById.Clear();
            var cleaned = new List<PathProgressRecord>();
            foreach (var record in userData.Snapshot.PathProgress)
            {
                if (record == null || string.IsNullOrWhiteSpace(record.PathId) || GetPath(record.PathId) == null)
                {
                    continue;
                }

                record.CompletedStepIds ??= new List<string>();
                progressById[record.PathId] = record;
                cleaned.Add(record);
            }

            userData.Snapshot.PathProgress = cleaned;
        }

        private void Persist()
        {
            userData.Save();
            Changed?.Invoke();
        }

        private static PathProgressRecord CreateEmpty(string pathId)
        {
            return new PathProgressRecord
            {
                PathId = pathId,
                CompletedStepIds = new List<string>()
            };
        }

        private static PathProgressRecord Clone(PathProgressRecord record)
        {
            return new PathProgressRecord
            {
                PathId = record.PathId,
                CompletedStepIds = new List<string>(record.CompletedStepIds ?? new List<string>()),
                FinalQuizBestScore = record.FinalQuizBestScore,
                FinalQuizBestTotal = record.FinalQuizBestTotal,
                FinalQuizAttempts = record.FinalQuizAttempts,
                LastActivityUnix = record.LastActivityUnix
            };
        }
    }
}
