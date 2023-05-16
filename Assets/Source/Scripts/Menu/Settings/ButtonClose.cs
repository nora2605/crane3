using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClose : MonoBehaviour
{
    private Button buttonClose;
    public GameObject settingsDialog;

    private float lerpDuration = 0.3f;
    private CanvasGroup canvasGroup;

    void Start()
    {
        buttonClose = GetComponent<Button>();
        buttonClose.onClick.AddListener(CloseSettingsDialog);
        canvasGroup = settingsDialog.GetComponent<CanvasGroup>();
    }

    void CloseSettingsDialog()
    {
        StartCoroutine(FadeDialog());
    }

    IEnumerator FadeDialog()
    {
        float timeElapsed = 0.0f;
        canvasGroup.alpha = 1f;
        while (timeElapsed < lerpDuration)
        {
            canvasGroup.alpha = 1.0f - (timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        settingsDialog.SetActive(false);
    }
}
