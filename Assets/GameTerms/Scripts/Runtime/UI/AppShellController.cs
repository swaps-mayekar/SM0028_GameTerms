using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameTerms.UI
{
    public sealed class AppShellController : MonoBehaviour
    {
        [SerializeField] private GlossaryDatabaseAsset database;
        [SerializeField] private UiTheme theme;
        [SerializeField] private TextAsset glossaryJson;

        private AppServices services;
        private AppNavigator navigator;
        private UiFactory factory;
        private RectTransform contentRoot;
        private RectTransform panelHost;
        private RectTransform bottomNav;
        private CanvasGroup contentCanvasGroup;
        private string currentSearchQuery = string.Empty;
        private TermSortMode currentSortMode = TermSortMode.Alphabetical;

        private void Awake()
        {
            if (database == null)
            {
                database = Resources.Load<GlossaryDatabaseAsset>("GlossaryDatabase");
            }

            if (glossaryJson == null)
            {
                glossaryJson = Resources.Load<TextAsset>("glossary");
            }

            if (theme == null)
            {
                theme = Resources.Load<UiTheme>("UiTheme");
            }

            EnsureThemeFonts();

            services = new AppServices(database, glossaryJson);
            navigator = new AppNavigator();
            BuildUi();
            navigator.Changed += Refresh;
            services.Favorites.Changed += Refresh;
            services.RecentlyViewed.Changed += Refresh;
            Refresh();
        }

        private void OnDestroy()
        {
            if (navigator != null)
            {
                navigator.Changed -= Refresh;
            }

            if (services?.Favorites != null)
            {
                services.Favorites.Changed -= Refresh;
            }

            if (services?.RecentlyViewed != null)
            {
                services.RecentlyViewed.Changed -= Refresh;
            }
        }

        private void BuildUi()
        {
            factory = new UiFactory(theme, Screen.width);

            var canvasGo = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(390f, 844f);
            scaler.matchWidthOrHeight = 0.5f;

            if (FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                new GameObject("EventSystem", typeof(UnityEngine.EventSystems.EventSystem), typeof(UnityEngine.InputSystem.UI.InputSystemUIInputModule));
            }

            contentRoot = factory.CreateRoot(canvas.transform, "SafeAreaRoot");
            contentRoot.gameObject.AddComponent<SafeAreaFitter>();

            var background = factory.CreateImage(contentRoot, theme.Background, "Background");
            UiFactory.Stretch(background.rectTransform);

            panelHost = factory.CreateRoot(contentRoot, "PanelHost");
            var panelLayout = panelHost.gameObject.AddComponent<VerticalLayoutGroup>();
            panelLayout.childControlHeight = true;
            panelLayout.childForceExpandHeight = true;
            panelLayout.padding = new RectOffset((int)theme.PaddingMedium, (int)theme.PaddingMedium, (int)theme.PaddingMedium, 0);
            var panelLayoutElement = panelHost.gameObject.AddComponent<LayoutElement>();
            panelLayoutElement.flexibleHeight = 1f;

            contentCanvasGroup = panelHost.gameObject.AddComponent<CanvasGroup>();
            bottomNav = BuildBottomNav(contentRoot);
        }

        private RectTransform BuildBottomNav(RectTransform parent)
        {
            var nav = factory.CreatePanel(parent, theme.Surface, 0f, "BottomNav");
            nav.anchorMin = new Vector2(0f, 0f);
            nav.anchorMax = new Vector2(1f, 0f);
            nav.pivot = new Vector2(0.5f, 0f);
            nav.sizeDelta = new Vector2(0f, 72f);
            nav.anchoredPosition = Vector2.zero;

            var layout = nav.gameObject.AddComponent<HorizontalLayoutGroup>();
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlWidth = true;
            layout.childForceExpandWidth = true;
            layout.spacing = 4f;
            layout.padding = new RectOffset(8, 8, 8, 8);

            CreateNavButton(nav, "Home", AppTab.Home);
            CreateNavButton(nav, "Categories", AppTab.Categories);
            CreateNavButton(nav, "Favorites", AppTab.Favorites);
            return nav;
        }

        private void CreateNavButton(RectTransform parent, string label, AppTab tab)
        {
            var button = factory.CreateButton(parent, label, () => navigator.ShowTab(tab));
            button.name = $"Nav_{label}";
        }

        private void Refresh()
        {
            StopAllCoroutines();
            StartCoroutine(AnimateRefresh());
        }

        private System.Collections.IEnumerator AnimateRefresh()
        {
            if (contentCanvasGroup != null)
            {
                contentCanvasGroup.alpha = 0.92f;
            }

            RebuildPanel();
            UpdateBottomNav();
            Canvas.ForceUpdateCanvases();

            yield return null;
            if (contentCanvasGroup != null)
            {
                contentCanvasGroup.alpha = 1f;
            }
        }

        private void UpdateBottomNav()
        {
            var showNav = navigator.CurrentScreen is AppScreen.Home or AppScreen.Categories or AppScreen.Favorites;
            bottomNav.gameObject.SetActive(showNav);
        }

        private void RebuildPanel()
        {
            foreach (Transform child in panelHost)
            {
                Destroy(child.gameObject);
            }

            switch (navigator.CurrentScreen)
            {
                case AppScreen.Home:
                    BuildHomePanel();
                    break;
                case AppScreen.Categories:
                    BuildCategoriesPanel();
                    break;
                case AppScreen.CategoryTerms:
                    BuildCategoryTermsPanel(navigator.SelectedCategory.GetValueOrDefault());
                    break;
                case AppScreen.Favorites:
                    BuildFavoritesPanel();
                    break;
                case AppScreen.SearchResults:
                    BuildSearchResultsPanel(navigator.SearchQuery);
                    break;
                case AppScreen.TermDetail:
                    BuildTermDetailPanel(navigator.SelectedTermId);
                    break;
            }
        }

        private RectTransform CreateScrollPanel(string name)
        {
            var scrollRoot = factory.CreateRoot(panelHost, name);
            var scroll = scrollRoot.gameObject.AddComponent<ScrollRect>();
            scroll.horizontal = false;
            scroll.movementType = ScrollRect.MovementType.Clamped;

            var viewport = factory.CreateRoot(scrollRoot, "Viewport");
            var viewportImage = viewport.gameObject.AddComponent<Image>();
            viewportImage.color = Color.clear;
            viewportImage.raycastTarget = true;
            viewport.gameObject.AddComponent<RectMask2D>();
            scroll.viewport = viewport;
            UiFactory.Stretch(viewport);

            var content = factory.CreateRoot(viewport, "Content");
            content.anchorMin = new Vector2(0f, 1f);
            content.anchorMax = new Vector2(1f, 1f);
            content.pivot = new Vector2(0.5f, 1f);
            var layout = content.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.spacing = theme.PaddingMedium;
            layout.padding = new RectOffset(0, 0, 0, (int)theme.PaddingLarge);
            layout.childControlWidth = true;
            layout.childForceExpandWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandHeight = false;
            content.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            scroll.content = content;

            var scrollLayout = scrollRoot.gameObject.AddComponent<LayoutElement>();
            scrollLayout.flexibleHeight = 1f;
            return content;
        }

        private void BuildHomePanel()
        {
            var content = CreateScrollPanel("HomePanel");
            factory.CreateText(content, "GAME TERMS", theme.TextPrimary, theme.TitleSize, theme.SansBold, TextAlignmentOptions.MidlineLeft);
            factory.CreateText(content, "Game Development Terms Explained", theme.TextSecondary, theme.BodySize, theme.SansRegular, TextAlignmentOptions.MidlineLeft);

            var search = factory.CreateSearchField(content, query =>
            {
                currentSearchQuery = query;
                if (!string.IsNullOrWhiteSpace(query))
                {
                    navigator.ShowSearch(query);
                }
            });
            var searchLayout = search.gameObject.AddComponent<LayoutElement>();
            searchLayout.minHeight = theme.MinTouchTarget;

            factory.CreateSectionHeader(content, "Explore Categories");
            foreach (var category in CategoryMetadata.AllCategories)
            {
                BuildCategoryCard(content, category);
            }

            factory.CreateSectionHeader(content, "Term of the Day");
            var daily = services.DailyTerm.GetTermOfTheDay();
            if (daily != null)
            {
                BuildTermRow(content, daily, null);
            }

            var randomButton = factory.CreateButton(content, "Random Term", () =>
            {
                var randomTerm = services.RandomTerm.GetRandomTerm();
                if (randomTerm != null)
                {
                    navigator.ShowTerm(randomTerm.Id);
                }
            }, true);
            var randomLayout = randomButton.gameObject.AddComponent<LayoutElement>();
            randomLayout.minHeight = theme.MinTouchTarget;

            var recentIds = services.RecentlyViewed.GetRecentIds();
            if (recentIds.Count > 0)
            {
                factory.CreateSectionHeader(content, "Recently Viewed");
                foreach (var id in recentIds)
                {
                    var term = services.Glossary.GetTerm(id);
                    if (term != null)
                    {
                        BuildTermRow(content, term, null);
                    }
                }
            }
        }

        private void BuildCategoriesPanel()
        {
            var content = CreateScrollPanel("CategoriesPanel");
            factory.CreateText(content, "Categories", theme.TextPrimary, theme.TitleSize, theme.SansBold, TextAlignmentOptions.MidlineLeft);
            factory.CreateText(content, "Browse terms by discipline", theme.TextSecondary, theme.BodySize, theme.SansRegular, TextAlignmentOptions.MidlineLeft);

            foreach (var category in CategoryMetadata.AllCategories)
            {
                BuildCategoryCard(content, category);
            }
        }

        private void BuildCategoryCard(RectTransform parent, GlossaryCategory category)
        {
            var card = factory.CreateCard(parent);
            var button = card.gameObject.AddComponent<Button>();
            button.targetGraphic = card.GetComponent<Image>();
            button.onClick.AddListener(() => navigator.ShowCategory(category));

            var layout = card.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(16, 16, 16, 16);
            layout.spacing = 8f;

            var header = factory.CreateRoot(card, "Header");
            var headerLayout = header.gameObject.AddComponent<HorizontalLayoutGroup>();
            headerLayout.spacing = 12f;
            headerLayout.childAlignment = TextAnchor.MiddleLeft;

            var iconBox = factory.CreatePanel(header, theme.PrimaryMuted, 10f, "Icon");
            iconBox.sizeDelta = new Vector2(44f, 44f);
            var iconText = factory.CreateText(iconBox, CategoryMetadata.GetIconLabel(category), theme.Primary, theme.MetaSize, theme.SansSemiBold, TextAlignmentOptions.Center);
            UiFactory.Stretch(iconText.rectTransform);

            var textColumn = factory.CreateRoot(header, "TextColumn");
            var textLayout = textColumn.gameObject.AddComponent<VerticalLayoutGroup>();
            textLayout.spacing = 4f;
            var textLayoutElement = textColumn.gameObject.AddComponent<LayoutElement>();
            textLayoutElement.flexibleWidth = 1f;

            factory.CreateText(textColumn, CategoryMetadata.GetDisplayName(category).ToUpperInvariant(), theme.TextPrimary, theme.BodySize, theme.SansSemiBold, TextAlignmentOptions.MidlineLeft);
            factory.CreateText(textColumn, $"{services.Glossary.GetCategoryCount(category)} terms", theme.TextMuted, theme.MetaSize, theme.SansRegular, TextAlignmentOptions.MidlineLeft);
        }

        private void BuildCategoryTermsPanel(GlossaryCategory category)
        {
            var content = CreateScrollPanel("CategoryTermsPanel");
            AddBackButton(content);
            factory.CreateText(content, CategoryMetadata.GetDisplayName(category).ToUpperInvariant(), theme.TextPrimary, theme.TitleSize, theme.SansBold, TextAlignmentOptions.MidlineLeft);

            var sortRow = factory.CreateRoot(content, "SortRow");
            var sortLayout = sortRow.gameObject.AddComponent<HorizontalLayoutGroup>();
            sortLayout.spacing = 8f;
            sortLayout.childControlWidth = true;
            sortLayout.childForceExpandWidth = true;

            factory.CreateButton(sortRow, "A-Z", () => { currentSortMode = TermSortMode.Alphabetical; RebuildPanel(); });
            factory.CreateButton(sortRow, "Difficulty", () => { currentSortMode = TermSortMode.Difficulty; RebuildPanel(); });
            factory.CreateButton(sortRow, "Recent", () => { currentSortMode = TermSortMode.RecentlyAdded; RebuildPanel(); });

            var terms = services.Glossary.SortTerms(services.Glossary.GetTermsByCategory(category), currentSortMode);
            foreach (var term in terms)
            {
                BuildTermRow(content, term, null);
            }
        }

        private void BuildFavoritesPanel()
        {
            var content = CreateScrollPanel("FavoritesPanel");
            factory.CreateText(content, "Favorites", theme.TextPrimary, theme.TitleSize, theme.SansBold, TextAlignmentOptions.MidlineLeft);

            var favoriteIds = services.Favorites.GetFavoriteIds();
            if (favoriteIds.Count == 0)
            {
                factory.CreateEmptyState(content, "No favorites yet", "Tap the bookmark icon on any term to save it here.");
                return;
            }

            foreach (var id in favoriteIds)
            {
                var term = services.Glossary.GetTerm(id);
                if (term != null)
                {
                    BuildTermRow(content, term, null);
                }
            }
        }

        private void BuildSearchResultsPanel(string query)
        {
            var content = CreateScrollPanel("SearchResultsPanel");
            AddBackButton(content);
            factory.CreateText(content, "Search Results", theme.TextPrimary, theme.TitleSize, theme.SansBold, TextAlignmentOptions.MidlineLeft);

            var results = services.Search.Search(query);
            if (results.Count == 0)
            {
                factory.CreateEmptyState(content, "No terms found", "Try searching for another keyword.");
                return;
            }

            foreach (var result in results)
            {
                BuildTermRow(content, result.Term, query);
            }
        }

        private void BuildTermDetailPanel(string termId)
        {
            var term = services.Glossary.GetTerm(termId);
            if (term == null)
            {
                navigator.GoBack();
                return;
            }

            services.RecentlyViewed.RecordView(term.Id);
            var content = CreateScrollPanel("TermDetailPanel");
            AddBackButton(content);

            var headerRow = factory.CreateRoot(content, "HeaderRow");
            var headerLayout = headerRow.gameObject.AddComponent<HorizontalLayoutGroup>();
            headerLayout.childAlignment = TextAnchor.MiddleLeft;
            headerLayout.childControlWidth = true;
            headerLayout.childForceExpandWidth = true;

            var titleColumn = factory.CreateRoot(headerRow, "TitleColumn");
            var titleLayout = titleColumn.gameObject.AddComponent<VerticalLayoutGroup>();
            titleLayout.spacing = 6f;
            var titleLayoutElement = titleColumn.gameObject.AddComponent<LayoutElement>();
            titleLayoutElement.flexibleWidth = 1f;

            factory.CreateText(titleColumn, term.Term, theme.TextPrimary, theme.TermSize, theme.SansSemiBold, TextAlignmentOptions.MidlineLeft);
            factory.CreateText(titleColumn, CategoryMetadata.GetDisplayName(term.Category).ToUpperInvariant(), theme.TextSecondary, theme.MetaSize, theme.SansSemiBold, TextAlignmentOptions.MidlineLeft);
            factory.CreateDifficultyBadge(titleColumn, term.Difficulty);

            var favoriteLabel = services.Favorites.IsFavorite(term.Id) ? "★ Saved" : "☆ Save";
            factory.CreateButton(headerRow, favoriteLabel, () => services.Favorites.ToggleFavorite(term.Id));

            AddSection(content, "What is it?", term.ShortDefinition);
            if (!string.IsNullOrWhiteSpace(term.SimpleExplanation))
            {
                AddSection(content, "In Simple Words", term.SimpleExplanation);
            }

            if (!string.IsNullOrWhiteSpace(term.WhyItMatters))
            {
                AddSection(content, "Why It Matters", term.WhyItMatters);
            }

            if (!string.IsNullOrWhiteSpace(term.Example))
            {
                AddSection(content, "Example", term.Example);
            }

            if (term.HasDiagram && term.DiagramType != DiagramType.None)
            {
                factory.CreateSectionHeader(content, "Example");
                DiagramRenderer.Create(content, term.DiagramType, theme.Primary, theme.SurfaceElevated);
            }

            if (term.GameUses != null && term.GameUses.Count > 0)
            {
                factory.CreateSectionHeader(content, "Game Uses");
                foreach (var use in term.GameUses)
                {
                    factory.CreateText(content, $"• {use}", theme.TextSecondary, theme.BodySize, theme.SansRegular, TextAlignmentOptions.MidlineLeft);
                }
            }

            if (!string.IsNullOrWhiteSpace(term.CodeExample))
            {
                factory.CreateSectionHeader(content, "Code Example");
                var codeCard = factory.CreateCard(content);
                var codeLayout = codeCard.gameObject.AddComponent<VerticalLayoutGroup>();
                codeLayout.padding = new RectOffset(14, 14, 14, 14);
                factory.CreateText(codeCard, term.CodeExample, theme.Accent, theme.MetaSize, theme.MonoRegular, TextAlignmentOptions.TopLeft);
            }

            var related = services.Glossary.GetRelatedTerms(term);
            if (related.Count > 0)
            {
                factory.CreateSectionHeader(content, "Related Terms");
                foreach (var relatedTerm in related)
                {
                    BuildTermRow(content, relatedTerm, null);
                }
            }
        }

        private void AddSection(RectTransform parent, string title, string body)
        {
            factory.CreateSectionHeader(parent, title);
            factory.CreateText(parent, body, theme.TextSecondary, theme.BodySize, theme.SansRegular, TextAlignmentOptions.TopLeft);
        }

        private void BuildTermRow(RectTransform parent, GlossaryTermData term, string highlightQuery)
        {
            var card = factory.CreateCard(parent);
            var button = card.gameObject.AddComponent<Button>();
            button.targetGraphic = card.GetComponent<Image>();
            button.onClick.AddListener(() => navigator.ShowTerm(term.Id));

            var layout = card.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(14, 14, 14, 14);
            layout.spacing = 6f;

            var termLabel = string.IsNullOrWhiteSpace(highlightQuery)
                ? term.Term
                : SearchService.HighlightMatches(term.Term, highlightQuery);
            factory.CreateText(card, termLabel, theme.TextPrimary, theme.BodySize, theme.SansSemiBold, TextAlignmentOptions.MidlineLeft);

            var meta = $"{CategoryMetadata.GetDisplayName(term.Category)} • {DifficultyMetadata.GetLabel(term.Difficulty)}";
            factory.CreateText(card, meta, theme.TextMuted, theme.MetaSize, theme.SansRegular, TextAlignmentOptions.MidlineLeft);

            if (!string.IsNullOrWhiteSpace(term.ShortDefinition))
            {
                var definition = string.IsNullOrWhiteSpace(highlightQuery)
                    ? term.ShortDefinition
                    : SearchService.HighlightMatches(term.ShortDefinition, highlightQuery);
                factory.CreateText(card, definition, theme.TextSecondary, theme.MetaSize, theme.SansRegular, TextAlignmentOptions.TopLeft);
            }
        }

        private void AddBackButton(RectTransform parent)
        {
            var back = factory.CreateButton(parent, "← Back", () => navigator.GoBack());
            var layout = back.gameObject.AddComponent<LayoutElement>();
            layout.minHeight = theme.MinTouchTarget;
        }

        private void EnsureThemeFonts()
        {
            if (theme == null)
            {
                return;
            }

            var fallback = TMP_Settings.defaultFontAsset;
            theme.SansRegular = FontAssetUtility.GetUsableFont(theme.SansRegular, fallback);
            theme.SansSemiBold = FontAssetUtility.GetUsableFont(theme.SansSemiBold, theme.SansRegular);
            theme.SansBold = FontAssetUtility.GetUsableFont(theme.SansBold, theme.SansRegular);
            theme.MonoRegular = FontAssetUtility.GetUsableFont(theme.MonoRegular, theme.SansRegular);
        }
    }
}
