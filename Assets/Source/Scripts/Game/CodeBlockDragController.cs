using Assets.Source.Scripts.Game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CodeBlockDragController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public LevelVisEditor m_LevelVisEditor;
    public CodeBlock m_CodeBlock;
    public CodeBlock m_Block;

    public float m_InsertDistance = 100;

    private Image m_DraggedImage;
    private Vector3 m_Offset;
    private bool m_IsDragging;

    private void Start()
    {
        m_LevelVisEditor = LevelVisEditor.instance;
        m_DraggedImage = m_LevelVisEditor.blockPrototype;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!m_IsDragging)
        {
            m_IsDragging = true;
            m_CodeBlock = new CodeBlock(Instantiate(m_DraggedImage))
            {
                codeType = m_Block.codeType
            };
            if (m_CodeBlock.codeType == Codes.Number)
            {
                m_CodeBlock.number = m_Block.number;
            }

            m_CodeBlock.gObject.rectTransform.SetParent(m_LevelVisEditor.gameObject.transform, false);
            m_CodeBlock.gObject.gameObject.SetActive(true);
            m_CodeBlock.gObject.name = "timeline codeblock";
            m_CodeBlock.InitializeCodeType();
            m_CodeBlock.gObject.rectTransform.anchorMin = new Vector2(0, 0);
            m_CodeBlock.gObject.rectTransform.anchorMax = new Vector2(0, 0);
            m_Offset = eventData.pointerCurrentRaycast.worldPosition;
        }

        m_CodeBlock.gObject.rectTransform.position = Input.mousePosition - m_Offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!m_IsDragging)
        {
            return;
        }
        m_CodeBlock.gObject.rectTransform.SetParent(m_LevelVisEditor.scrollTimeline.transform, true);
        if (m_CodeBlock.gObject.rectTransform.anchoredPosition.x <= -60)
        {
            m_IsDragging = false;
            Destroy(m_CodeBlock.gObject.gameObject);
            return;
        }

        float closestDistance = float.MaxValue;
        int closestIndex = -1;
        for (int i = 0; i < m_LevelVisEditor.blocks.Count; i++)
        {
            float distance = Vector3.Distance(m_CodeBlock.gObject.rectTransform.anchoredPosition, m_LevelVisEditor.blocks[i].gObject.rectTransform.anchoredPosition - new Vector2(60, 0));
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        if (closestDistance <= m_InsertDistance)
        {
            m_LevelVisEditor.blocks.Insert(closestIndex, m_CodeBlock);
        }
        else
        {
            m_LevelVisEditor.blocks.Add(m_CodeBlock);
        }

        CodeBlockTimelineController cbtc = m_CodeBlock.gObject.gameObject.AddComponent<CodeBlockTimelineController>();
        cbtc.parent = m_CodeBlock;

        m_IsDragging = false;
    }
}