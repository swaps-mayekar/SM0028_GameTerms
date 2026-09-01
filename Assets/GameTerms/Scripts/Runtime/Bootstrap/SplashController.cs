using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GameTerms.UI;

namespace GameTerms.Bootstrap
{
    public sealed class SplashController : MonoBehaviour
    {
        [SerializeField] private string mainSceneName = "1_MainScene";
        [SerializeField] private float minimumDisplaySeconds = 1.4f;
        [SerializeField] private UiTheme theme;

        private IEnumerator Start()
        {
            if (theme == null)
            {
                theme = Resources.Load<UiTheme>("UiTheme");
            }

            var canvasGroup = BuildSplashUi();
            yield return FadeIn(canvasGroup, 0.5f);
            yield return new WaitForSeconds(Mathf.Max(0f, minimumDisplaySeconds - 0.5f));
            yield return FadeOut(canvasGroup, 0.35f);
            SceneManager.LoadScene(mainSceneName);
        }

        private CanvasGroup BuildSplashUi()
        {
            var backgroundColor = theme != null ? theme.Background : new Color(0.07f, 0.08f, 0.11f);
            var accentColor = theme != null ? theme.Primary : new Color(0.36f, 0.78f, 1f);
            var textPrimary = theme != null ? theme.TextPrimary : Color.white;
            var textSecondary = theme != null ? theme.TextSecondary : new Color(0.68f, 0.73f, 0.8f);
            var titleFont = theme?.SansBold ?? TMP_Settings.defaultFontAsset;
            var bodyFont = theme?.SansRegular ?? TMP_Settings.defaultFontAsset;

            var canvasGo = new GameObject("SplashCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster), typeof(CanvasGroup));
            var canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(390f, 844f);
            scaler.matchWidthOrHeight = 0.5f;

            var canvasGroup = canvasGo.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0f;

            var background = new GameObject("Background", typeof(RectTransform), typeof(Image));
            background.transform.SetParent(canvasGo.transform, false);
            var bgRect = background.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.offsetMin = Vector2.zero;
            bgRect.offsetMax = Vector2.zero;
            background.GetComponent<Image>().color = backgroundColor;

            var stack = new GameObject("Stack", typeof(RectTransform), typeof(VerticalLayoutGroup));
            stack.transform.SetParent(canvasGo.transform, false);
            var stackRect = stack.GetComponent<RectTransform>();
            stackRect.anchorMin = new Vector2(0.5f, 0.5f);
            stackRect.anchorMax = new Vector2(0.5f, 0.5f);
            stackRect.sizeDelta = new Vector2(320f, 200f);
            var layout = stack.GetComponent<VerticalLayoutGroup>();
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.spacing = 12f;

            var iconBox = new GameObject("Icon", typeof(RectTransform), typeof(Image));
            iconBox.transform.SetParent(stack.transform, false);
            var iconRect = iconBox.GetComponent<RectTransform>();
            iconRect.sizeDelta = new Vector2(64f, 64f);
            var iconImage = iconBox.GetComponent<Image>();
            iconImage.color = theme != null ? theme.PrimaryMuted : new Color(accentColor.r, accentColor.g, accentColor.b, 0.18f);
            RoundedRectUtility.Apply(iconImage);
            CreateLabel(iconBox.transform, "GT", 22f, titleFont, accentColor, TextAlignmentOptions.Center, true);

            CreateLabel(stack.transform, "GAME TERMS", 32f, titleFont, textPrimary, TextAlignmentOptions.Center, true);
            CreateLabel(stack.transform, "Game Development Terms Explained", 16f, bodyFont, textSecondary, TextAlignmentOptions.Center, false);

            return canvasGroup;
        }

        private static void CreateLabel(Transform parent, string text, float size, TMP_FontAsset font, Color color, TextAlignmentOptions alignment, bool stretch)
        {
            var go = new GameObject(text, typeof(RectTransform), typeof(TextMeshProUGUI));
            go.transform.SetParent(parent, false);
            var rect = go.GetComponent<RectTransform>();
            if (stretch)
            {
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;
            }

            var label = go.GetComponent<TextMeshProUGUI>();
            label.text = text;
            label.fontSize = size;
            label.font = font;
            label.color = color;
            label.alignment = alignment;
        }

        private static IEnumerator FadeIn(CanvasGroup group, float duration)
        {
            var elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                group.alpha = Mathf.SmoothStep(0f, 1f, elapsed / duration);
                yield return null;
            }

            group.alpha = 1f;
        }

        private static IEnumerator FadeOut(CanvasGroup group, float duration)
        {
            var elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                group.alpha = Mathf.SmoothStep(1f, 0f, elapsed / duration);
                yield return null;
            }

            group.alpha = 0f;
        }
    }
}
