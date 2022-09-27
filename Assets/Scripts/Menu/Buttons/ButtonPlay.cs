using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPlay : MonoBehaviour
{
    private Button buttonPlay;

    void Start()
    {
        buttonPlay = this.gameObject.GetComponent<Button>();
        buttonPlay.onClick.AddListener(buttonPlay_Click);
    }

    void buttonPlay_Click()
    {
        Camera.main.GetComponent<AudioSource>().volume = 0.0f;
        // Redirect to Save Stat Screen or show Overlay
    }
}
