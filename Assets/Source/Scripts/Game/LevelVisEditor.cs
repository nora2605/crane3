using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.Scripts.Game
{
    public class LevelVisEditor : MonoBehaviour
    {
        public Image blockPrototype;
        public Transform scrollContent;
        public Transform scrollTimeline;
        public RectTransform tlContent;

        public static LevelVisEditor instance;

        public List<CodeBlock> templateBlocks = new();

        public List<CodeBlock> blocks = new();

        private void Start()
        {
            instance = this;

            Codes[] keys = (Codes[])Enum.GetValues(typeof(Codes));
            for (int i = 0; i < keys.Length; i++)
            {
                Codes key = keys[i];
                CodeBlock cb = new CodeBlock(Instantiate(blockPrototype));
                cb.codeType = key;
                if (key == Codes.Number) cb.number = 1;
                cb.InitializeCodeType();
                cb.gObject.name = $"TemplateBlock_{key}";
                cb.gObject.gameObject.SetActive(true);
                cb.gObject.rectTransform.localScale = new Vector3(0.5f, 0.5f, 1f);
                cb.gObject.rectTransform.SetParent(scrollContent);
                cb.gObject.rectTransform.anchoredPosition = new Vector2(-80 + 55 * (i / 6), -35 - 60 * (i % 6));
                scrollContent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 15);
                var a = cb.gObject.gameObject.AddComponent<CodeBlockDragController>();
                a.m_Block = cb;
            }
        }

        private void Update()
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].gObject.rectTransform.anchoredPosition = new Vector2(i * 120, 0);
            }
        }

        public static string GetCode()
        {
            return instance.GetBytecode();
        }

        public string GetBytecode()
        {
            return blocks.Count > 0 ? blocks.Select(x => x.ToRepresentativeCode()).Aggregate((x, y) => x + y) : "";
        }
    }
}
