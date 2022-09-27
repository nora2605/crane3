using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSettings : MonoBehaviour
{
    public GameObject dialog;
    private Button buttonSettings;

    void Start()
    {
        buttonSettings = this.gameObject.GetComponent<Button>();
        buttonSettings.onClick.AddListener(buttonSettings_Click);
    }

    void buttonSettings_Click()
    {
        dialog.SetActive(true);
    }
}
