using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data.Texts;
using System.Linq;
using TMPro;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
using System;
using Assets.Source.Data;
using UnityEngine.AddressableAssets;

public class UILangLoader : MonoBehaviour
{
    public static string langCode;
    private static Dictionary<string, string> currentLang;

    void Start()
    {
        langCode = DataIO.LangCode;
        ReloadLang();
    }

    public static void ReloadLang()
    {
        currentLang = TextLoader.LoadLanguage(langCode).UI;

        foreach (string key in currentLang.Keys)
        {
            GameObject go = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(x => key.Split("_")[1].ToLower() == x.name.ToLower());
            if (go == null || go == default(GameObject)) continue;
            if (key.StartsWith("txt"))
                go.GetComponentInChildren<TextMeshProUGUI>().text = currentLang[key];
            else if (key.StartsWith("img"))
                go.transform.GetComponentInChildren<Image>().sprite = Addressables.LoadAssetAsync<Sprite>(currentLang[key]).WaitForCompletion();
        }
    }
    public static void ReloadLang(string langCode)
    {
        UILangLoader.langCode = langCode;
        ReloadLang();
    }
}
