using System;
using System.Collections.Generic;
using System.Linq;

namespace GameTerms
{
    public sealed class QuizService
    {
        private readonly GlossaryService glossary;
        private readonly ProgressService progress;
        private readonly FavoritesService favorites;
        private readonly Random random;

        private StudyConfig config;
        private List<QuizQuestion> questions = new();
        private readonly List<QuizAnswerRecord> answers = new();
        private int currentIndex;
        private bool awaitingAdvance;
        private string lastSelectedOptionId;

        public QuizService(GlossaryService glossary, ProgressService progress, FavoritesService favorites, int? seed = null)
        {
            this.glossary = glossary;
            this.progress = progress;
            this.favorites = favorites;
            random = seed.HasValue ? new Random(seed.Value) : new Random();
        }

        public bool HasActiveSession => questions.Count > 0 && currentIndex < questions.Count;
        public bool IsComplete => questions.Count > 0 && currentIndex >= questions.Count;
        public int CurrentIndex => currentIndex;
        public int QuestionCount => questions.Count;
        public int CorrectCount => answers.Count(answer => answer.IsCorrect);
        public string LastSelectedOptionId => lastSelectedOptionId;
        public bool IsAwaitingAdvance => awaitingAdvance;

        public void StartSession(StudyConfig studyConfig)
        {
            config = studyConfig ?? new StudyConfig { Mode = StudyMode.Quiz, QuestionCount = 5 };
            config.QuestionCount = Math.Max(1, config.QuestionCount);
            questions = BuildQuestions(config);
            answers.Clear();
            currentIndex = 0;
            awaitingAdvance = false;
            lastSelectedOptionId = null;
            progress.RecordStudyActivity();
        }

        public QuizQuestion GetCurrentQuestion()
        {
            if (!HasActiveSession)
            {
                return null;
            }

            return questions[currentIndex];
        }

        public bool SubmitAnswer(string optionId)
        {
            if (!HasActiveSession || awaitingAdvance)
            {
                return false;
            }

            var question = questions[currentIndex];
            var selected = question.Options.FirstOrDefault(option => option.Id == optionId);
            if (selected == null)
            {
                return false;
            }

            var isCorrect = selected.IsCorrect;
            lastSelectedOptionId = optionId;
            awaitingAdvance = true;
            answers.Add(new QuizAnswerRecord
            {
                TermId = question.TermId,
                SelectedOptionId = optionId,
                IsCorrect = isCorrect
            });
            progress.RecordAnswer(question.TermId, isCorrect);
            return isCorrect;
        }

        public bool Advance()
        {
            if (!awaitingAdvance)
            {
                return false;
            }

            awaitingAdvance = false;
            lastSelectedOptionId = null;
            currentIndex++;

            if (currentIndex >= questions.Count)
            {
                var result = GetResults();
                progress.RecordQuizSession(result.CorrectCount, result.TotalQuestions, result.MissedTermIds);
                return true;
            }

            return true;
        }

        public QuizSessionResult GetResults()
        {
            return new QuizSessionResult
            {
                TotalQuestions = questions.Count,
                CorrectCount = CorrectCount,
                MissedTermIds = answers.Where(answer => !answer.IsCorrect).Select(answer => answer.TermId).Distinct().ToList(),
                Answers = answers.ToList()
            };
        }

        private List<QuizQuestion> BuildQuestions(StudyConfig studyConfig)
        {
            var pool = GetTermPool(studyConfig).ToList();
            if (pool.Count == 0)
            {
                return new List<QuizQuestion>();
            }

            Shuffle(pool);
            var selected = pool.Take(Math.Min(studyConfig.QuestionCount, pool.Count)).ToList();
            var allTerms = glossary.GetAllTerms().ToList();
            var built = new List<QuizQuestion>();

            foreach (var term in selected)
            {
                built.Add(CreateQuestion(term, allTerms));
            }

            return built;
        }

        private QuizQuestion CreateQuestion(GlossaryTermData term, List<GlossaryTermData> allTerms)
        {
            var prompt = !string.IsNullOrWhiteSpace(term.ShortDefinition)
                ? term.ShortDefinition
                : term.SimpleExplanation;

            var distractors = allTerms
                .Where(candidate => candidate.Id != term.Id && candidate.Category == term.Category)
                .ToList();

            if (distractors.Count < 3)
            {
                var extras = allTerms
                    .Where(candidate => candidate.Id != term.Id && distractors.All(existing => existing.Id != candidate.Id))
                    .ToList();
                Shuffle(extras);
                distractors.AddRange(extras.Take(3 - distractors.Count));
            }

            Shuffle(distractors);
            distractors = distractors.Take(3).ToList();

            var options = new List<QuizOption>
            {
                new() { Id = term.Id, Label = term.Term, IsCorrect = true }
            };

            foreach (var distractor in distractors)
            {
                options.Add(new QuizOption
                {
                    Id = distractor.Id,
                    Label = distractor.Term,
                    IsCorrect = false
                });
            }

            while (options.Count < 4 && allTerms.Count > options.Count)
            {
                var filler = allTerms.FirstOrDefault(candidate => options.All(option => option.Id != candidate.Id));
                if (filler == null)
                {
                    break;
                }

                options.Add(new QuizOption { Id = filler.Id, Label = filler.Term, IsCorrect = false });
            }

            Shuffle(options);

            return new QuizQuestion
            {
                TermId = term.Id,
                Prompt = prompt,
                Options = options,
                CorrectOptionId = term.Id,
                Explanation = term.SimpleExplanation
            };
        }

        private IEnumerable<GlossaryTermData> GetTermPool(StudyConfig studyConfig)
        {
            IEnumerable<GlossaryTermData> pool = glossary.GetAllTerms();

            if (!string.IsNullOrWhiteSpace(studyConfig.FocusTermId))
            {
                var focus = glossary.GetTerm(studyConfig.FocusTermId);
                if (focus != null)
                {
                    return new[] { focus };
                }
            }

            switch (studyConfig.Scope)
            {
                case StudyScope.Category when studyConfig.Category.HasValue:
                    pool = glossary.GetTermsByCategory(studyConfig.Category.Value);
                    break;
                case StudyScope.Favorites:
                    var favoriteIds = new HashSet<string>(favorites.GetFavoriteIds(), StringComparer.Ordinal);
                    pool = glossary.GetAllTerms().Where(term => favoriteIds.Contains(term.Id));
                    break;
                case StudyScope.DueForReview:
                    var dueIds = new HashSet<string>(progress.GetDueTermIds(), StringComparer.Ordinal);
                    pool = glossary.GetAllTerms().Where(term => dueIds.Contains(term.Id));
                    break;
            }

            return pool.Where(term => !string.IsNullOrWhiteSpace(term.ShortDefinition) || !string.IsNullOrWhiteSpace(term.SimpleExplanation));
        }

        private void Shuffle<T>(IList<T> values)
        {
            for (var i = values.Count - 1; i > 0; i--)
            {
                var j = random.Next(i + 1);
                (values[i], values[j]) = (values[j], values[i]);
            }
        }
    }
}
