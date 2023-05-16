using Assets.Source.Scripts;
using UnityEngine;
using UnityEngine.UI;

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
