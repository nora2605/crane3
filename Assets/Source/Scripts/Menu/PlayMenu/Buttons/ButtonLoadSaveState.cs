using Assets.Source.Scripts;
using Assets.Source.Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLoadSaveState : MonoBehaviour
{
    private Button loadSaveStateButton;
    public SceneLoader sceneLoader;

    void Start()
    {
        loadSaveStateButton = GetComponent<Button>();
        loadSaveStateButton.onClick.AddListener(Load);
    }

    private void Update()
    {
        loadSaveStateButton.interactable = Indexer.selected;
    }

    void Load()
    {
        SaveStateManager.gameName = Indexer.currentSelected.Split('\\', '/')[^1];
        SaveStateManager.Load();
        StartCoroutine(sceneLoader.LoadScene(3));
    }
}
