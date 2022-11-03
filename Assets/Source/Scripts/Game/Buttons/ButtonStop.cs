using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.Scripts.Game.Buttons
{
    public class ButtonStop : MonoBehaviour
    {
        public ButtonStart startButton;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(StopInterpreter);
        }

        private void StopInterpreter()
        {
            startButton.StopCoroutine(startButton.interpreter);
            startButton.GetComponent<Button>().enabled = true;
            startButton.ResetScene();
        }
    }
}
