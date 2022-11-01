using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class ButtonSettings : MonoBehaviour
{
    public GameObject dialog;
    private Button buttonSettings;
    private float lerpDuration = 0.3f;
    private CanvasGroup canvasGroup;

    void Start()
    {
        buttonSettings = this.gameObject.GetComponent<Button>();
        buttonSettings.onClick.AddListener(buttonSettings_Click);
        canvasGroup = dialog.GetComponent<CanvasGroup>();
    }

    void buttonSettings_Click()
    {
        StartCoroutine(FadeDialog());
    }

    IEnumerator FadeDialog()
    {
        float timeElapsed = 0.0f;
        canvasGroup.alpha = 0f;
        dialog.SetActive(true);
        while (timeElapsed < lerpDuration)
        {
            canvasGroup.alpha = timeElapsed / lerpDuration;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }
}
