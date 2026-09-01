using System;
using UnityEngine;

namespace GameTerms.UI
{
    public enum AppTab
    {
        Home,
        Categories,
        Favorites
    }

    public enum AppScreen
    {
        Home,
        Categories,
        CategoryTerms,
        Favorites,
        SearchResults,
        TermDetail
    }

    public sealed class AppNavigator
    {
        public AppScreen CurrentScreen { get; private set; } = AppScreen.Home;
        public AppTab CurrentTab { get; private set; } = AppTab.Home;
        public GlossaryCategory? SelectedCategory { get; private set; }
        public string SearchQuery { get; private set; } = string.Empty;
        public string SelectedTermId { get; private set; }

        public event Action Changed;

        public void ShowTab(AppTab tab)
        {
            CurrentTab = tab;
            CurrentScreen = tab switch
            {
                AppTab.Categories => AppScreen.Categories,
                AppTab.Favorites => AppScreen.Favorites,
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
                case AppScreen.TermDetail:
                    CurrentScreen = CurrentTab switch
                    {
                        AppTab.Categories => AppScreen.Categories,
                        AppTab.Favorites => AppScreen.Favorites,
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
                default:
                    ShowTab(CurrentTab);
                    break;
            }

            Changed?.Invoke();
        }
    }
}
