using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameTerms.UI
{
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

        public Button CreateButton(RectTransform parent, string label, Action onClick, bool primary = false)
        {
            var buttonGo = new GameObject("Button", typeof(RectTransform), typeof(Image), typeof(Button));
            var rect = buttonGo.GetComponent<RectTransform>();
            rect.SetParent(parent, false);
            var image = buttonGo.GetComponent<Image>();
            image.color = primary ? theme.Primary : theme.SurfaceElevated;
            image.raycastTarget = true;

            var button = buttonGo.GetComponent<Button>();
            button.targetGraphic = image;
            button.onClick.AddListener(() => onClick?.Invoke());
            buttonGo.AddComponent<ScrollDragForwarder>();

            var layout = buttonGo.AddComponent<LayoutElement>();
            layout.minHeight = theme.MinTouchTarget;

            var text = CreateText(rect, label, primary ? theme.Background : theme.TextPrimary, theme.ButtonSize, theme.SansSemiBold, TextAlignmentOptions.Center);
            Stretch(text.rectTransform);

            return button;
        }

        public TMP_InputField CreateSearchField(RectTransform parent, Action<string> onChanged)
        {
            var container = CreatePanel(parent, theme.Surface, 12f, "SearchField");
            var layout = container.gameObject.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(12, 12, 10, 10);
            layout.childAlignment = TextAnchor.MiddleLeft;
            layout.childControlHeight = true;
            layout.childForceExpandHeight = true;
            layout.spacing = 8f;

            var icon = CreateText(container, "⌕", theme.TextMuted, theme.BodySize, theme.SansRegular, TextAlignmentOptions.MidlineLeft);
            icon.rectTransform.sizeDelta = new Vector2(20f, 20f);

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

            var inputLayout = inputGo.AddComponent<LayoutElement>();
            inputLayout.flexibleWidth = 1f;
            inputLayout.minHeight = theme.MinTouchTarget;

            return input;
        }

        public RectTransform CreatePanel(RectTransform parent, Color color, float radius, string name = "Panel")
        {
            var image = CreateImage(parent, color, name);
            var rect = image.rectTransform;
            return rect;
        }

        public RectTransform CreateCard(RectTransform parent, string name = "Card")
        {
            var card = CreatePanel(parent, theme.Surface, theme.CornerRadius, name);
            var outline = card.gameObject.AddComponent<Outline>();
            outline.effectColor = theme.Border;
            outline.effectDistance = new Vector2(1f, -1f);
            card.gameObject.AddComponent<ScrollDragForwarder>();
            return card;
        }

        public TextMeshProUGUI CreateText(RectTransform parent, string text, Color color, float size, TMP_FontAsset font, TextAlignmentOptions alignment)
        {
            var go = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
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
            return label;
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
            var container = CreatePanel(parent, theme.SurfaceElevated, 8f, "DifficultyBadge");
            var layout = container.gameObject.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(10, 10, 6, 6);
            layout.spacing = 6f;
            layout.childAlignment = TextAnchor.MiddleLeft;

            CreateText(container, DifficultyMetadata.GetIcon(difficulty), DifficultyMetadata.GetColor(difficulty), theme.MetaSize, theme.SansSemiBold, TextAlignmentOptions.MidlineLeft);
            CreateText(container, DifficultyMetadata.GetLabel(difficulty), DifficultyMetadata.GetColor(difficulty), theme.MetaSize, theme.SansSemiBold, TextAlignmentOptions.MidlineLeft);

            return container;
        }

        public RectTransform CreateEmptyState(RectTransform parent, string title, string message)
        {
            var container = CreateRoot(parent, "EmptyState");
            var layout = container.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.spacing = 8f;
            layout.padding = new RectOffset(24, 24, 48, 48);

            CreateText(container, title, theme.TextPrimary, theme.SectionSize, theme.SansSemiBold, TextAlignmentOptions.Center);
            CreateText(container, message, theme.TextSecondary, theme.BodySize, theme.SansRegular, TextAlignmentOptions.Center);
            return container;
        }

        public static void Stretch(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }
    }
}
