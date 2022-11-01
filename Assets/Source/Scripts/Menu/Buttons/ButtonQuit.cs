using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonQuit : MonoBehaviour
{
    private Button buttonQuit;

    void Start()
    {
        buttonQuit = this.gameObject.GetComponent<Button>();
        buttonQuit.onClick.AddListener(buttonQuit_Click);
    }

    void buttonQuit_Click()
    {
        Application.Quit();
		#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
		#endif
    }
}
