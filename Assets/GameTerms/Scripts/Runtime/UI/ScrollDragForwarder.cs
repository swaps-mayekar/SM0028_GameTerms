using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameTerms.UI
{
    /// <summary>
    /// Forwards drag and scroll events to the parent ScrollRect so lists remain scrollable
    /// when the pointer starts on a button or card.
    /// </summary>
    public sealed class ScrollDragForwarder : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler
    {
        private ScrollRect scrollRect;

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

        public void OnBeginDrag(PointerEventData eventData)
        {
            ParentScrollRect?.OnBeginDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            ParentScrollRect?.OnDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ParentScrollRect?.OnEndDrag(eventData);
        }

        public void OnScroll(PointerEventData eventData)
        {
            ParentScrollRect?.OnScroll(eventData);
        }
    }
}
