using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameTerms.UI
{
    /// <summary>
    /// Forwards drag and scroll events to the parent ScrollRect so lists remain scrollable
    /// when the pointer starts on a button or card, without blocking taps/clicks.
    /// </summary>
    public sealed class ScrollDragForwarder : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IScrollHandler
    {
        private const float DragThresholdPixels = 12f;

        private ScrollRect scrollRect;
        private bool forwarding;
        private Vector2 pointerDownPosition;

        private ScrollRect ParentScrollRect
        {
            get
            {
                if (scrollRect == null)
                {
                    scrollRect = GetComponentInParent<ScrollRect>();
                }

                return scrollRect;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            pointerDownPosition = eventData.position;
            forwarding = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            var scroll = ParentScrollRect;
            if (scroll == null)
            {
                return;
            }

            if (!forwarding)
            {
                if (Vector2.Distance(eventData.position, pointerDownPosition) < DragThresholdPixels)
                {
                    return;
                }

                forwarding = true;
                eventData.eligibleForClick = false;
                scroll.OnBeginDrag(eventData);
            }

            scroll.OnDrag(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (forwarding)
            {
                ParentScrollRect?.OnEndDrag(eventData);
            }

            forwarding = false;
        }

        public void OnScroll(PointerEventData eventData)
        {
            ParentScrollRect?.OnScroll(eventData);
        }
    }
}
