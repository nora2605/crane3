using Assets.Source.Data;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
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
            foreach (TextAsset f in Addressables.LoadAssetsAsync<TextAsset>("Texts", (_) => { }).WaitForCompletion())
            {
                langs.Add(new TMP_Dropdown.OptionData(Languages.ContainsKey(f.name) ? Languages[f.name] : f.name));
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
