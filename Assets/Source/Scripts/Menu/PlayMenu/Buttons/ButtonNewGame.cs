using Assets.Source.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonNewGame : MonoBehaviour
{
    private Button newGameButton;
    public SceneLoader sceneLoader;
    void Start()
    {
        newGameButton = GetComponent<Button>();
        newGameButton.onClick.AddListener(NewGame);
    }

    void NewGame()
    {
        StartCoroutine(sceneLoader.LoadScene(2)); // Game First Cutscene
    }
}
