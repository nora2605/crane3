using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data.Texts;
using System.Linq;
using TMPro;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
using System;

public class UILangLoader : MonoBehaviour
{
    public static string langCode = "de"; // Changed later to load from persistentDataPath instead
    private static Dictionary<string, string> currentLang;

    void Start()
    {
        ReloadLang();
    }

    public static void ReloadLang()
    {


        currentLang = TextLoader.LoadLanguage(langCode).UI;

        foreach (string key in currentLang.Keys)
        {
            GameObject go = Resources.FindObjectsOfTypeAll<GameObject>().First(x => key.Contains(x.name.ToLower()));
            if (go == null) return;
            if (key.StartsWith("txt"))
                go.GetComponentInChildren<TextMeshProUGUI>().text = currentLang[key];
            else if (key.StartsWith("img"))
                go.transform.GetComponentInChildren<Image>().sprite = Resources.Load(currentLang[key]) as Sprite;
        }
    }
    public static void ReloadLang(string langCode)
    {
        UILangLoader.langCode = langCode;
        ReloadLang();
    }

    void Update()
    {
        
    }
}
