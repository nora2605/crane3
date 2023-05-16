using Assets.Source.Scripts;
using Assets.Source.Scripts.Game;
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
        if (BytecodeInterpreter.win)
        {
            LocalGame.dialogNumber = LocalGame.levelNumber++;
            LocalGame.cutsceneNumber = LocalGame.dialogNumber % 10 == 0 ? LocalGame.levelNumber / 10 : -1;
            SaveStateManager.Save();
            StartCoroutine(sceneLoader.LoadScene(2));
            return;
        }
        SaveStateManager.Save();
        StartCoroutine(sceneLoader.LoadScene(0));
    }
}
