using Assets.Source.Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonStart : MonoBehaviour
{
    public TMP_InputField bytecodeEditor;
    public Slider speedSlider;
    public Coroutine interpreter;

    private Button startButton;
    void Start()
    {
        startButton = GetComponent<Button>();
        startButton.onClick.AddListener(StartInterpreter);
    }

    // Update is called once per frame
    void StartInterpreter()
    {
        ResetScene();
        interpreter = StartCoroutine(InterpreterCoroutine());
    }

    public void ResetScene()
    {
        BytecodeInterpreter.win = false;
    }

    IEnumerator InterpreterCoroutine()
    {
        IEnumerator fix = BytecodeInterpreter.Execute(bytecodeEditor.text, Level.crane);
        while (fix.MoveNext())
        {
            yield return new WaitForSeconds(1f / (float)speedSlider.value);
        }
    }
}
