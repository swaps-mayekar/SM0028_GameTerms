using UnityEngine;
using UnityEngine.UI;

namespace GameTerms.UI
{
    /// <summary>
    /// Generates a cached 9-slice rounded-rectangle sprite for tintable uGUI elements.
    /// </summary>
    public static class RoundedRectUtility
    {
        private const int Resolution = 64;
        private const int Border = 20;

        private static Sprite cachedSprite;

        public static Sprite Sprite
        {
            get
            {
                if (cachedSprite == null)
                {
                    cachedSprite = CreateSprite();
                }

                return cachedSprite;
            }
        }

        public static void Apply(Image image)
        {
            image.sprite = Sprite;
            image.type = Image.Type.Sliced;
        }

        private static Sprite CreateSprite()
        {
            var texture = new Texture2D(Resolution, Resolution, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp,
                hideFlags = HideFlags.HideAndDontSave
            };

            var half = Resolution / 2f - 0.5f;
            var radius = Border - 1f;

            for (var y = 0; y < Resolution; y++)
            {
                for (var x = 0; x < Resolution; x++)
                {
                    var px = x - half;
                    var py = y - half;
                    var distance = RoundedRectSdf(px, py, half, half, radius);
                    var alpha = Mathf.Clamp01(0.75f - distance);
                    texture.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
                }
            }

            texture.Apply();

            return Sprite.Create(
                texture,
                new Rect(0f, 0f, Resolution, Resolution),
                new Vector2(0.5f, 0.5f),
                100f,
                0,
                SpriteMeshType.FullRect,
                new Vector4(Border, Border, Border, Border));
        }

        private static float RoundedRectSdf(float x, float y, float halfWidth, float halfHeight, float radius)
        {
            var dx = Mathf.Abs(x) - halfWidth + radius;
            var dy = Mathf.Abs(y) - halfHeight + radius;
            var ax = Mathf.Max(dx, 0f);
            var ay = Mathf.Max(dy, 0f);
            return Mathf.Min(Mathf.Max(dx, dy), 0f) + Mathf.Sqrt(ax * ax + ay * ay) - radius;
        }
    }
}
