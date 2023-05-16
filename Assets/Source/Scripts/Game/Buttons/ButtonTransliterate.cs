using Assets.Source.Scripts.Game;
using Assets.Source.Scripts.Game.Buttons;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class ButtonTransliterate : MonoBehaviour
{
    public GameObject[] VisEdit;
    public GameObject[] ByteEdit;
    public TMP_InputField BCText;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(Transliterate);
    }

    private void Transliterate()
    {
        BCText.text = LevelVisEditor.GetCode();
        ChangeToBytecode.byteedit = !ChangeToBytecode.byteedit;
        foreach (GameObject go in VisEdit)
        {
            go.SetActive(!ChangeToBytecode.byteedit);
        }
        foreach (GameObject go in ByteEdit)
        {
            go.SetActive(ChangeToBytecode.byteedit);
        }
    }
}
