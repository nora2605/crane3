using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Assets.Source.Scripts;

public class ButtonBackToMenu : MonoBehaviour
{
    private Button buttonBackToMenu;
    public SceneLoader transition;
    void Start()
    {
        buttonBackToMenu = this.gameObject.GetComponent<Button>();
        buttonBackToMenu.onClick.AddListener(buttonBackToMenu_Click);
    }

    void buttonBackToMenu_Click()
    {
        StartCoroutine(transition.LoadScene(0));
    }
}
