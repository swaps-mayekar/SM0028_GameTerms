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
                UiIconId.HeartFilled => HeartAlpha(x, y, filled: true),
                UiIconId.HeartOutline => HeartAlpha(x, y, filled: false),
                UiIconId.Search => SearchAlpha(x, y),
                UiIconId.Back => BackAlpha(x, y),
                UiIconId.Dot => SoftCircle(x, y, 0.22f),
                _ => 0f
            };
        }

        private static float HomeAlpha(float x, float y)
        {
            const float stroke = 0.078f;
            var roofLeft = Capsule(x, y, new Vector2(0f, 0.48f), new Vector2(-0.56f, -0.02f), stroke);
            var roofRight = Capsule(x, y, new Vector2(0f, 0.48f), new Vector2(0.56f, -0.02f), stroke);
            var eaves = Capsule(x, y, new Vector2(-0.56f, -0.02f), new Vector2(0.56f, -0.02f), stroke);
            var leftWall = Capsule(x, y, new Vector2(-0.4f, -0.02f), new Vector2(-0.4f, -0.58f), stroke);
            var rightWall = Capsule(x, y, new Vector2(0.4f, -0.02f), new Vector2(0.4f, -0.58f), stroke);
            var floor = Capsule(x, y, new Vector2(-0.4f, -0.58f), new Vector2(0.4f, -0.58f), stroke);
            var doorLeft = Capsule(x, y, new Vector2(-0.14f, -0.58f), new Vector2(-0.14f, -0.22f), stroke * 0.85f);
            var doorRight = Capsule(x, y, new Vector2(0.14f, -0.58f), new Vector2(0.14f, -0.22f), stroke * 0.85f);
            var doorTop = Capsule(x, y, new Vector2(-0.14f, -0.22f), new Vector2(0.14f, -0.22f), stroke * 0.85f);

            return Mathf.Max(
                Mathf.Max(Mathf.Max(roofLeft, roofRight), Mathf.Max(eaves, Mathf.Max(leftWall, rightWall))),
                Mathf.Max(floor, Mathf.Max(doorLeft, Mathf.Max(doorRight, doorTop))));
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

        private static float HeartAlpha(float x, float y, bool filled)
        {
            var distance = HeartDistance(x * 0.92f, y * 0.92f + 0.52f);
            if (filled)
            {
                return SoftEdge(-distance * 22f);
            }

            return SoftEdge((0.09f - Mathf.Abs(distance)) * 22f);
        }

        /// <summary>
        /// Inigo Quilez 2D heart SDF, expecting the heart tip near the origin and lobes near y = 1.
        /// </summary>
        private static float HeartDistance(float x, float y)
        {
            x = Mathf.Abs(x);
            if (y + x > 1f)
            {
                var q = new Vector2(x - 0.25f, y - 0.75f);
                return q.magnitude - Mathf.Sqrt(2f) * 0.25f;
            }

            var toTop = new Vector2(x, y - 1f);
            var m = Mathf.Max(x + y, 0f) * 0.5f;
            var toDiag = new Vector2(x - m, y - m);
            var minSq = Mathf.Min(Vector2.Dot(toTop, toTop), Vector2.Dot(toDiag, toDiag));
            return Mathf.Sqrt(minSq) * Mathf.Sign(x - y);
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

        private static float SoftEdge(float value)
        {
            return Mathf.Clamp01(value);
        }
    }
}
