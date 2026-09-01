using System;
using System.Collections.Generic;
using System.Linq;
using GameTerms.Persistence;
using NUnit.Framework;
using UnityEngine;

namespace GameTerms.Tests
{
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
            var favorites = new FavoritesService(store);
            favorites.ToggleFavorite("draw-call");
            Assert.That(favorites.IsFavorite("draw-call"), Is.True);

            var reloaded = new FavoritesService(store);
            Assert.That(reloaded.IsFavorite("draw-call"), Is.True);
        }
    }

    public class RecentlyViewedServiceTests
    {
        [Test]
        public void RecordView_MovesExistingTermToTopWithoutDuplicates()
        {
            var store = new MemoryUserDataStore();
            var recents = new RecentlyViewedService(store);
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
            var store = new MemoryUserDataStore();
            var repository = new InMemoryGlossaryRepository(GlossaryTestData.CreateSampleTerms());
            var glossary = new GlossaryService(repository);
            var random = new RandomTermService(glossary, store);

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
        }

        [Test]
        public void UiTheme_ExistsInResources()
        {
            var theme = Resources.Load<UI.UiTheme>("UiTheme");
            Assert.That(theme, Is.Not.Null);
            Assert.That(theme.SansRegular, Is.Not.Null);
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
        private UserDataSnapshot snapshot = new();

        public UserDataSnapshot Load() => snapshot;

        public void Save(UserDataSnapshot newSnapshot)
        {
            snapshot = newSnapshot;
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
                }
            };
        }
    }
}
