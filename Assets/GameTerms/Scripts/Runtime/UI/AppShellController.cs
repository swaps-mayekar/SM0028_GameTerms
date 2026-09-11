using System;
using System.Collections;
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
        private readonly List<NavTabButton> navButtons = new();
        private bool quizAdvanceQueued;

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
            EnsureThemeColors();

            services = new AppServices(database, glossaryJson);
            navigator = new AppNavigator();
            BuildUi();
            navigator.Changed += OnNavigatorChanged;
            services.Favorites.Changed += Refresh;
            services.RecentlyViewed.Changed += Refresh;
            services.Progress.Changed += Refresh;
            Refresh();
        }

        private void OnNavigatorChanged()
        {
            if (navigator.CurrentScreen == AppScreen.TermDetail && !string.IsNullOrEmpty(navigator.SelectedTermId))
            {
                services.RecentlyViewed.RecordView(navigator.SelectedTermId);
            }

            if (navigator.CurrentScreen == AppScreen.FlashcardSession)
            {
                services.Flashcards.StartSession(navigator.ActiveStudyConfig ?? new StudyConfig
                {
                    Mode = StudyMode.Flashcards,
                    CardCount = 10,
                    Scope = StudyScope.DueForReview
                });
            }

            if (navigator.CurrentScreen == AppScreen.QuizSession)
            {
                services.Quiz.StartSession(navigator.ActiveStudyConfig ?? new StudyConfig
                {
                    Mode = StudyMode.Quiz,
                    QuestionCount = 5,
                    Scope = StudyScope.All
                });
                quizAdvanceQueued = false;
            }

            Refresh();
        }

        private void OnDestroy()
        {
            if (navigator != null)
            {
                navigator.Changed -= OnNavigatorChanged;
            }

            if (services?.Favorites != null)
            {
                services.Favorites.Changed -= Refresh;
            }

            if (services?.RecentlyViewed != null)
            {
                services.RecentlyViewed.Changed -= Refresh;
            }

            if (services?.Progress != null)
            {
                services.Progress.Changed -= Refresh;
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
            scaler.matchWidthOrHeight = 0f;

            if (FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                new GameObject("EventSystem", typeof(UnityEngine.EventSystems.EventSystem), typeof(UnityEngine.InputSystem.UI.InputSystemUIInputModule));
            }

            contentRoot = factory.CreateRoot(canvas.transform, "SafeAreaRoot");
            contentRoot.gameObject.AddComponent<SafeAreaFitter>();

            var background = factory.CreateImage(contentRoot, theme.Background, "Background");
            UiFactory.Stretch(background.rectTransform);

            var contentFrame = factory.CreateRoot(contentRoot, "ContentFrame");
            ApplyContentMaxWidth(contentFrame);

            panelHost = factory.CreateRoot(contentFrame, "PanelHost");
            contentCanvasGroup = panelHost.gameObject.AddComponent<CanvasGroup>();
            bottomNav = BuildBottomNav(contentFrame);
            ApplyPanelInsets(true);
        }

        private void ApplyContentMaxWidth(RectTransform frame)
        {
            var maxWidth = theme.GetContentMaxWidth(Screen.width);
            if (maxWidth >= Screen.width - 1f)
            {
                UiFactory.Stretch(frame);
                return;
            }

            frame.anchorMin = new Vector2(0.5f, 0f);
            frame.anchorMax = new Vector2(0.5f, 1f);
            frame.pivot = new Vector2(0.5f, 0.5f);
            frame.sizeDelta = new Vector2(maxWidth, 0f);
            frame.anchoredPosition = Vector2.zero;
        }

        private void ApplyPanelInsets(bool showNav)
        {
            var padding = theme.PaddingMedium;
            var bottom = showNav ? theme.NavHeight : 0f;
            panelHost.offsetMin = new Vector2(padding, bottom);
            panelHost.offsetMax = new Vector2(-padding, -padding);
        }

        private RectTransform BuildBottomNav(RectTransform parent)
        {
            var navImage = factory.CreateImage(parent, theme.Surface, "BottomNav");
            RoundedRectUtility.Apply(navImage);
            var nav = navImage.rectTransform;
            nav.anchorMin = new Vector2(0f, 0f);
            nav.anchorMax = new Vector2(1f, 0f);
            nav.pivot = new Vector2(0.5f, 0f);
            nav.anchoredPosition = Vector2.zero;
            nav.sizeDelta = new Vector2(0f, theme.NavHeight);

            var navLayout = nav.gameObject.AddComponent<VerticalLayoutGroup>();
            navLayout.childControlHeight = true;
            navLayout.childForceExpandHeight = true;
            navLayout.childControlWidth = true;
            navLayout.childForceExpandWidth = true;
            navLayout.spacing = 0f;
            navLayout.padding = new RectOffset(0, 0, 0, 0);

            factory.CreateDivider(nav);

            var buttonRow = factory.CreateRoot(nav, "NavButtons");
            var rowLayout = buttonRow.gameObject.AddComponent<LayoutElement>();
            rowLayout.flexibleHeight = 1f;
            rowLayout.minHeight = 0f;

            var layout = buttonRow.gameObject.AddComponent<HorizontalLayoutGroup>();
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlWidth = true;
            layout.childForceExpandWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandHeight = true;
            layout.spacing = 4f;
            layout.padding = new RectOffset(8, 8, 4, 6);

            navButtons.Clear();
            navButtons.Add(factory.CreateNavButton(buttonRow, UiIconId.Home, "Home", AppTab.Home, () => navigator.ShowTab(AppTab.Home)));
            navButtons.Add(factory.CreateNavButton(buttonRow, UiIconId.Categories, "Categories", AppTab.Categories, () => navigator.ShowTab(AppTab.Categories)));
            navButtons.Add(factory.CreateNavButton(buttonRow, UiIconId.Study, "Study", AppTab.Study, () => navigator.ShowTab(AppTab.Study)));
            navButtons.Add(factory.CreateNavButton(buttonRow, UiIconId.HeartOutline, "Favorites", AppTab.Favorites, () => navigator.ShowTab(AppTab.Favorites)));

            return nav;
        }

        private void Refresh()
        {
            StopAllCoroutines();
            StartCoroutine(AnimateRefresh());
        }

        private IEnumerator AnimateRefresh()
        {
            if (contentCanvasGroup != null)
            {
                contentCanvasGroup.alpha = 0f;
            }

            RebuildPanel();
            UpdateBottomNav();
            Canvas.ForceUpdateCanvases();

            const float duration = 0.18f;
            var elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                if (contentCanvasGroup != null)
                {
                    contentCanvasGroup.alpha = Mathf.SmoothStep(0f, 1f, elapsed / duration);
                }

                yield return null;
            }

            if (contentCanvasGroup != null)
            {
                contentCanvasGroup.alpha = 1f;
            }
        }

        private void UpdateBottomNav()
        {
            var showNav = navigator.CurrentScreen is AppScreen.Home or AppScreen.Categories or AppScreen.Favorites or AppScreen.StudyHub;
            bottomNav.gameObject.SetActive(showNav);
            ApplyPanelInsets(showNav);

            foreach (var navButton in navButtons)
            {
                navButton.SetSelected(navButton.Tab == navigator.CurrentTab, theme);
            }
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
                case AppScreen.StudyHub:
                    BuildStudyHubPanel();
                    break;
                case AppScreen.FlashcardSession:
                    BuildFlashcardSessionPanel();
                    break;
                case AppScreen.QuizSession:
                    BuildQuizSessionPanel();
                    break;
                case AppScreen.QuizResults:
                    BuildQuizResultsPanel();
                    break;
            }
        }

        private RectTransform CreateScrollPanel(string name)
        {
            var scrollRoot = factory.CreateRoot(panelHost, name);
            var scroll = scrollRoot.gameObject.AddComponent<ScrollRect>();
            scroll.horizontal = false;
            scroll.movementType = ScrollRect.MovementType.Clamped;
            scroll.scrollSensitivity = 24f;

            var scrollLayout = scrollRoot.gameObject.AddComponent<LayoutElement>();
            scrollLayout.flexibleHeight = 1f;
            scrollLayout.flexibleWidth = 1f;

            var viewport = factory.CreateRoot(scrollRoot, "Viewport");
            var viewportImage = viewport.gameObject.AddComponent<Image>();
            viewportImage.color = Color.clear;
            viewportImage.raycastTarget = true;
            viewport.gameObject.AddComponent<RectMask2D>();
            scroll.viewport = viewport;
            UiFactory.Stretch(viewport);

            var content = factory.CreateLayoutChild(viewport, "Content");
            var layout = content.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.spacing = theme.PaddingMedium;
            layout.padding = new RectOffset(0, 0, 0, (int)theme.PaddingLarge);
            layout.childControlWidth = true;
            layout.childForceExpandWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandHeight = false;
            content.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            scroll.content = content;

            return content;
        }

        private void BuildHomePanel()
        {
            var content = CreateScrollPanel("HomePanel");
            factory.CreateScreenHeader(content, "GAME TERMS", "Game Development Terms Explained");

            factory.CreateSearchField(content, query =>
            {
                currentSearchQuery = query;
                if (!string.IsNullOrWhiteSpace(query))
                {
                    navigator.ShowSearch(query);
                }
            }, currentSearchQuery);

            var stats = services.Progress.GetOverallStats();
            factory.CreateSectionHeader(content, "Continue Learning");
            var practiceCard = factory.CreateCard(content, "PracticeCard");
            var practiceLayout = practiceCard.gameObject.AddComponent<VerticalLayoutGroup>();
            practiceLayout.padding = new RectOffset(16, 16, 16, 16);
            practiceLayout.spacing = 10f;
            practiceLayout.childControlWidth = true;
            practiceLayout.childForceExpandWidth = true;

            factory.CreateText(practiceCard, $"{stats.MasteredCount} of {stats.TotalTerms} terms mastered", theme.TextPrimary, theme.BodySize, theme.SansSemiBold, TextAlignmentOptions.MidlineLeft);
            factory.CreateText(practiceCard, stats.DueCount > 0
                ? $"{stats.DueCount} term{(stats.DueCount == 1 ? string.Empty : "s")} due for review"
                : "Start a quiz or flashcard session to build mastery.", theme.TextSecondary, theme.MetaSize, theme.SansRegular, TextAlignmentOptions.MidlineLeft);
            factory.CreateProgressBar(practiceCard, stats.TotalTerms == 0 ? 0f : (float)stats.MasteredCount / stats.TotalTerms);
            factory.CreateButton(practiceCard, "Open Study", () => navigator.ShowStudyHub(), primary: true);

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
            randomButton.GetComponent<LayoutElement>().flexibleWidth = 1f;

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

        private void BuildStudyHubPanel()
        {
            var content = CreateScrollPanel("StudyHubPanel");
            factory.CreateScreenHeader(content, "Study", "Build lasting game-dev vocabulary");

            var stats = services.Progress.GetOverallStats();
            var statsRow = factory.CreateLayoutChild(content, "StatsRow");
            var statsLayout = statsRow.gameObject.AddComponent<HorizontalLayoutGroup>();
            statsLayout.spacing = 8f;
            statsLayout.childControlWidth = true;
            statsLayout.childForceExpandWidth = true;
            factory.CreateStatChip(statsRow, "Mastered", $"{stats.MasteredCount}/{stats.TotalTerms}");
            factory.CreateStatChip(statsRow, "Due", stats.DueCount.ToString());
            factory.CreateStatChip(statsRow, "Streak", $"{stats.CurrentStreakDays}d");

            if (stats.TotalQuizSessions > 0)
            {
                factory.CreateText(content, $"Best quiz: {stats.BestQuizScore}/5 · Last quiz: {stats.LastQuizScore}/5 · Sessions: {stats.TotalQuizSessions}", theme.TextMuted, theme.MetaSize, theme.SansRegular, TextAlignmentOptions.MidlineLeft);
            }

            factory.CreateSectionHeader(content, "Practice Modes");
            BuildStudyModeCard(content, UiIconId.Flip, "Flashcards", "Review terms at your pace with spaced repetition.", () =>
            {
                navigator.StartFlashcards(new StudyConfig
                {
                    Mode = StudyMode.Flashcards,
                    Scope = StudyScope.DueForReview,
                    CardCount = 10
                });
            });
            BuildStudyModeCard(content, UiIconId.Check, "Quick Quiz", "Answer 5 multiple-choice questions generated from the glossary.", () =>
            {
                navigator.StartQuiz(new StudyConfig
                {
                    Mode = StudyMode.Quiz,
                    Scope = StudyScope.All,
                    QuestionCount = 5
                });
            });

            if (stats.LastMissedTermIds.Count > 0)
            {
                factory.CreateSectionHeader(content, "Review Missed Terms");
                foreach (var id in stats.LastMissedTermIds)
                {
                    var term = services.Glossary.GetTerm(id);
                    if (term != null)
                    {
                        BuildTermRow(content, term, null);
                    }
                }
            }
        }

        private void BuildStudyModeCard(RectTransform parent, UiIconId icon, string title, string subtitle, Action onClick)
        {
            var card = factory.CreateCard(parent);
            var button = card.gameObject.AddComponent<Button>();
            var cardImage = card.GetComponent<Image>();
            button.targetGraphic = cardImage;
            ApplyCardButtonColors(button, cardImage.color);
            button.onClick.AddListener(() => onClick?.Invoke());

            var layout = card.gameObject.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(16, 16, 16, 16);
            layout.spacing = 12f;
            layout.childAlignment = TextAnchor.MiddleLeft;
            layout.childControlWidth = true;
            layout.childForceExpandWidth = true;
            layout.childControlHeight = true;

            var iconBox = factory.CreatePanel(card, theme.PrimaryMuted, "Icon");
            var iconBoxLayout = iconBox.gameObject.GetComponent<LayoutElement>();
            iconBoxLayout.minWidth = 44f;
            iconBoxLayout.minHeight = 44f;
            iconBoxLayout.preferredWidth = 44f;
            iconBoxLayout.preferredHeight = 44f;
            iconBoxLayout.flexibleWidth = 0f;
            var iconImage = factory.CreateIcon(iconBox, icon, theme.Primary, theme.SectionSize, "ModeIcon");
            UiFactory.Stretch(iconImage.rectTransform);

            var textColumn = factory.CreateLayoutChild(card, "TextColumn");
            var textLayout = textColumn.gameObject.AddComponent<VerticalLayoutGroup>();
            textLayout.spacing = 4f;
            textLayout.childControlWidth = true;
            textLayout.childForceExpandWidth = true;
            textColumn.gameObject.AddComponent<LayoutElement>().flexibleWidth = 1f;
            factory.CreateText(textColumn, title, theme.TextPrimary, theme.BodySize, theme.SansSemiBold, TextAlignmentOptions.MidlineLeft);
            factory.CreateText(textColumn, subtitle, theme.TextSecondary, theme.MetaSize, theme.SansRegular, TextAlignmentOptions.MidlineLeft);
        }

        private void BuildFlashcardSessionPanel()
        {
            var content = CreateScrollPanel("FlashcardSessionPanel");
            AddBackButton(content);

            if (services.Flashcards.IsComplete || services.Flashcards.DeckCount == 0)
            {
                factory.CreateScreenHeader(content, "Deck Complete");
                factory.CreateEmptyState(content, "Nice work", $"You reviewed {services.Flashcards.ReviewedThisSession} cards and marked {services.Flashcards.KnownThisSession} as known.", UiIconId.Check);
                factory.CreateButton(content, "Back to Study", () => navigator.ShowStudyHub(), primary: true);
                return;
            }

            var card = services.Flashcards.CurrentCard;
            factory.CreateScreenHeader(content, "Flashcards", $"Card {services.Flashcards.CurrentIndex + 1} of {services.Flashcards.DeckCount}");
            factory.CreateProgressBar(content, (float)services.Flashcards.CurrentIndex / Math.Max(1, services.Flashcards.DeckCount));

            var flashcard = factory.CreateCard(content, "Flashcard");
            var flashLayout = flashcard.gameObject.AddComponent<VerticalLayoutGroup>();
            flashLayout.padding = new RectOffset(20, 20, 24, 24);
            flashLayout.spacing = 12f;
            flashLayout.childControlWidth = true;
            flashLayout.childForceExpandWidth = true;
            flashcard.gameObject.AddComponent<LayoutElement>().minHeight = 220f;

            factory.CreateText(flashcard, CategoryMetadata.GetDisplayName(card.Category).ToUpperInvariant(), theme.TextMuted, theme.MetaSize, theme.SansSemiBold, TextAlignmentOptions.MidlineLeft);

            if (services.Flashcards.Side == FlashcardSide.Front)
            {
                factory.CreateText(flashcard, card.Term, theme.TextPrimary, theme.TermSize, theme.SansBold, TextAlignmentOptions.MidlineLeft);
                factory.CreateText(flashcard, "Tap Show Definition when you are ready.", theme.TextSecondary, theme.MetaSize, theme.SansRegular, TextAlignmentOptions.MidlineLeft);
                factory.CreateButton(content, "Show Definition", () =>
                {
                    services.Flashcards.Reveal();
                    Refresh();
                }, primary: true);
            }
            else
            {
                factory.CreateText(flashcard, card.Term, theme.TextPrimary, theme.BodySize, theme.SansSemiBold, TextAlignmentOptions.MidlineLeft);
                factory.CreateText(flashcard, card.ShortDefinition, theme.TextSecondary, theme.BodySize, theme.SansRegular, TextAlignmentOptions.TopLeft);
                if (!string.IsNullOrWhiteSpace(card.SimpleExplanation))
                {
                    factory.CreateText(flashcard, card.SimpleExplanation, theme.TextMuted, theme.MetaSize, theme.SansRegular, TextAlignmentOptions.TopLeft);
                }

                var actions = factory.CreateLayoutChild(content, "Actions");
                var actionsLayout = actions.gameObject.AddComponent<HorizontalLayoutGroup>();
                actionsLayout.spacing = 8f;
                actionsLayout.childControlWidth = true;
                actionsLayout.childForceExpandWidth = true;
                factory.CreateButton(actions, "Review Again", () =>
                {
                    services.Flashcards.MarkUnknown();
                    Refresh();
                });
                factory.CreateButton(actions, "Know It", () =>
                {
                    services.Flashcards.MarkKnown();
                    Refresh();
                }, primary: true);
            }
        }

        private void BuildQuizSessionPanel()
        {
            var content = CreateScrollPanel("QuizSessionPanel");
            AddBackButton(content);

            if (services.Quiz.IsComplete)
            {
                factory.CreateScreenHeader(content, "Quiz Complete");
                factory.CreateButton(content, "View Results", () => navigator.ShowQuizResults(), primary: true);
                return;
            }

            var question = services.Quiz.GetCurrentQuestion();
            if (question == null)
            {
                factory.CreateEmptyState(content, "No quiz available", "Add more glossary terms or try again later.", UiIconId.Search);
                factory.CreateButton(content, "Back to Study", () => navigator.ShowStudyHub(), primary: true);
                return;
            }

            factory.CreateScreenHeader(content, "Quick Quiz", $"Question {services.Quiz.CurrentIndex + 1} of {services.Quiz.QuestionCount}");
            factory.CreateProgressBar(content, (float)services.Quiz.CurrentIndex / Math.Max(1, services.Quiz.QuestionCount));
            factory.CreateText(content, question.Prompt, theme.TextPrimary, theme.BodySize, theme.SansSemiBold, TextAlignmentOptions.TopLeft);

            var awaiting = services.Quiz.IsAwaitingAdvance;
            foreach (var option in question.Options)
            {
                Color? background = null;
                Color? text = null;
                string label = option.Label;

                if (awaiting)
                {
                    if (option.Id == question.CorrectOptionId)
                    {
                        background = theme.SuccessMuted;
                        text = theme.Success;
                        label = $"{option.Label}  · Correct";
                    }
                    else if (option.Id == services.Quiz.LastSelectedOptionId)
                    {
                        background = theme.ErrorMuted;
                        text = theme.Error;
                        label = $"{option.Label}  · Incorrect";
                    }
                }

                var capturedId = option.Id;
                var button = factory.CreateQuizOption(content, label, () =>
                {
                    if (services.Quiz.IsAwaitingAdvance || quizAdvanceQueued)
                    {
                        return;
                    }

                    services.Quiz.SubmitAnswer(capturedId);
                    Refresh();
                    if (!quizAdvanceQueued)
                    {
                        quizAdvanceQueued = true;
                        StartCoroutine(AdvanceQuizAfterDelay());
                    }
                }, background, text);

                if (awaiting)
                {
                    button.interactable = false;
                }
            }
        }

        private IEnumerator AdvanceQuizAfterDelay()
        {
            yield return new WaitForSeconds(0.65f);
            quizAdvanceQueued = false;
            if (services.Quiz.Advance())
            {
                if (services.Quiz.IsComplete)
                {
                    navigator.ShowQuizResults();
                }
                else
                {
                    Refresh();
                }
            }
        }

        private void BuildQuizResultsPanel()
        {
            var content = CreateScrollPanel("QuizResultsPanel");
            AddBackButton(content);

            var result = services.Quiz.GetResults();
            factory.CreateScreenHeader(content, "Quiz Results", $"You scored {result.CorrectCount} of {result.TotalQuestions}");

            var summary = factory.CreateCard(content, "Summary");
            var summaryLayout = summary.gameObject.AddComponent<VerticalLayoutGroup>();
            summaryLayout.padding = new RectOffset(16, 16, 16, 16);
            summaryLayout.spacing = 8f;
            summaryLayout.childControlWidth = true;
            summaryLayout.childForceExpandWidth = true;
            factory.CreateText(summary, result.CorrectCount >= 4 ? "Strong recall." : result.CorrectCount >= 2 ? "Good start — keep practicing." : "Review the missed terms and try again.", theme.TextSecondary, theme.BodySize, theme.SansRegular, TextAlignmentOptions.MidlineLeft);
            factory.CreateProgressBar(summary, result.TotalQuestions == 0 ? 0f : (float)result.CorrectCount / result.TotalQuestions);

            if (result.MissedTermIds.Count > 0)
            {
                factory.CreateSectionHeader(content, "Missed Terms");
                foreach (var id in result.MissedTermIds)
                {
                    var term = services.Glossary.GetTerm(id);
                    if (term != null)
                    {
                        BuildTermRow(content, term, null);
                    }
                }
            }

            var actions = factory.CreateLayoutChild(content, "Actions");
            var actionsLayout = actions.gameObject.AddComponent<HorizontalLayoutGroup>();
            actionsLayout.spacing = 8f;
            actionsLayout.childControlWidth = true;
            actionsLayout.childForceExpandWidth = true;
            factory.CreateButton(actions, "Back to Study", () => navigator.ShowStudyHub());
            factory.CreateButton(actions, "Try Again", () =>
            {
                navigator.StartQuiz(navigator.ActiveStudyConfig ?? new StudyConfig
                {
                    Mode = StudyMode.Quiz,
                    QuestionCount = 5,
                    Scope = StudyScope.All
                });
            }, primary: true);
        }

        private void BuildCategoriesPanel()
        {
            var content = CreateScrollPanel("CategoriesPanel");
            factory.CreateScreenHeader(content, "Categories", "Browse terms by discipline");

            foreach (var category in CategoryMetadata.AllCategories)
            {
                BuildCategoryCard(content, category);
            }
        }

        private void BuildCategoryCard(RectTransform parent, GlossaryCategory category)
        {
            var card = factory.CreateCard(parent);
            var button = card.gameObject.AddComponent<Button>();
            var cardImage = card.GetComponent<Image>();
            button.targetGraphic = cardImage;
            ApplyCardButtonColors(button, cardImage.color);
            button.onClick.AddListener(() => navigator.ShowCategory(category));

            var layout = card.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(16, 16, 16, 16);
            layout.spacing = 8f;
            layout.childControlWidth = true;
            layout.childForceExpandWidth = true;

            var header = factory.CreateLayoutChild(card, "Header");
            var headerLayout = header.gameObject.AddComponent<HorizontalLayoutGroup>();
            headerLayout.spacing = 12f;
            headerLayout.childAlignment = TextAnchor.MiddleLeft;
            headerLayout.childControlWidth = true;
            headerLayout.childForceExpandWidth = true;
            headerLayout.childControlHeight = true;

            var iconBox = factory.CreatePanel(header, theme.PrimaryMuted, "Icon");
            var iconBoxLayout = iconBox.gameObject.GetComponent<LayoutElement>();
            iconBoxLayout.minWidth = 44f;
            iconBoxLayout.minHeight = 44f;
            iconBoxLayout.preferredWidth = 44f;
            iconBoxLayout.preferredHeight = 44f;
            iconBoxLayout.flexibleWidth = 0f;
            var iconText = factory.CreateText(iconBox, CategoryMetadata.GetIconLabel(category), theme.Primary, theme.MetaSize, theme.SansSemiBold, TextAlignmentOptions.Center);
            UiFactory.Stretch(iconText.rectTransform);

            var textColumn = factory.CreateLayoutChild(header, "TextColumn");
            var textLayout = textColumn.gameObject.AddComponent<VerticalLayoutGroup>();
            textLayout.spacing = 4f;
            textLayout.childControlWidth = true;
            textLayout.childForceExpandWidth = true;
            var textLayoutElement = textColumn.gameObject.AddComponent<LayoutElement>();
            textLayoutElement.flexibleWidth = 1f;

            factory.CreateText(textColumn, CategoryMetadata.GetDisplayName(category).ToUpperInvariant(), theme.TextPrimary, theme.BodySize, theme.SansSemiBold, TextAlignmentOptions.MidlineLeft);
            factory.CreateText(textColumn, $"{services.Glossary.GetCategoryCount(category)} terms", theme.TextMuted, theme.MetaSize, theme.SansRegular, TextAlignmentOptions.MidlineLeft);
        }

        private void BuildCategoryTermsPanel(GlossaryCategory category)
        {
            var content = CreateScrollPanel("CategoryTermsPanel");
            AddBackButton(content);
            factory.CreateScreenHeader(content, CategoryMetadata.GetDisplayName(category).ToUpperInvariant());

            var sortRow = factory.CreateLayoutChild(content, "SortRow");
            var sortLayout = sortRow.gameObject.AddComponent<HorizontalLayoutGroup>();
            sortLayout.spacing = 8f;
            sortLayout.childControlWidth = true;
            sortLayout.childForceExpandWidth = true;

            factory.CreateButton(sortRow, "A-Z", () => { currentSortMode = TermSortMode.Alphabetical; RebuildPanel(); }, selected: currentSortMode == TermSortMode.Alphabetical);
            factory.CreateButton(sortRow, "Difficulty", () => { currentSortMode = TermSortMode.Difficulty; RebuildPanel(); }, selected: currentSortMode == TermSortMode.Difficulty);
            factory.CreateButton(sortRow, "Recent", () => { currentSortMode = TermSortMode.RecentlyAdded; RebuildPanel(); }, selected: currentSortMode == TermSortMode.RecentlyAdded);

            var terms = services.Glossary.SortTerms(services.Glossary.GetTermsByCategory(category), currentSortMode);
            foreach (var term in terms)
            {
                BuildTermRow(content, term, null);
            }
        }

        private void BuildFavoritesPanel()
        {
            var content = CreateScrollPanel("FavoritesPanel");
            factory.CreateScreenHeader(content, "Favorites");

            var favoriteIds = services.Favorites.GetFavoriteIds();
            if (favoriteIds.Count == 0)
            {
                factory.CreateEmptyState(content, "No favorites yet", "Tap the heart on any term to save it here.", UiIconId.HeartOutline);
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
            factory.CreateScreenHeader(content, "Search Results");

            currentSearchQuery = query;
            factory.CreateSearchField(content, value =>
            {
                currentSearchQuery = value;
                if (string.IsNullOrWhiteSpace(value))
                {
                    navigator.GoBack();
                }
                else
                {
                    navigator.ShowSearch(value);
                }
            }, query);

            var results = services.Search.Search(query);
            if (results.Count == 0)
            {
                factory.CreateEmptyState(content, "No terms found", "Try a different keyword or browse categories.", UiIconId.Search);
                return;
            }

            factory.CreateText(content, $"{results.Count} result{(results.Count == 1 ? string.Empty : "s")}", theme.TextMuted, theme.MetaSize, theme.SansRegular, TextAlignmentOptions.MidlineLeft);

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

            var content = CreateScrollPanel("TermDetailPanel");
            AddBackButton(content);

            var headerRow = factory.CreateLayoutChild(content, "HeaderRow");
            var headerLayout = headerRow.gameObject.AddComponent<HorizontalLayoutGroup>();
            headerLayout.childAlignment = TextAnchor.MiddleLeft;
            headerLayout.childControlWidth = true;
            headerLayout.childForceExpandWidth = true;
            headerLayout.spacing = 12f;

            var titleColumn = factory.CreateLayoutChild(headerRow, "TitleColumn");
            var titleLayout = titleColumn.gameObject.AddComponent<VerticalLayoutGroup>();
            titleLayout.spacing = 6f;
            titleLayout.childControlWidth = true;
            titleLayout.childForceExpandWidth = true;
            var titleLayoutElement = titleColumn.gameObject.AddComponent<LayoutElement>();
            titleLayoutElement.flexibleWidth = 1f;

            factory.CreateText(titleColumn, term.Term, theme.TextPrimary, theme.TermSize, theme.SansSemiBold, TextAlignmentOptions.MidlineLeft);
            factory.CreateText(titleColumn, CategoryMetadata.GetDisplayName(term.Category).ToUpperInvariant(), theme.TextSecondary, theme.MetaSize, theme.SansSemiBold, TextAlignmentOptions.MidlineLeft);
            factory.CreateDifficultyBadge(titleColumn, term.Difficulty);

            factory.CreateFavoriteButton(headerRow, services.Favorites.IsFavorite(term.Id), () => services.Favorites.ToggleFavorite(term.Id));

            var mastery = services.Progress.GetMasteryLevel(term.Id);
            factory.CreateText(content, $"Mastery: {mastery}/5", theme.TextMuted, theme.MetaSize, theme.SansRegular, TextAlignmentOptions.MidlineLeft);

            factory.CreateButton(content, "Quiz This Term", () =>
            {
                navigator.StartQuiz(new StudyConfig
                {
                    Mode = StudyMode.Quiz,
                    QuestionCount = 1,
                    FocusTermId = term.Id,
                    Scope = StudyScope.All
                });
            });

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

            if (!string.IsNullOrWhiteSpace(term.CommonMistake))
            {
                AddSection(content, "Common Mistake", term.CommonMistake);
            }

            if (!string.IsNullOrWhiteSpace(term.PracticePrompt))
            {
                AddSection(content, "Try It Yourself", term.PracticePrompt);
            }

            if (term.HasDiagram && term.DiagramType != DiagramType.None)
            {
                factory.CreateSectionHeader(content, "Visual Example");
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
            var cardImage = card.GetComponent<Image>();
            button.targetGraphic = cardImage;
            ApplyCardButtonColors(button, cardImage.color);
            button.onClick.AddListener(() => navigator.ShowTerm(term.Id));

            var layout = card.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(14, 14, 14, 14);
            layout.spacing = 6f;
            layout.childControlWidth = true;
            layout.childForceExpandWidth = true;

            var termLabel = string.IsNullOrWhiteSpace(highlightQuery)
                ? term.Term
                : SearchService.HighlightMatches(term.Term, highlightQuery, theme.GetSearchHighlightHex());
            factory.CreateText(card, termLabel, theme.TextPrimary, theme.BodySize, theme.SansSemiBold, TextAlignmentOptions.MidlineLeft);

            var meta = $"{CategoryMetadata.GetDisplayName(term.Category)} • {DifficultyMetadata.GetLabel(term.Difficulty)}";
            factory.CreateText(card, meta, theme.TextMuted, theme.MetaSize, theme.SansRegular, TextAlignmentOptions.MidlineLeft);

            if (!string.IsNullOrWhiteSpace(term.ShortDefinition))
            {
                var definition = string.IsNullOrWhiteSpace(highlightQuery)
                    ? term.ShortDefinition
                    : SearchService.HighlightMatches(term.ShortDefinition, highlightQuery, theme.GetSearchHighlightHex());
                factory.CreateText(card, definition, theme.TextSecondary, theme.MetaSize, theme.SansRegular, TextAlignmentOptions.TopLeft);
            }
        }

        private void AddBackButton(RectTransform parent)
        {
            factory.CreateBackButton(parent, () => navigator.GoBack());
        }

        private void ApplyCardButtonColors(Button button, Color normalColor)
        {
            var colors = button.colors;
            colors.normalColor = normalColor;
            colors.highlightedColor = Color.Lerp(normalColor, theme.PrimaryMuted, 0.35f);
            colors.pressedColor = Color.Lerp(normalColor, theme.Primary, 0.15f);
            colors.selectedColor = normalColor;
            colors.fadeDuration = 0.1f;
            button.colors = colors;
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

        private void EnsureThemeColors()
        {
            if (theme == null)
            {
                return;
            }

            if (theme.Success.a <= 0.01f)
            {
                theme.Success = new Color(0.36f, 0.78f, 0.52f, 1f);
            }

            if (theme.SuccessMuted.a <= 0.01f)
            {
                theme.SuccessMuted = new Color(0.36f, 0.78f, 0.52f, 0.18f);
            }

            if (theme.Error.a <= 0.01f)
            {
                theme.Error = new Color(0.96f, 0.42f, 0.52f, 1f);
            }

            if (theme.ErrorMuted.a <= 0.01f)
            {
                theme.ErrorMuted = new Color(0.96f, 0.42f, 0.52f, 0.18f);
            }
        }
    }
}
