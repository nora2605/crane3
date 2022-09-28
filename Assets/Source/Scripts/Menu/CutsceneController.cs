using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneController : MonoBehaviour
{
    private VideoPlayer vp;
    public Canvas canvas;
    private CanvasGroup canvasGroup;
    private AudioSource aS;

    private float lerpDuration = 3.0f;
    private bool done = false;

    void Start()
    {
        vp = Camera.main.GetComponent<VideoPlayer>();
        aS = Camera.main.GetComponent<AudioSource>();

        canvasGroup = canvas.GetComponent<CanvasGroup>();
        canvas.gameObject.SetActive(false);

        vp.loopPointReached += stoppedCutscene;
    }

    void Update()
    {
        if (Input.anyKeyDown && !done)
            stoppedCutscene(vp);
    }

    void stoppedCutscene(VideoPlayer source)
    {
        StartCoroutine(StopCutscene());
    }

    IEnumerator StopCutscene()
    {
        done = true;
        float timeElapsed = 0.0f;

        vp.Stop();
        vp.enabled = false;
        canvas.gameObject.SetActive(true);
        canvasGroup.alpha = 0;
        aS.loop = true;
        aS.Play();

        while (timeElapsed < lerpDuration)
        {
            canvasGroup.alpha = timeElapsed / lerpDuration;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1.0f;
    }
}
