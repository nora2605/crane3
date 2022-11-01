using Assets.Source.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonNewGame : MonoBehaviour
{
    private Button newGameButton;
    public SceneLoader sceneLoader;
    public GameObject inputField;

    void Start()
    {
        newGameButton = GetComponent<Button>();
        newGameButton.onClick.AddListener(NewGame);
    }

    void NewGame()
    {
        inputField.SetActive(true);
    }
}
