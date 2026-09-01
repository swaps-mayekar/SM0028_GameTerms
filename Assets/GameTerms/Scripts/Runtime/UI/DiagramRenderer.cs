using UnityEngine;
using UnityEngine.UI;

namespace GameTerms.UI
{
    public static class DiagramRenderer
    {
        public static RectTransform Create(Transform parent, DiagramType diagramType, Color accent, Color surface)
        {
            var root = new GameObject("Diagram", typeof(RectTransform)).GetComponent<RectTransform>();
            root.SetParent(parent, false);
            var layout = root.gameObject.AddComponent<LayoutElement>();
            layout.minHeight = 140f;
            layout.preferredHeight = 160f;

            var background = CreateBox(root, surface, "DiagramBackground");
            UiFactory.Stretch(background);

            switch (diagramType)
            {
                case DiagramType.OcclusionCulling:
                    BuildOcclusion(background, accent);
                    break;
                case DiagramType.ObjectPooling:
                    BuildPooling(background, accent);
                    break;
                case DiagramType.FrustumCulling:
                    BuildFrustum(background, accent);
                    break;
                case DiagramType.LevelOfDetail:
                    BuildLod(background, accent);
                    break;
                case DiagramType.StateMachine:
                    BuildStateMachine(background, accent, surface);
                    break;
            }

            return root;
        }

        private static RectTransform CreateBox(Transform parent, Color color, string name)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            go.GetComponent<Image>().color = color;
            return go.GetComponent<RectTransform>();
        }

        private static void BuildOcclusion(RectTransform parent, Color accent)
        {
            var wall = CreateBox(parent, accent * 0.8f, "Wall");
            wall.anchorMin = new Vector2(0.45f, 0.1f);
            wall.anchorMax = new Vector2(0.55f, 0.9f);
            wall.offsetMin = Vector2.zero;
            wall.offsetMax = Vector2.zero;

            var visible = CreateBox(parent, accent, "Visible");
            visible.anchorMin = new Vector2(0.08f, 0.25f);
            visible.anchorMax = new Vector2(0.35f, 0.75f);
            visible.offsetMin = Vector2.zero;
            visible.offsetMax = Vector2.zero;

            var hidden = CreateBox(parent, new Color(accent.r, accent.g, accent.b, 0.2f), "Hidden");
            hidden.anchorMin = new Vector2(0.62f, 0.25f);
            hidden.anchorMax = new Vector2(0.9f, 0.75f);
            hidden.offsetMin = Vector2.zero;
            hidden.offsetMax = Vector2.zero;
        }

        private static void BuildPooling(RectTransform parent, Color accent)
        {
            for (var i = 0; i < 4; i++)
            {
                var slot = CreateBox(parent, new Color(accent.r, accent.g, accent.b, 0.25f + i * 0.15f), $"Slot{i}");
                slot.anchorMin = new Vector2(0.1f + i * 0.2f, 0.35f);
                slot.anchorMax = new Vector2(0.22f + i * 0.2f, 0.65f);
                slot.offsetMin = Vector2.zero;
                slot.offsetMax = Vector2.zero;
            }

            var active = CreateBox(parent, accent, "Active");
            active.anchorMin = new Vector2(0.72f, 0.2f);
            active.anchorMax = new Vector2(0.9f, 0.8f);
            active.offsetMin = Vector2.zero;
            active.offsetMax = Vector2.zero;
        }

        private static void BuildFrustum(RectTransform parent, Color accent)
        {
            var camera = CreateBox(parent, accent, "Camera");
            camera.anchorMin = new Vector2(0.05f, 0.42f);
            camera.anchorMax = new Vector2(0.12f, 0.58f);
            camera.offsetMin = Vector2.zero;
            camera.offsetMax = Vector2.zero;

            var inView = CreateBox(parent, accent, "InView");
            inView.anchorMin = new Vector2(0.2f, 0.25f);
            inView.anchorMax = new Vector2(0.62f, 0.75f);
            inView.offsetMin = Vector2.zero;
            inView.offsetMax = Vector2.zero;

            var outView = CreateBox(parent, new Color(accent.r, accent.g, accent.b, 0.2f), "OutView");
            outView.anchorMin = new Vector2(0.72f, 0.3f);
            outView.anchorMax = new Vector2(0.92f, 0.7f);
            outView.offsetMin = Vector2.zero;
            outView.offsetMax = Vector2.zero;
        }

        private static void BuildLod(RectTransform parent, Color accent)
        {
            var near = CreateBox(parent, accent, "High");
            near.anchorMin = new Vector2(0.08f, 0.2f);
            near.anchorMax = new Vector2(0.28f, 0.8f);
            near.offsetMin = Vector2.zero;
            near.offsetMax = Vector2.zero;

            var mid = CreateBox(parent, accent * 0.75f, "Mid");
            mid.anchorMin = new Vector2(0.38f, 0.28f);
            mid.anchorMax = new Vector2(0.56f, 0.72f);
            mid.offsetMin = Vector2.zero;
            mid.offsetMax = Vector2.zero;

            var far = CreateBox(parent, accent * 0.45f, "Low");
            far.anchorMin = new Vector2(0.68f, 0.34f);
            far.anchorMax = new Vector2(0.84f, 0.66f);
            far.offsetMin = Vector2.zero;
            far.offsetMax = Vector2.zero;
        }

        private static void BuildStateMachine(RectTransform parent, Color accent, Color surface)
        {
            var idle = CreateBox(parent, accent, "Idle");
            idle.anchorMin = new Vector2(0.1f, 0.55f);
            idle.anchorMax = new Vector2(0.3f, 0.8f);
            idle.offsetMin = Vector2.zero;
            idle.offsetMax = Vector2.zero;

            var run = CreateBox(parent, accent * 0.8f, "Run");
            run.anchorMin = new Vector2(0.4f, 0.55f);
            run.anchorMax = new Vector2(0.6f, 0.8f);
            run.offsetMin = Vector2.zero;
            run.offsetMax = Vector2.zero;

            var jump = CreateBox(parent, surface, "Jump");
            jump.anchorMin = new Vector2(0.7f, 0.55f);
            jump.anchorMax = new Vector2(0.9f, 0.8f);
            jump.offsetMin = Vector2.zero;
            jump.offsetMax = Vector2.zero;
        }
    }
}
