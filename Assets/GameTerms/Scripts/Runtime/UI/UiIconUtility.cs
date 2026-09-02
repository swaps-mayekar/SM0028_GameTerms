using System.Collections.Generic;
using UnityEngine;

namespace GameTerms.UI
{
    /// <summary>
    /// Generates cached tintable icon sprites so UI never depends on font glyph coverage.
    /// </summary>
    public static class UiIconUtility
    {
        private const int Resolution = 64;

        private static readonly Dictionary<UiIconId, Sprite> Cache = new();

        public static Sprite Get(UiIconId icon)
        {
            if (!Cache.TryGetValue(icon, out var sprite) || sprite == null)
            {
                sprite = CreateSprite(icon);
                Cache[icon] = sprite;
            }

            return sprite;
        }

        private static Sprite CreateSprite(UiIconId icon)
        {
            var texture = new Texture2D(Resolution, Resolution, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp,
                hideFlags = HideFlags.HideAndDontSave
            };

            var center = Resolution * 0.5f;
            var scale = Resolution * 0.46f;

            for (var y = 0; y < Resolution; y++)
            {
                for (var x = 0; x < Resolution; x++)
                {
                    var px = (x - center + 0.5f) / scale;
                    var py = (y - center + 0.5f) / scale;
                    var alpha = SampleIcon(icon, px, py);
                    texture.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
                }
            }

            texture.Apply();

            return Sprite.Create(
                texture,
                new Rect(0f, 0f, Resolution, Resolution),
                new Vector2(0.5f, 0.5f),
                100f);
        }

        private static float SampleIcon(UiIconId icon, float x, float y)
        {
            return icon switch
            {
                UiIconId.Home => HomeAlpha(x, y),
                UiIconId.Categories => CategoriesAlpha(x, y),
                UiIconId.StarFilled => StarAlpha(x, y, filled: true),
                UiIconId.StarOutline => StarAlpha(x, y, filled: false),
                UiIconId.Search => SearchAlpha(x, y),
                UiIconId.Back => BackAlpha(x, y),
                UiIconId.Dot => SoftCircle(x, y, 0.22f),
                _ => 0f
            };
        }

        private static float HomeAlpha(float x, float y)
        {
            var body = Box(x, y, 0.42f, 0.28f, new Vector2(0f, -0.18f));
            var roof = Triangle(x, y, new Vector2(0f, 0.34f), new Vector2(-0.46f, -0.02f), new Vector2(0.46f, -0.02f));
            return Mathf.Max(body, roof);
        }

        private static float CategoriesAlpha(float x, float y)
        {
            var cell = 0.3f;
            var gap = 0.1f;
            var offset = cell + gap * 0.5f;
            return Mathf.Max(
                Mathf.Max(RoundedBox(x, y, cell, cell, new Vector2(-offset, offset), 0.08f), RoundedBox(x, y, cell, cell, new Vector2(offset, offset), 0.08f)),
                Mathf.Max(RoundedBox(x, y, cell, cell, new Vector2(-offset, -offset), 0.08f), RoundedBox(x, y, cell, cell, new Vector2(offset, -offset), 0.08f)));
        }

        private static float StarAlpha(float x, float y, bool filled)
        {
            var angle = Mathf.Atan2(y, x);
            var distance = Mathf.Sqrt(x * x + y * y);
            var spikes = 5f;
            var outer = 0.95f;
            var inner = 0.38f;
            var sector = Mathf.Repeat(angle + Mathf.PI * 0.5f, Mathf.PI * 2f / spikes);
            var halfSector = Mathf.PI / spikes;
            var t = Mathf.Abs(sector - halfSector) / halfSector;
            var radius = Mathf.Lerp(outer, inner, t);
            var edge = filled ? radius - distance : Mathf.Abs(distance - radius) - 0.07f;
            return SoftEdge(edge * 18f);
        }

        private static float SearchAlpha(float x, float y)
        {
            var lens = SoftCircle(x - 0.12f, y - 0.08f, 0.34f) - SoftCircle(x - 0.12f, y - 0.08f, 0.22f);
            var handle = Capsule(x, y, new Vector2(0.18f, -0.18f), new Vector2(0.48f, -0.48f), 0.08f);
            return Mathf.Max(lens, handle);
        }

        private static float BackAlpha(float x, float y)
        {
            var shaft = Capsule(x, y, new Vector2(0.18f, 0f), new Vector2(-0.28f, 0f), 0.08f);
            var top = Capsule(x, y, new Vector2(-0.05f, 0.22f), new Vector2(-0.34f, 0f), 0.08f);
            var bottom = Capsule(x, y, new Vector2(-0.05f, -0.22f), new Vector2(-0.34f, 0f), 0.08f);
            return Mathf.Max(shaft, Mathf.Max(top, bottom));
        }

        private static float Box(float x, float y, float halfWidth, float halfHeight, Vector2 center)
        {
            var dx = Mathf.Abs(x - center.x) - halfWidth;
            var dy = Mathf.Abs(y - center.y) - halfHeight;
            var outside = Mathf.Max(dx, dy);
            var inside = Mathf.Min(Mathf.Max(dx, dy), 0f);
            return SoftEdge(-(outside + inside) * 20f);
        }

        private static float RoundedBox(float x, float y, float halfWidth, float halfHeight, Vector2 center, float radius)
        {
            var px = x - center.x;
            var py = y - center.y;
            var dx = Mathf.Abs(px) - halfWidth + radius;
            var dy = Mathf.Abs(py) - halfHeight + radius;
            var ax = Mathf.Max(dx, 0f);
            var ay = Mathf.Max(dy, 0f);
            var distance = Mathf.Min(Mathf.Max(dx, dy), 0f) + Mathf.Sqrt(ax * ax + ay * ay) - radius;
            return SoftEdge(-distance * 20f);
        }

        private static float Triangle(float x, float y, Vector2 a, Vector2 b, Vector2 c)
        {
            var w1 = Sign(x, y, a, b);
            var w2 = Sign(x, y, b, c);
            var w3 = Sign(x, y, c, a);
            var inside = w1 < 0f && w2 < 0f && w3 < 0f;
            if (inside)
            {
                return 1f;
            }

            var edge = Mathf.Max(
                Mathf.Max(EdgeDistance(x, y, a, b), EdgeDistance(x, y, b, c)),
                EdgeDistance(x, y, c, a));
            return SoftEdge(-edge * 22f);
        }

        private static float SoftCircle(float x, float y, float radius)
        {
            var distance = Mathf.Sqrt(x * x + y * y);
            return SoftEdge((radius - distance) * 20f);
        }

        private static float Capsule(float x, float y, Vector2 a, Vector2 b, float radius)
        {
            var pa = new Vector2(x, y) - a;
            var ba = b - a;
            var h = Mathf.Clamp01(Vector2.Dot(pa, ba) / Vector2.Dot(ba, ba));
            var distance = (pa - ba * h).magnitude - radius;
            return SoftEdge(-distance * 20f);
        }

        private static float EdgeDistance(float x, float y, Vector2 a, Vector2 b)
        {
            var p = new Vector2(x, y);
            var ab = b - a;
            var t = Mathf.Clamp01(Vector2.Dot(p - a, ab) / Vector2.Dot(ab, ab));
            return Vector2.Distance(p, a + ab * t);
        }

        private static float Sign(float x, float y, Vector2 a, Vector2 b)
        {
            return (x - b.x) * (a.y - b.y) - (a.x - b.x) * (y - b.y);
        }

        private static float SoftEdge(float value)
        {
            return Mathf.Clamp01(value);
        }
    }
}
