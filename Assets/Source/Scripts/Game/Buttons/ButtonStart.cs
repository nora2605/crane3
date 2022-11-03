using Assets.Source.Scripts.Game;
using Assets.Source.Scripts.Game.Buttons;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonStart : MonoBehaviour
{
    public TMP_InputField bytecodeEditor;
    public Slider speedSlider;
    public Coroutine interpreter;
    public Level3DController dscene;
    public Button saeButton;

    private bool resetOnly;
    public float MsSpeed => 1f / (float)speedSlider.value;

    private Button startButton;
    void Start()
    {
        startButton = GetComponent<Button>();
        startButton.onClick.AddListener(StartInterpreter);
    }

    // Update is called once per frame
    private void StartInterpreter()
    {
        if (resetOnly)
        {
            ResetScene();
            startButton.GetComponentInChildren<TMP_Text>().text = UILangLoader.currentLang["txt_buttonstart"];
            resetOnly = false;
            return;
        }
        if (GetExecutableText() != "")
        {
            interpreter = StartCoroutine(InterpreterCoroutine());
            startButton.enabled = false;
        }
    }

    public void ResetScene()
    {
        BytecodeInterpreter.win = false;
        Level.Reset();
        dscene.StopAllCoroutines();
        dscene.PReset();
    }

    private IEnumerator InterpreterCoroutine()
    {
        IEnumerator fix = BytecodeInterpreter.Execute(GetExecutableText(), Level.crane);
        dscene.UpdateScene();
        while (fix.MoveNext())
        {
            dscene.UpdateScene();
            yield return new WaitForSeconds(1f / (float)speedSlider.value);
        }
        startButton.enabled = true;
        if (!BytecodeInterpreter.win)
        {
            startButton.GetComponentInChildren<TMP_Text>().text = UILangLoader.currentLang["txt_buttonstartreset"];
            resetOnly = true;
        }
        else
        {
            startButton.interactable = false;
            startButton.GetComponentInChildren<TMP_Text>().text = UILangLoader.currentLang["txt_buttonstartwon"];
            saeButton.GetComponentInChildren<TMP_Text>().text = UILangLoader.currentLang["txt_buttonproceed"];
            saeButton.colors = new ColorBlock()
            {
                disabledColor = Color.white,
                normalColor = Color.green,
                pressedColor = Color.black,
                highlightedColor = Color.white,
                selectedColor = Color.white,
                colorMultiplier = 1.0f,
                fadeDuration = 0.3f
            };
        }
    }

    public string GetExecutableText()
    {
        return ChangeToBytecode.byteedit ? bytecodeEditor.text : LevelVisEditor.GetCode();
    }
}
