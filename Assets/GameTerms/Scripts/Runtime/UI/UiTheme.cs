using TMPro;
using UnityEngine;

namespace GameTerms.UI
{
    [CreateAssetMenu(fileName = "UiTheme", menuName = "Game Terms/UI Theme")]
    public sealed class UiTheme : ScriptableObject
    {
        [Header("Colors")]
        public Color Background = new(0.07f, 0.08f, 0.11f);
        public Color Surface = new(0.11f, 0.13f, 0.17f);
        public Color SurfaceElevated = new(0.15f, 0.17f, 0.22f);
        public Color Border = new(0.24f, 0.28f, 0.36f, 0.65f);
        public Color Primary = new(0.36f, 0.78f, 1f);
        public Color PrimaryMuted = new(0.36f, 0.78f, 1f, 0.18f);
        public Color TextPrimary = new(0.94f, 0.96f, 0.98f);
        public Color TextSecondary = new(0.68f, 0.73f, 0.8f);
        public Color TextMuted = new(0.5f, 0.56f, 0.64f);
        public Color Accent = new(0.55f, 0.72f, 1f);
        public Color Favorite = new(0.96f, 0.42f, 0.52f);
        public Color SearchHighlight = new(0.36f, 0.78f, 1f);
        public Color Shadow = new(0f, 0f, 0f, 0.25f);
        public Color NavActive = new(0.36f, 0.78f, 1f);
        public Color NavInactive = new(0.5f, 0.56f, 0.64f);

        [Header("Typography")]
        public TMP_FontAsset SansRegular;
        public TMP_FontAsset SansSemiBold;
        public TMP_FontAsset SansBold;
        public TMP_FontAsset MonoRegular;

        [Header("Spacing")]
        public float PaddingSmall = 8f;
        public float PaddingMedium = 16f;
        public float PaddingLarge = 24f;
        public float CornerRadius = 14f;
        public float MinTouchTarget = 44f;

        [Header("Type Scale")]
        public float TitleSize = 30f;
        public float SectionSize = 18f;
        public float TermSize = 20f;
        public float BodySize = 16f;
        public float MetaSize = 13f;
        public float ButtonSize = 15f;
        public float NavIconSize = 18f;
        public float NavLabelSize = 10f;
        public float NavHeight = 54f;

        public float GetResponsiveSize(float baseSize, float screenWidth)
        {
            var scale = screenWidth >= 768f ? 1.12f : 1f;
            return baseSize * scale;
        }

        public float GetContentMaxWidth(float screenWidth)
        {
            return screenWidth >= 768f ? 720f : screenWidth;
        }

        public string GetSearchHighlightHex()
        {
            return $"#{ColorUtility.ToHtmlStringRGB(SearchHighlight)}";
        }
    }
}
