using Assets.Source.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Settings
{
    public class Language : MonoBehaviour
    {
        public Button apply;
        private TMP_Dropdown dropdown;

        public static Dictionary<string, string> Languages = new() // Dictionary of language codes to native Language string
        {
            { "de", "Deutsch" },
            { "en", "English" },
            { "jp", "日本語" },
            { "fi", "Suomi" },
            { "in", "हिन्दी" }
        };

        public static Dictionary<string, string> LanguageCodes;

        public void Start()
        {
            LanguageCodes = Languages.ToDictionary(x => x.Value, x => x.Key);

            apply.onClick.AddListener(Apply);

            dropdown = gameObject.GetComponent<TMP_Dropdown>();

            dropdown.ClearOptions();
            List<TMP_Dropdown.OptionData> langs = new();
            foreach (TextAsset f in Resources.LoadAll<TextAsset>("Texts/"))
            {
                langs.Add(new TMP_Dropdown.OptionData(Languages.ContainsKey(f.name) ? Languages[f.name] : f.name));
                Resources.UnloadAsset(f);
            }
            gameObject.GetComponent<TMP_Dropdown>().AddOptions(langs);

            dropdown.value = dropdown.options.IndexOf(
                dropdown.options.First(x => x.text == DataIO.LangCode || x.text == Languages[DataIO.LangCode])
            );
        }

        public void Apply()
        {
            string lang = dropdown.options[dropdown.value].text;
            string langcode = LanguageCodes.ContainsKey(lang) ? LanguageCodes[lang] : lang;
            DataIO.LangCode = langcode;
            UILangLoader.ReloadLang(DataIO.LangCode);
        }
    }
}
