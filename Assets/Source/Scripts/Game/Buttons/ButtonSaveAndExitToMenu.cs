using Assets.Source.Scripts;
using Assets.Source.Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSaveAndExitToMenu : MonoBehaviour
{
    private Button buttonSaveAndExitToMenu;
    public SceneLoader sceneLoader;
    void Start()
    {
        buttonSaveAndExitToMenu = GetComponent<Button>();
        buttonSaveAndExitToMenu.onClick.AddListener(SaveAndExit);
    }

    void SaveAndExit()
    {
        SaveStateManager.Save();
        StartCoroutine(sceneLoader.LoadScene(0));
    }
}
