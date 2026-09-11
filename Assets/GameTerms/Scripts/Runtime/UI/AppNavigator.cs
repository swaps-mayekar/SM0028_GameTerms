using System;

namespace GameTerms.UI
{
    public enum AppTab
    {
        Home,
        Categories,
        Study,
        Favorites
    }

    public enum AppScreen
    {
        Home,
        Categories,
        CategoryTerms,
        Favorites,
        SearchResults,
        TermDetail,
        StudyHub,
        FlashcardSession,
        QuizSession,
        QuizResults
    }

    public sealed class AppNavigator
    {
        public AppScreen CurrentScreen { get; private set; } = AppScreen.Home;
        public AppTab CurrentTab { get; private set; } = AppTab.Home;
        public GlossaryCategory? SelectedCategory { get; private set; }
        public string SearchQuery { get; private set; } = string.Empty;
        public string SelectedTermId { get; private set; }
        public StudyConfig ActiveStudyConfig { get; private set; }

        public event Action Changed;

        public void ShowTab(AppTab tab)
        {
            CurrentTab = tab;
            CurrentScreen = tab switch
            {
                AppTab.Categories => AppScreen.Categories,
                AppTab.Favorites => AppScreen.Favorites,
                AppTab.Study => AppScreen.StudyHub,
                _ => AppScreen.Home
            };
            Changed?.Invoke();
        }

        public void ShowCategory(GlossaryCategory category)
        {
            SelectedCategory = category;
            CurrentScreen = AppScreen.CategoryTerms;
            Changed?.Invoke();
        }

        public void ShowSearch(string query)
        {
            SearchQuery = query ?? string.Empty;
            CurrentScreen = string.IsNullOrWhiteSpace(SearchQuery) ? AppScreen.Home : AppScreen.SearchResults;
            Changed?.Invoke();
        }

        public void ShowTerm(string termId)
        {
            SelectedTermId = termId;
            CurrentScreen = AppScreen.TermDetail;
            Changed?.Invoke();
        }

        public void ShowStudyHub()
        {
            CurrentTab = AppTab.Study;
            CurrentScreen = AppScreen.StudyHub;
            Changed?.Invoke();
        }

        public void StartFlashcards(StudyConfig config)
        {
            ActiveStudyConfig = config ?? new StudyConfig { Mode = StudyMode.Flashcards };
            CurrentTab = AppTab.Study;
            CurrentScreen = AppScreen.FlashcardSession;
            Changed?.Invoke();
        }

        public void StartQuiz(StudyConfig config)
        {
            ActiveStudyConfig = config ?? new StudyConfig { Mode = StudyMode.Quiz, QuestionCount = 5 };
            CurrentTab = AppTab.Study;
            CurrentScreen = AppScreen.QuizSession;
            Changed?.Invoke();
        }

        public void ShowQuizResults()
        {
            CurrentScreen = AppScreen.QuizResults;
            Changed?.Invoke();
        }

        public void GoBack()
        {
            switch (CurrentScreen)
            {
                case AppScreen.TermDetail when !string.IsNullOrEmpty(SearchQuery):
                    CurrentScreen = AppScreen.SearchResults;
                    break;
                case AppScreen.TermDetail when SelectedCategory.HasValue:
                    CurrentScreen = AppScreen.CategoryTerms;
                    break;
                case AppScreen.TermDetail when CurrentTab == AppTab.Study:
                    CurrentScreen = AppScreen.StudyHub;
                    break;
                case AppScreen.TermDetail:
                    CurrentScreen = CurrentTab switch
                    {
                        AppTab.Categories => AppScreen.Categories,
                        AppTab.Favorites => AppScreen.Favorites,
                        AppTab.Study => AppScreen.StudyHub,
                        _ => AppScreen.Home
                    };
                    break;
                case AppScreen.SearchResults:
                    CurrentScreen = AppScreen.Home;
                    SearchQuery = string.Empty;
                    break;
                case AppScreen.CategoryTerms:
                    CurrentScreen = AppScreen.Categories;
                    SelectedCategory = null;
                    break;
                case AppScreen.FlashcardSession:
                case AppScreen.QuizSession:
                case AppScreen.QuizResults:
                    CurrentScreen = AppScreen.StudyHub;
                    break;
                default:
                    ShowTab(CurrentTab);
                    break;
            }

            Changed?.Invoke();
        }
    }
}
