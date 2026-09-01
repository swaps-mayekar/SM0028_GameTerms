using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameTerms.Bootstrap
{
    public sealed class SplashController : MonoBehaviour
    {
        [SerializeField] private string mainSceneName = "1_MainScene";
        [SerializeField] private float minimumDisplaySeconds = 1.2f;
        [SerializeField] private Color backgroundColor = new(0.07f, 0.08f, 0.11f);
        [SerializeField] private Color accentColor = new(0.36f, 0.78f, 1f);

        private IEnumerator Start()
        {
            BuildSplashUi();
            yield return new WaitForSeconds(minimumDisplaySeconds);
            SceneManager.LoadScene(mainSceneName);
        }

        private void BuildSplashUi()
        {
            var canvasGo = new GameObject("SplashCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(390f, 844f);

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
            stackRect.sizeDelta = new Vector2(320f, 180f);
            var layout = stack.GetComponent<VerticalLayoutGroup>();
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.spacing = 10f;

            CreateLabel(stack.transform, "GAME TERMS", 34f, FontStyles.Bold, Color.white);
            CreateLabel(stack.transform, "Game Development Terms Explained", 16f, FontStyles.Normal, new Color(0.68f, 0.73f, 0.8f));
            CreateLabel(stack.transform, "●", 24f, FontStyles.Bold, accentColor);
        }

        private static void CreateLabel(Transform parent, string text, float size, FontStyles style, Color color)
        {
            var go = new GameObject(text, typeof(RectTransform), typeof(TextMeshProUGUI));
            go.transform.SetParent(parent, false);
            var label = go.GetComponent<TextMeshProUGUI>();
            label.text = text;
            label.fontSize = size;
            label.fontStyle = style;
            label.color = color;
            label.alignment = TextAlignmentOptions.Center;
        }
    }
}
