using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Assets.Source.Scripts.Menu.PlayMenu;

public class InitializeSaveStatList : MonoBehaviour
{
    public GameObject SaveStatListItem;
    public Transform ContentRect;
    public GameObject NoSavesLabel;

    // public static List<GameObject> saveStats = new List<GameObject>();

    void Start()
    {
        Camera.main.GetComponent<AudioSource>().time = Indexer.entry + Time.deltaTime; // It's a small scene it shouldn't be too noticeable
        // will work on an in-scene solution tho
        int listEntries = 0;
        string savePath = Application.persistentDataPath + "/saves";
        if (!System.IO.Directory.Exists(savePath))
        {
            System.IO.Directory.CreateDirectory(savePath);
            goto NoSaves;
        }
        string[] f = System.IO.Directory.EnumerateFiles(savePath).ToArray();
        foreach (string s in f)
        {
            GameObject li = Instantiate(SaveStatListItem, ContentRect);
            li.name = s.Split('\\', '/')[^1];
            li.SetActive(true);
            foreach (MonoBehaviour tmp in li.GetComponentsInChildren<MonoBehaviour>().TakeWhile(x => x.GetComponent<TextMeshProUGUI>() != null))
            {
                if (tmp.name == "SaveStatName") tmp.GetComponent<TextMeshProUGUI>().text = li.name;
                else if (tmp.name == "SaveStatLastModified") tmp.GetComponent<TextMeshProUGUI>().text = File.GetLastAccessTime(s).ToString("yyyy-MM-dd HH:mm:ss");
            }
            ContentRect.GetComponent<RectTransform>().sizeDelta += new Vector2(0f, 180f);
            li.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, - 180 * listEntries - 90, 0);
            listEntries++;
            li.GetComponentInChildren<SaveStatSelector>().saveName = s;
            // saveStats.Add(li);
        }
    NoSaves:
        if (listEntries == 0)
        {
            NoSavesLabel.SetActive(true);
        }
        UILangLoader.ReloadLang();
    }

    void Update()
    {
        
    }
}
