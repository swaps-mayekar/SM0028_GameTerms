using TMPro;
using UnityEngine;

namespace GameTerms.UI
{
    public static class FontAssetUtility
    {
        public static TMP_FontAsset GetUsableFont(TMP_FontAsset font, TMP_FontAsset fallback)
        {
            return IsReady(font) ? font : fallback;
        }

        public static bool IsReady(TMP_FontAsset font)
        {
            if (font == null)
            {
                return false;
            }

            if (font.atlasTexture == null)
            {
                return false;
            }

            return font.characterTable != null && font.characterTable.Count > 0;
        }
    }
}
