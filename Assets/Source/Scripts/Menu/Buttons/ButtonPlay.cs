using Data.Texts.Schemas;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.Scripts
{

    public class ButtonPlay : MonoBehaviour
    {
        public SceneLoader transition;

        private Button buttonPlay;

        private float lerpDuration = 0.1f;
        private AudioSource audioSource;

        void Start()
        {
            buttonPlay = this.gameObject.GetComponent<Button>();
            buttonPlay.onClick.AddListener(buttonPlay_Click);
        }

        void buttonPlay_Click()
        {
            audioSource = Camera.main.GetComponent<AudioSource>();
            StartCoroutine(FadeOutVol());
            // where is value = value == x ? y : x, it could be as easy as volume ?= x : y; the flipflop operator random thought really

            // Redirect
            StartCoroutine(transition.LoadScene(1));
        }

        IEnumerator FadeOutVol()
        {
            float timeElapsed = 0.0f;
            while (timeElapsed < lerpDuration)
            {
                audioSource.volume = 1f - timeElapsed / lerpDuration;
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            audioSource.volume = 0f;
            yield return null;
        }
    }
}
