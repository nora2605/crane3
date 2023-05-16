using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Source.Scripts.Game
{
    public class CodeBlockTimelineController : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler, IScrollHandler
    {
        public float m_InsertDistance = 100;
        private Vector3 m_Offset;
        private bool m_IsDragging;
        public CodeBlock parent;

        public void OnPointerDown(PointerEventData e)
        {
            if (e.button == PointerEventData.InputButton.Right)
            {
                LevelVisEditor.instance.blocks.Remove(parent);
                Destroy(gameObject);
            }
        }
        public void OnScroll(PointerEventData e)
        {
            if (parent.codeType == Codes.Number)
            {
                parent.number = e.scrollDelta.y > 0 ? (parent.number + 1 > 9 ? 9 : parent.number + 1) : (parent.number - 1 < 1 ? 1 : parent.number - 1);
                parent.InitializeCodeType();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!m_IsDragging)
            {
                LevelVisEditor.instance.blocks.Remove(parent);
                m_IsDragging = true;
                m_Offset = eventData.pointerCurrentRaycast.worldPosition;
            }

            parent.gObject.rectTransform.position = Input.mousePosition - m_Offset;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!m_IsDragging)
            {
                return;
            }

            if (parent.gObject.rectTransform.anchoredPosition.x <= -20)
            {
                m_IsDragging = false;
                Destroy(parent.gObject.gameObject);
                return;
            }

            float closestDistance = float.MaxValue;
            int closestIndex = -1;
            for (int i = 0; i < LevelVisEditor.instance.blocks.Count; i++)
            {
                float distance = Vector3.Distance(parent.gObject.rectTransform.position, LevelVisEditor.instance.blocks[i].gObject.rectTransform.position - new Vector3(60, 0, 0));
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = i;
                }
            }

            if (closestDistance <= m_InsertDistance)
            {
                LevelVisEditor.instance.blocks.Insert(closestIndex, parent);
            }
            else
            {
                LevelVisEditor.instance.blocks.Add(parent);
            }

            m_IsDragging = false;
        }
    }

}
