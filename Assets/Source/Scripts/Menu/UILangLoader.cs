using Assets.Source.Data;
using Data.Texts;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class UILangLoader : MonoBehaviour
{
    public static string langCode;
    public static NoErrorDictionary<string, string> currentLang;

    void Start()
    {
        langCode = DataIO.LangCode;
        ReloadLang();
    }

    public static void ReloadLang()
    {
        currentLang = new NoErrorDictionary<string, string>(TextLoader.LoadLanguage(langCode).UI);

        foreach (string key in currentLang.Keys)
        {
            GameObject go = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(x => key.Split("_")[1].ToLower() == x.name.ToLower());
            if (go is null or (default(GameObject)))
            {
                continue;
            }

            if (key.StartsWith("txt"))
            {
                go.GetComponentInChildren<TextMeshProUGUI>().text = currentLang[key];
            }
            else if (key.StartsWith("img"))
            {
                go.transform.GetComponentInChildren<Image>().sprite = Addressables.LoadAssetAsync<Sprite>(currentLang[key]).WaitForCompletion();
            }
        }
    }
    public static void ReloadLang(string langCode)
    {
        UILangLoader.langCode = langCode;
        ReloadLang();
    }
}

public class NoErrorDictionary<TKey, TValue> : Dictionary<TKey, TValue>
{
    public new TValue this[TKey key]
    {
        get => TryGetValue(key, out TValue val) ? val : default;
        set => base[key] = value;
    }
    public NoErrorDictionary(Dictionary<TKey, TValue> dict)
    {
        foreach (TKey key in dict.Keys)
        {
            this[key] = dict[key];
        }
    }
}