using System;
using GameTerms.Persistence;
using UnityEngine;

namespace GameTerms
{
    public sealed class AppServices
    {
        public UserDataService UserData { get; }
        public GlossaryService Glossary { get; }
        public SearchService Search { get; }
        public FavoritesService Favorites { get; }
        public RecentlyViewedService RecentlyViewed { get; }
        public DailyTermService DailyTerm { get; }
        public RandomTermService RandomTerm { get; }
        public ProgressService Progress { get; }
        public QuizService Quiz { get; }
        public FlashcardService Flashcards { get; }
        public IGlossaryRepository Repository { get; }

        public AppServices(GlossaryDatabaseAsset database, TextAsset glossaryJson = null)
        {
            var dataStore = new JsonUserDataStore();
            UserData = new UserDataService(dataStore);
            Repository = CreateRepository(database, glossaryJson);
            Glossary = new GlossaryService(Repository);
            Search = new SearchService(Repository);
            Favorites = new FavoritesService(UserData);
            RecentlyViewed = new RecentlyViewedService(UserData);
            DailyTerm = new DailyTermService(Glossary);
            RandomTerm = new RandomTermService(Glossary, UserData);
            Progress = new ProgressService(Glossary, UserData);
            Quiz = new QuizService(Glossary, Progress, Favorites);
            Flashcards = new FlashcardService(Glossary, Progress, Favorites);
        }

        private static IGlossaryRepository CreateRepository(GlossaryDatabaseAsset database, TextAsset glossaryJson)
        {
            if (database != null && database.Terms != null && database.Terms.Count > 0)
            {
                var scriptableRepository = new ScriptableGlossaryRepository(database);
                if (scriptableRepository.GetAllTerms().Count > 0)
                {
                    return scriptableRepository;
                }
            }

            if (glossaryJson != null)
            {
                var jsonRepository = new JsonGlossaryRepository(glossaryJson);
                if (jsonRepository.GetAllTerms().Count > 0)
                {
                    return jsonRepository;
                }
            }

            throw new InvalidOperationException("No glossary content found. Run Game Terms/Setup Project in the editor.");
        }
    }
}
