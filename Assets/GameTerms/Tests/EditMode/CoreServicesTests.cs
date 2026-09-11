using System;
using System.Collections.Generic;
using System.Linq;
using GameTerms.Persistence;
using NUnit.Framework;
using UnityEngine;

namespace GameTerms.Tests
{
    public class UserDataMigrationTests
    {
        [Test]
        public void Normalize_UpgradesV1SnapshotWithoutLosingFavorites()
        {
            var snapshot = new UserDataSnapshot
            {
                Version = 1,
                FavoriteTermIds = new List<string> { "draw-call" },
                RecentlyViewedTermIds = new List<string> { "object-pooling" }
            };

            var normalized = UserDataMigrator.Normalize(snapshot);

            Assert.That(normalized.Version, Is.EqualTo(2));
            Assert.That(normalized.FavoriteTermIds, Does.Contain("draw-call"));
            Assert.That(normalized.RecentlyViewedTermIds, Does.Contain("object-pooling"));
            Assert.That(normalized.TermProgress, Is.Not.Null);
            Assert.That(normalized.TermProgress.Count, Is.EqualTo(0));
            Assert.That(normalized.LastMissedTermIds, Is.Not.Null);
        }
    }

    public class UserDataServiceTests
    {
        [Test]
        public void SharedSnapshot_FavoriteAndProgressPersistTogether()
        {
            var store = new MemoryUserDataStore();
            var userData = new UserDataService(store);
            var repository = new InMemoryGlossaryRepository(GlossaryTestData.CreateSampleTerms());
            var glossary = new GlossaryService(repository);
            var favorites = new FavoritesService(userData);
            var progress = new ProgressService(glossary, userData);

            favorites.ToggleFavorite("draw-call");
            progress.RecordAnswer("object-pooling", true);

            Assert.That(store.LastSaved.FavoriteTermIds, Does.Contain("draw-call"));
            Assert.That(store.LastSaved.TermProgress.Any(record => record.TermId == "object-pooling"), Is.True);

            var reloaded = new UserDataService(store);
            Assert.That(reloaded.Snapshot.FavoriteTermIds, Does.Contain("draw-call"));
            Assert.That(reloaded.Snapshot.TermProgress.Any(record => record.TermId == "object-pooling"), Is.True);
        }
    }

    public class ProgressServiceTests
    {
        [Test]
        public void RecordAnswer_IncreasesMasteryAndSchedulesReview()
        {
            var userData = new UserDataService(new MemoryUserDataStore());
            var glossary = new GlossaryService(new InMemoryGlossaryRepository(GlossaryTestData.CreateSampleTerms()));
            var progress = new ProgressService(glossary, userData);
            var now = new DateTime(2026, 9, 11, 12, 0, 0, DateTimeKind.Utc);

            progress.RecordAnswer("draw-call", true, now);

            var record = progress.GetProgress("draw-call");
            Assert.That(record.MasteryLevel, Is.EqualTo(1));
            Assert.That(record.CorrectCount, Is.EqualTo(1));
            Assert.That(record.NextReviewUnix, Is.GreaterThan(record.LastReviewedUnix));
        }

        [Test]
        public void GetDueTermIds_IncludesUnreviewedAndExpiredTerms()
        {
            var userData = new UserDataService(new MemoryUserDataStore());
            var glossary = new GlossaryService(new InMemoryGlossaryRepository(GlossaryTestData.CreateSampleTerms()));
            var progress = new ProgressService(glossary, userData);
            var now = new DateTime(2026, 9, 11, 12, 0, 0, DateTimeKind.Utc);

            progress.RecordAnswer("draw-call", true, now.AddDays(-10));
            var due = progress.GetDueTermIds(now);

            Assert.That(due, Does.Contain("draw-call"));
            Assert.That(due, Does.Contain("object-pooling"));
        }

        [Test]
        public void ProgressService_StripsOrphanTermIdsOnLoad()
        {
            var store = new MemoryUserDataStore(new UserDataSnapshot
            {
                Version = 2,
                TermProgress = new List<TermProgressRecord>
                {
                    new() { TermId = "missing-term", MasteryLevel = 3 },
                    new() { TermId = "draw-call", MasteryLevel = 2 }
                }
            });
            var userData = new UserDataService(store);
            var glossary = new GlossaryService(new InMemoryGlossaryRepository(GlossaryTestData.CreateSampleTerms()));
            var progress = new ProgressService(glossary, userData);

            Assert.That(progress.GetProgress("missing-term"), Is.Null);
            Assert.That(progress.GetMasteryLevel("draw-call"), Is.EqualTo(2));
        }
    }

    public class QuizServiceTests
    {
        [Test]
        public void StartSession_BuildsFiveQuestionsWithOneCorrectOption()
        {
            var harness = StudyTestHarness.Create(seed: 7);
            harness.Quiz.StartSession(new StudyConfig { Mode = StudyMode.Quiz, QuestionCount = 5 });

            Assert.That(harness.Quiz.QuestionCount, Is.EqualTo(5));
            var question = harness.Quiz.GetCurrentQuestion();
            Assert.That(question.Options.Count, Is.EqualTo(4));
            Assert.That(question.Options.Count(option => option.IsCorrect), Is.EqualTo(1));
            Assert.That(question.Options.Any(option => option.Id == question.TermId), Is.True);
        }

        [Test]
        public void SubmitAnswer_ScoresAndCompletesSession()
        {
            var harness = StudyTestHarness.Create(seed: 11);
            harness.Quiz.StartSession(new StudyConfig { Mode = StudyMode.Quiz, QuestionCount = 2 });

            var first = harness.Quiz.GetCurrentQuestion();
            harness.Quiz.SubmitAnswer(first.CorrectOptionId);
            harness.Quiz.Advance();

            var second = harness.Quiz.GetCurrentQuestion();
            var wrong = second.Options.First(option => !option.IsCorrect).Id;
            harness.Quiz.SubmitAnswer(wrong);
            harness.Quiz.Advance();

            Assert.That(harness.Quiz.IsComplete, Is.True);
            var result = harness.Quiz.GetResults();
            Assert.That(result.CorrectCount, Is.EqualTo(1));
            Assert.That(result.MissedTermIds, Does.Contain(second.TermId));
            Assert.That(harness.UserData.Snapshot.TotalQuizSessions, Is.EqualTo(1));
        }

        [Test]
        public void CreateQuestion_DoesNotUseTermAsDistractor()
        {
            var harness = StudyTestHarness.Create(seed: 3);
            harness.Quiz.StartSession(new StudyConfig { Mode = StudyMode.Quiz, QuestionCount = 5 });

            while (harness.Quiz.HasActiveSession)
            {
                var question = harness.Quiz.GetCurrentQuestion();
                Assert.That(question.Options.Count(option => option.Id == question.TermId), Is.EqualTo(1));
                harness.Quiz.SubmitAnswer(question.CorrectOptionId);
                harness.Quiz.Advance();
            }
        }
    }

    public class FlashcardServiceTests
    {
        [Test]
        public void StartSession_BuildsDeckAndPersistsKnownMarks()
        {
            var harness = StudyTestHarness.Create(seed: 5);
            harness.Flashcards.StartSession(new StudyConfig
            {
                Mode = StudyMode.Flashcards,
                Scope = StudyScope.All,
                CardCount = 3
            });

            Assert.That(harness.Flashcards.DeckCount, Is.EqualTo(3));
            var firstId = harness.Flashcards.CurrentCard.Id;
            harness.Flashcards.Reveal();
            harness.Flashcards.MarkKnown();

            Assert.That(harness.Progress.GetMasteryLevel(firstId), Is.GreaterThan(0));
            Assert.That(harness.Flashcards.KnownThisSession, Is.EqualTo(1));
        }

        [Test]
        public void Session_CompletesAfterAllCards()
        {
            var harness = StudyTestHarness.Create(seed: 9);
            harness.Flashcards.StartSession(new StudyConfig
            {
                Mode = StudyMode.Flashcards,
                Scope = StudyScope.All,
                CardCount = 2
            });

            harness.Flashcards.MarkUnknown();
            harness.Flashcards.MarkKnown();

            Assert.That(harness.Flashcards.IsComplete, Is.True);
            Assert.That(harness.Flashcards.ReviewedThisSession, Is.EqualTo(2));
        }
    }

    public class SearchServiceTests
    {
        private SearchService searchService;

        [SetUp]
        public void SetUp()
        {
            var repository = new InMemoryGlossaryRepository(GlossaryTestData.CreateSampleTerms());
            searchService = new SearchService(repository);
        }

        [Test]
        public void Search_Gc_FindsGarbageCollection()
        {
            var results = searchService.Search("gc");
            Assert.That(results.Any(result => result.Term.Id == "garbage-collection"), Is.True);
        }

        [Test]
        public void Search_Draw_FindsDrawTerms()
        {
            var results = searchService.Search("draw");
            var ids = results.Select(result => result.Term.Id).ToList();
            Assert.That(ids, Does.Contain("draw-call"));
            Assert.That(ids, Does.Contain("occlusion-culling"));
        }

        [Test]
        public void Search_Pool_FindsObjectPooling()
        {
            var results = searchService.Search("pool");
            Assert.That(results.Any(result => result.Term.Id == "object-pooling"), Is.True);
        }
    }

    public class DailyTermServiceTests
    {
        [Test]
        public void GetTermOfTheDay_IsDeterministicForSameDate()
        {
            var repository = new InMemoryGlossaryRepository(GlossaryTestData.CreateSampleTerms());
            var service = new DailyTermService(new GlossaryService(repository));
            var date = new DateTime(2026, 9, 1);

            var first = service.GetTermOfTheDay(date);
            var second = service.GetTermOfTheDay(date);

            Assert.That(second.Id, Is.EqualTo(first.Id));
        }

        [Test]
        public void GetTermOfTheDay_ChangesOnNextDay()
        {
            var repository = new InMemoryGlossaryRepository(GlossaryTestData.CreateSampleTerms());
            var service = new DailyTermService(new GlossaryService(repository));

            var dayOne = service.GetTermOfTheDay(new DateTime(2026, 9, 1));
            var dayTwo = service.GetTermOfTheDay(new DateTime(2026, 9, 2));

            Assert.That(dayTwo.Id, Is.Not.EqualTo(dayOne.Id));
        }
    }

    public class FavoritesServiceTests
    {
        [Test]
        public void ToggleFavorite_PersistsBetweenInstances()
        {
            var store = new MemoryUserDataStore();
            var userData = new UserDataService(store);
            var favorites = new FavoritesService(userData);
            favorites.ToggleFavorite("draw-call");
            Assert.That(favorites.IsFavorite("draw-call"), Is.True);

            var reloaded = new FavoritesService(new UserDataService(store));
            Assert.That(reloaded.IsFavorite("draw-call"), Is.True);
        }
    }

    public class RecentlyViewedServiceTests
    {
        [Test]
        public void RecordView_MovesExistingTermToTopWithoutDuplicates()
        {
            var userData = new UserDataService(new MemoryUserDataStore());
            var recents = new RecentlyViewedService(userData);
            recents.RecordView("draw-call");
            recents.RecordView("object-pooling");
            recents.RecordView("draw-call");

            var ids = recents.GetRecentIds();
            Assert.That(ids[0], Is.EqualTo("draw-call"));
            Assert.That(ids.Count(id => id == "draw-call"), Is.EqualTo(1));
        }
    }

    public class RandomTermServiceTests
    {
        [Test]
        public void GetRandomTerm_DoesNotRepeatImmediately()
        {
            var userData = new UserDataService(new MemoryUserDataStore());
            var repository = new InMemoryGlossaryRepository(GlossaryTestData.CreateSampleTerms());
            var glossary = new GlossaryService(repository);
            var random = new RandomTermService(glossary, userData);

            var first = random.GetRandomTerm();
            var second = random.GetRandomTerm();

            Assert.That(second.Id, Is.Not.EqualTo(first.Id));
        }
    }

    public class ResourceSmokeTests
    {
        [Test]
        public void MainScene_IsInBuildSettings()
        {
            var hasMainScene = false;
            for (var i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; i++)
            {
                var scenePath = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
                if (scenePath.Contains("1_MainScene"))
                {
                    hasMainScene = true;
                    break;
                }
            }

            Assert.That(hasMainScene, Is.True);
        }

        [Test]
        public void GlossaryJson_ExistsInResources()
        {
            var glossary = Resources.Load<TextAsset>("glossary");
            Assert.That(glossary, Is.Not.Null);
            Assert.That(glossary.text.Contains("object-pooling"), Is.True);
            Assert.That(glossary.text.Contains("CommonMistake"), Is.True);
            Assert.That(glossary.text.Contains("addressable-assets"), Is.True);
        }

        [Test]
        public void UiTheme_ExistsInResources()
        {
            var theme = Resources.Load<UI.UiTheme>("UiTheme");
            Assert.That(theme, Is.Not.Null);
            Assert.That(theme.SansRegular, Is.Not.Null);
        }
    }

    internal sealed class StudyTestHarness
    {
        public UserDataService UserData { get; }
        public ProgressService Progress { get; }
        public QuizService Quiz { get; }
        public FlashcardService Flashcards { get; }

        private StudyTestHarness(UserDataService userData, ProgressService progress, QuizService quiz, FlashcardService flashcards)
        {
            UserData = userData;
            Progress = progress;
            Quiz = quiz;
            Flashcards = flashcards;
        }

        public static StudyTestHarness Create(int seed)
        {
            var userData = new UserDataService(new MemoryUserDataStore());
            var glossary = new GlossaryService(new InMemoryGlossaryRepository(GlossaryTestData.CreateSampleTerms()));
            var favorites = new FavoritesService(userData);
            var progress = new ProgressService(glossary, userData);
            var quiz = new QuizService(glossary, progress, favorites, seed);
            var flashcards = new FlashcardService(glossary, progress, favorites, seed);
            return new StudyTestHarness(userData, progress, quiz, flashcards);
        }
    }

    internal sealed class InMemoryGlossaryRepository : IGlossaryRepository
    {
        private readonly Dictionary<string, GlossaryTermData> terms;

        public InMemoryGlossaryRepository(IEnumerable<GlossaryTermData> source)
        {
            terms = source.ToDictionary(term => term.Id, term => term);
        }

        public IReadOnlyList<GlossaryTermData> GetAllTerms() => terms.Values.ToList();

        public GlossaryTermData GetTermById(string id) => terms.TryGetValue(id, out var term) ? term : null;

        public IReadOnlyList<GlossaryTermData> GetTermsByCategory(GlossaryCategory category)
        {
            return terms.Values.Where(term => term.Category == category).ToList();
        }

        public bool TryGetTerm(string id, out GlossaryTermData term) => terms.TryGetValue(id, out term);
    }

    internal sealed class MemoryUserDataStore : IUserDataStore
    {
        private UserDataSnapshot snapshot;

        public UserDataSnapshot LastSaved => snapshot;

        public MemoryUserDataStore(UserDataSnapshot initial = null)
        {
            snapshot = UserDataMigrator.Normalize(initial ?? new UserDataSnapshot());
        }

        public UserDataSnapshot Load() => snapshot;

        public void Save(UserDataSnapshot newSnapshot)
        {
            snapshot = UserDataMigrator.Normalize(newSnapshot);
        }
    }

    internal static class GlossaryTestData
    {
        public static List<GlossaryTermData> CreateSampleTerms()
        {
            return new List<GlossaryTermData>
            {
                new()
                {
                    Id = "draw-call",
                    Term = "Draw Call",
                    Category = GlossaryCategory.GraphicsAndRendering,
                    Difficulty = DifficultyLevel.Intermediate,
                    ShortDefinition = "A GPU draw request.",
                    Tags = new List<string> { "draw", "rendering" }
                },
                new()
                {
                    Id = "occlusion-culling",
                    Term = "Occlusion Culling",
                    Category = GlossaryCategory.GraphicsAndRendering,
                    Difficulty = DifficultyLevel.Intermediate,
                    ShortDefinition = "Skip hidden objects.",
                    Tags = new List<string> { "draw", "culling" }
                },
                new()
                {
                    Id = "object-pooling",
                    Term = "Object Pooling",
                    Abbreviation = "pool",
                    Category = GlossaryCategory.Programming,
                    Difficulty = DifficultyLevel.Intermediate,
                    ShortDefinition = "Reuse objects instead of creating new ones.",
                    Tags = new List<string> { "pool", "performance" }
                },
                new()
                {
                    Id = "garbage-collection",
                    Term = "Garbage Collection",
                    Abbreviation = "gc",
                    Category = GlossaryCategory.Programming,
                    Difficulty = DifficultyLevel.Intermediate,
                    ShortDefinition = "Automatic memory cleanup.",
                    Tags = new List<string> { "gc", "memory" }
                },
                new()
                {
                    Id = "core-loop",
                    Term = "Core Loop",
                    Category = GlossaryCategory.GameDesign,
                    Difficulty = DifficultyLevel.Beginner,
                    ShortDefinition = "Main repeating player actions."
                },
                new()
                {
                    Id = "state-machine",
                    Term = "State Machine",
                    Category = GlossaryCategory.Programming,
                    Difficulty = DifficultyLevel.Intermediate,
                    ShortDefinition = "Behavior switches between discrete states."
                },
                new()
                {
                    Id = "latency",
                    Term = "Latency",
                    Category = GlossaryCategory.Multiplayer,
                    Difficulty = DifficultyLevel.Beginner,
                    ShortDefinition = "Network delay between action and result."
                },
                new()
                {
                    Id = "tick-rate",
                    Term = "Tick Rate",
                    Category = GlossaryCategory.Multiplayer,
                    Difficulty = DifficultyLevel.Intermediate,
                    ShortDefinition = "How often the server updates simulation."
                }
            };
        }
    }
}
