using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameTerms.UI
{
    public sealed class NavTabButton
    {
        public AppTab Tab { get; }
        public Button Button { get; }
        public Image Background { get; }
        public Image Icon { get; }
        public TextMeshProUGUI Label { get; }

        public NavTabButton(AppTab tab, Button button, Image background, Image icon, TextMeshProUGUI label)
        {
            Tab = tab;
            Button = button;
            Background = background;
            Icon = icon;
            Label = label;
        }

        public void SetSelected(bool selected, UiTheme theme)
        {
            Background.color = selected ? theme.PrimaryMuted : Color.clear;
            var textColor = selected ? theme.NavActive : theme.NavInactive;
            Icon.color = textColor;
            Label.color = textColor;
        }
    }

    public sealed class UiFactory
    {
        private readonly UiTheme theme;
        private readonly float screenWidth;

        public UiFactory(UiTheme theme, float screenWidth)
        {
            this.theme = theme;
            this.screenWidth = screenWidth;
        }

        public RectTransform CreateRoot(Transform parent, string name)
        {
            var go = new GameObject(name, typeof(RectTransform));
            var rect = go.GetComponent<RectTransform>();
            rect.SetParent(parent, false);
            Stretch(rect);
            return rect;
        }

        public RectTransform CreateLayoutChild(RectTransform parent, string name)
        {
            var go = new GameObject(name, typeof(RectTransform));
            var rect = go.GetComponent<RectTransform>();
            rect.SetParent(parent, false);
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.pivot = new Vector2(0.5f, 1f);
            rect.sizeDelta = Vector2.zero;
            return rect;
        }

        public Image CreateImage(RectTransform parent, Color color, string name = "Image")
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            var rect = go.GetComponent<RectTransform>();
            rect.SetParent(parent, false);
            var image = go.GetComponent<Image>();
            image.color = color;
            image.raycastTarget = false;
            return image;
        }

        public Button CreateButton(RectTransform parent, string label, Action onClick, bool primary = false, bool selected = false)
        {
            var buttonGo = new GameObject("Button", typeof(RectTransform), typeof(Image), typeof(Button));
            var rect = buttonGo.GetComponent<RectTransform>();
            rect.SetParent(parent, false);
            var image = buttonGo.GetComponent<Image>();
            var normalColor = primary || selected ? theme.Primary : theme.SurfaceElevated;
            image.color = normalColor;
            image.raycastTarget = true;
            RoundedRectUtility.Apply(image);

            var button = buttonGo.GetComponent<Button>();
            button.targetGraphic = image;
            button.onClick.AddListener(() => onClick?.Invoke());
            ApplyButtonColors(button, normalColor);
            buttonGo.AddComponent<ScrollDragForwarder>();

            var layout = buttonGo.AddComponent<LayoutElement>();
            layout.minHeight = theme.MinTouchTarget;
            layout.flexibleWidth = 1f;

            var textColor = primary || selected ? theme.Background : theme.TextPrimary;
            var text = CreateText(rect, label, textColor, theme.ButtonSize, theme.SansSemiBold, TextAlignmentOptions.Center);
            Stretch(text.rectTransform);

            return button;
        }

        public NavTabButton CreateNavButton(RectTransform parent, UiIconId icon, string label, AppTab tab, Action onClick)
        {
            var buttonGo = new GameObject($"Nav_{label}", typeof(RectTransform), typeof(Button), typeof(LayoutElement));
            var rect = buttonGo.GetComponent<RectTransform>();
            rect.SetParent(parent, false);

            var buttonLayout = buttonGo.GetComponent<LayoutElement>();
            buttonLayout.flexibleWidth = 1f;
            buttonLayout.minHeight = 40f;

            var background = CreateImage(rect, Color.clear, "Background");
            RoundedRectUtility.Apply(background);
            background.raycastTarget = true;
            Stretch(background.rectTransform);
            background.gameObject.AddComponent<LayoutElement>().ignoreLayout = true;

            var button = buttonGo.GetComponent<Button>();
            button.targetGraphic = background;
            button.onClick.AddListener(() => onClick?.Invoke());
            ApplyButtonColors(button, Color.clear);

            var layout = buttonGo.AddComponent<VerticalLayoutGroup>();
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.spacing = 1f;
            layout.padding = new RectOffset(2, 2, 3, 3);
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            var iconImage = CreateIcon(rect, icon, theme.NavInactive, theme.NavIconSize, "Icon");
            var iconLayout = iconImage.gameObject.GetComponent<LayoutElement>();
            iconLayout.minHeight = 18f;
            iconLayout.preferredHeight = 18f;

            var textLabel = CreateText(rect, label, theme.NavInactive, theme.NavLabelSize, theme.SansSemiBold, TextAlignmentOptions.Center);
            var textLayout = textLabel.gameObject.AddComponent<LayoutElement>();
            textLayout.minHeight = 12f;
            textLayout.preferredHeight = 12f;

            return new NavTabButton(tab, button, background, iconImage, textLabel);
        }

        public TMP_InputField CreateSearchField(RectTransform parent, Action<string> onChanged, string initialValue = null)
        {
            var container = CreatePanel(parent, theme.Surface, "SearchField");
            var containerLayout = container.gameObject.AddComponent<LayoutElement>();
            containerLayout.minHeight = theme.MinTouchTarget;

            var layout = container.gameObject.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(14, 14, 10, 10);
            layout.childAlignment = TextAnchor.MiddleLeft;
            layout.childControlHeight = true;
            layout.childForceExpandHeight = true;
            layout.spacing = 10f;

            var searchIcon = CreateIcon(container, UiIconId.Search, theme.TextMuted, theme.BodySize, "SearchIcon");
            var searchIconLayout = searchIcon.gameObject.GetComponent<LayoutElement>();
            searchIconLayout.flexibleWidth = 0f;
            searchIconLayout.minWidth = 20f;
            searchIconLayout.preferredWidth = 20f;
            searchIconLayout.minHeight = 20f;
            searchIconLayout.preferredHeight = 20f;

            var inputGo = new GameObject("Input", typeof(RectTransform), typeof(Image), typeof(TMP_InputField));
            inputGo.transform.SetParent(container, false);
            var inputImage = inputGo.GetComponent<Image>();
            inputImage.color = new Color(0f, 0f, 0f, 0f);

            var textArea = new GameObject("Text Area", typeof(RectTransform), typeof(RectMask2D));
            textArea.transform.SetParent(inputGo.transform, false);
            var textAreaRect = textArea.GetComponent<RectTransform>();
            Stretch(textAreaRect);

            var placeholder = CreateText(textAreaRect, "Search terms...", theme.TextMuted, theme.BodySize, theme.SansRegular, TextAlignmentOptions.MidlineLeft);
            var text = CreateText(textAreaRect, string.Empty, theme.TextPrimary, theme.BodySize, theme.SansRegular, TextAlignmentOptions.MidlineLeft);
            Stretch(placeholder.rectTransform);
            Stretch(text.rectTransform);

            var input = inputGo.GetComponent<TMP_InputField>();
            input.textViewport = textAreaRect;
            input.textComponent = text;
            input.placeholder = placeholder;
            input.fontAsset = theme.SansRegular;
            input.onValueChanged.AddListener(value => onChanged?.Invoke(value));

            if (!string.IsNullOrEmpty(initialValue))
            {
                input.SetTextWithoutNotify(initialValue);
                text.text = initialValue;
            }

            var inputLayout = inputGo.AddComponent<LayoutElement>();
            inputLayout.flexibleWidth = 1f;
            inputLayout.minHeight = theme.MinTouchTarget;

            return input;
        }

        public RectTransform CreatePanel(RectTransform parent, Color color, string name = "Panel")
        {
            var image = CreateImage(parent, color, name);
            image.raycastTarget = false;
            RoundedRectUtility.Apply(image);
            var layout = image.gameObject.AddComponent<LayoutElement>();
            layout.flexibleWidth = 1f;
            return image.rectTransform;
        }

        public RectTransform CreateCard(RectTransform parent, string name = "Card")
        {
            var card = CreatePanel(parent, theme.Surface, name);
            card.GetComponent<Image>().raycastTarget = true;
            card.gameObject.AddComponent<ScrollDragForwarder>();
            return card;
        }

        public Button CreateFavoriteButton(RectTransform parent, bool isFavorite, Action onClick)
        {
            var buttonGo = new GameObject("FavoriteButton", typeof(RectTransform), typeof(Image), typeof(Button));
            var rect = buttonGo.GetComponent<RectTransform>();
            rect.SetParent(parent, false);

            var image = buttonGo.GetComponent<Image>();
            image.color = isFavorite ? new Color(theme.Favorite.r, theme.Favorite.g, theme.Favorite.b, 0.18f) : theme.SurfaceElevated;
            image.raycastTarget = true;
            RoundedRectUtility.Apply(image);

            var button = buttonGo.GetComponent<Button>();
            button.targetGraphic = image;
            button.onClick.AddListener(() => onClick?.Invoke());
            ApplyButtonColors(button, image.color);

            var layout = buttonGo.AddComponent<LayoutElement>();
            layout.minWidth = theme.MinTouchTarget;
            layout.minHeight = theme.MinTouchTarget;
            layout.preferredWidth = theme.MinTouchTarget;

            var iconColor = isFavorite ? theme.Favorite : theme.TextSecondary;
            var iconImage = CreateIcon(rect, isFavorite ? UiIconId.HeartFilled : UiIconId.HeartOutline, iconColor, theme.SectionSize, "FavoriteIcon");
            Stretch(iconImage.rectTransform);

            return button;
        }

        public Image CreateIcon(RectTransform parent, UiIconId icon, Color color, float size, string name = "Icon")
        {
            var image = CreateImage(parent, color, name);
            image.sprite = UiIconUtility.Get(icon);
            image.preserveAspect = true;

            if (image.gameObject.GetComponent<LayoutElement>() == null)
            {
                image.gameObject.AddComponent<LayoutElement>();
            }

            var layout = image.gameObject.GetComponent<LayoutElement>();
            var dimension = theme.GetResponsiveSize(size, screenWidth);
            layout.flexibleWidth = 0f;
            layout.minWidth = dimension;
            layout.preferredWidth = dimension;
            layout.minHeight = dimension;
            layout.preferredHeight = dimension;

            return image;
        }

        public Button CreateBackButton(RectTransform parent, Action onClick)
        {
            var buttonGo = new GameObject("BackButton", typeof(RectTransform), typeof(Image), typeof(Button));
            var rect = buttonGo.GetComponent<RectTransform>();
            rect.SetParent(parent, false);

            var image = buttonGo.GetComponent<Image>();
            image.color = theme.SurfaceElevated;
            image.raycastTarget = true;
            RoundedRectUtility.Apply(image);

            var button = buttonGo.GetComponent<Button>();
            button.targetGraphic = image;
            button.onClick.AddListener(() => onClick?.Invoke());
            ApplyButtonColors(button, theme.SurfaceElevated);

            var layout = buttonGo.AddComponent<LayoutElement>();
            layout.minHeight = theme.MinTouchTarget;

            var content = CreateLayoutChild(rect, "Content");
            Stretch(content);
            var contentLayout = content.gameObject.AddComponent<HorizontalLayoutGroup>();
            contentLayout.padding = new RectOffset(16, 16, 0, 0);
            contentLayout.spacing = 8f;
            contentLayout.childAlignment = TextAnchor.MiddleLeft;
            contentLayout.childControlHeight = true;
            contentLayout.childForceExpandHeight = true;

            var backIcon = CreateIcon(content, UiIconId.Back, theme.TextPrimary, theme.ButtonSize, "BackIcon");
            backIcon.gameObject.GetComponent<LayoutElement>().flexibleWidth = 0f;
            CreateText(content, "Back", theme.TextPrimary, theme.ButtonSize, theme.SansSemiBold, TextAlignmentOptions.MidlineLeft);

            return button;
        }

        public TextMeshProUGUI CreateText(RectTransform parent, string text, Color color, float size, TMP_FontAsset font, TextAlignmentOptions alignment)
        {
            var go = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI), typeof(LayoutElement));
            var rect = go.GetComponent<RectTransform>();
            rect.SetParent(parent, false);
            var label = go.GetComponent<TextMeshProUGUI>();
            label.text = text;
            label.color = color;
            label.font = font;
            label.fontSize = theme.GetResponsiveSize(size, screenWidth);
            label.alignment = alignment;
            label.richText = true;
            label.raycastTarget = false;
            label.textWrappingMode = TextWrappingModes.Normal;

            var layout = go.GetComponent<LayoutElement>();
            layout.flexibleWidth = 1f;

            return label;
        }

        public void CreateScreenHeader(RectTransform parent, string title, string subtitle = null)
        {
            CreateText(parent, title, theme.TextPrimary, theme.TitleSize, theme.SansBold, TextAlignmentOptions.MidlineLeft);
            if (!string.IsNullOrWhiteSpace(subtitle))
            {
                CreateText(parent, subtitle, theme.TextSecondary, theme.BodySize, theme.SansRegular, TextAlignmentOptions.MidlineLeft);
            }
        }

        public RectTransform CreateSectionHeader(RectTransform parent, string title)
        {
            var header = CreateText(parent, title, theme.TextSecondary, theme.SectionSize, theme.SansSemiBold, TextAlignmentOptions.MidlineLeft).rectTransform;
            var layout = header.gameObject.AddComponent<LayoutElement>();
            layout.minHeight = 28f;
            return header;
        }

        public RectTransform CreateDifficultyBadge(RectTransform parent, DifficultyLevel difficulty)
        {
            var container = CreatePanel(parent, theme.SurfaceElevated, "DifficultyBadge");
            var layout = container.gameObject.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(10, 10, 6, 6);
            layout.spacing = 6f;
            layout.childAlignment = TextAnchor.MiddleLeft;

            var dots = CreateLayoutChild(container, "Dots");
            var dotsLayout = dots.gameObject.AddComponent<HorizontalLayoutGroup>();
            dotsLayout.spacing = 3f;
            dotsLayout.childAlignment = TextAnchor.MiddleLeft;
            dots.gameObject.AddComponent<LayoutElement>().flexibleWidth = 0f;

            var dotColor = DifficultyMetadata.GetColor(difficulty);
            var dotCount = difficulty switch
            {
                DifficultyLevel.Beginner => 1,
                DifficultyLevel.Intermediate => 2,
                DifficultyLevel.Advanced => 3,
                _ => 1
            };

            for (var i = 0; i < dotCount; i++)
            {
                CreateIcon(dots, UiIconId.Dot, dotColor, theme.MetaSize, $"Dot_{i}");
            }

            CreateText(container, DifficultyMetadata.GetLabel(difficulty), dotColor, theme.MetaSize, theme.SansSemiBold, TextAlignmentOptions.MidlineLeft);

            return container;
        }

        public RectTransform CreateEmptyState(RectTransform parent, string title, string message, UiIconId icon)
        {
            var container = CreateLayoutChild(parent, "EmptyState");
            var layout = container.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.spacing = 10f;
            layout.padding = new RectOffset(24, 24, 48, 48);
            layout.childControlWidth = true;
            layout.childForceExpandWidth = true;

            var iconBox = CreatePanel(container, theme.PrimaryMuted, "EmptyIcon");
            var iconBoxLayout = iconBox.gameObject.GetComponent<LayoutElement>();
            iconBoxLayout.minWidth = 56f;
            iconBoxLayout.minHeight = 56f;
            iconBoxLayout.preferredWidth = 56f;
            iconBoxLayout.preferredHeight = 56f;
            iconBoxLayout.flexibleWidth = 0f;

            var iconImage = CreateIcon(iconBox, icon, theme.Primary, theme.SectionSize, "EmptyIconImage");
            var iconImageLayout = iconImage.gameObject.GetComponent<LayoutElement>();
            iconImageLayout.minWidth = 24f;
            iconImageLayout.preferredWidth = 24f;
            iconImageLayout.minHeight = 24f;
            iconImageLayout.preferredHeight = 24f;
            Stretch(iconImage.rectTransform);

            CreateText(container, title, theme.TextPrimary, theme.SectionSize, theme.SansSemiBold, TextAlignmentOptions.Center);
            CreateText(container, message, theme.TextSecondary, theme.BodySize, theme.SansRegular, TextAlignmentOptions.Center);
            return container;
        }

        public RectTransform CreateDivider(RectTransform parent)
        {
            var divider = CreateImage(parent, theme.Border, "Divider");
            divider.rectTransform.sizeDelta = new Vector2(0f, 1f);
            var layout = divider.gameObject.AddComponent<LayoutElement>();
            layout.minHeight = 1f;
            layout.preferredHeight = 1f;
            return divider.rectTransform;
        }

        public RectTransform CreateProgressBar(RectTransform parent, float progress, string name = "ProgressBar")
        {
            progress = Mathf.Clamp01(progress);
            var track = CreatePanel(parent, theme.SurfaceElevated, name);
            var trackLayout = track.gameObject.GetComponent<LayoutElement>();
            trackLayout.minHeight = 10f;
            trackLayout.preferredHeight = 10f;

            var fill = CreateImage(track, theme.Primary, "Fill");
            fill.raycastTarget = false;
            RoundedRectUtility.Apply(fill);
            var fillRect = fill.rectTransform;
            fillRect.anchorMin = new Vector2(0f, 0f);
            fillRect.anchorMax = new Vector2(progress, 1f);
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;
            return track;
        }

        public RectTransform CreateStatChip(RectTransform parent, string label, string value)
        {
            var chip = CreateCard(parent, "StatChip");
            var layout = chip.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(12, 12, 12, 12);
            layout.spacing = 4f;
            layout.childControlWidth = true;
            layout.childForceExpandWidth = true;

            CreateText(chip, value, theme.TextPrimary, theme.SectionSize, theme.SansSemiBold, TextAlignmentOptions.Center);
            CreateText(chip, label, theme.TextMuted, theme.MetaSize, theme.SansRegular, TextAlignmentOptions.Center);
            return chip;
        }

        public Button CreateQuizOption(RectTransform parent, string label, Action onClick, Color? backgroundOverride = null, Color? textOverride = null)
        {
            var buttonGo = new GameObject("QuizOption", typeof(RectTransform), typeof(Image), typeof(Button));
            var rect = buttonGo.GetComponent<RectTransform>();
            rect.SetParent(parent, false);
            var image = buttonGo.GetComponent<Image>();
            var normalColor = backgroundOverride ?? theme.SurfaceElevated;
            image.color = normalColor;
            image.raycastTarget = true;
            RoundedRectUtility.Apply(image);

            var button = buttonGo.GetComponent<Button>();
            button.targetGraphic = image;
            button.onClick.AddListener(() => onClick?.Invoke());
            ApplyButtonColors(button, normalColor);
            buttonGo.AddComponent<ScrollDragForwarder>();

            var layout = buttonGo.AddComponent<LayoutElement>();
            layout.minHeight = theme.MinTouchTarget;
            layout.flexibleWidth = 1f;

            var textColor = textOverride ?? theme.TextPrimary;
            var text = CreateText(rect, label, textColor, theme.BodySize, theme.SansSemiBold, TextAlignmentOptions.Center);
            Stretch(text.rectTransform);
            text.margin = new Vector4(12f, 8f, 12f, 8f);

            return button;
        }

        public static void Stretch(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        private void ApplyButtonColors(Button button, Color normalColor)
        {
            var colors = button.colors;
            colors.normalColor = normalColor;
            colors.highlightedColor = Color.Lerp(normalColor, Color.white, 0.08f);
            colors.pressedColor = Color.Lerp(normalColor, Color.black, 0.12f);
            colors.selectedColor = normalColor;
            colors.disabledColor = new Color(normalColor.r, normalColor.g, normalColor.b, 0.4f);
            colors.fadeDuration = 0.1f;
            button.colors = colors;
        }
    }
}
