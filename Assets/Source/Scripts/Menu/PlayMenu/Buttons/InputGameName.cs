using Assets.Source.Scripts;
using Assets.Source.Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputGameName : MonoBehaviour
{
    public SceneLoader sceneLoader;

    void Update()
    { 
        if (gameObject.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            // LocalGame.cutsceneNumber = 1; // Know to play the first cutscene, cutscene is skipped with = 0
            // StartCoroutine(sceneLoader.LoadScene(2)); // Dailog Scene, Cutscenes will be played there by cutscene controller
            SaveStateManager.gameName = GetComponent<TMP_InputField>().text;
            LocalGame.levelNumber = 1; // Know to load the first level
            StartCoroutine(sceneLoader.LoadScene(3)); // Game Scene, intermediary solution until dialog and cutscenes are implemented
        }
    }
}
