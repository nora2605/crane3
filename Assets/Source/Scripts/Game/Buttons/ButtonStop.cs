using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            startButton.ResetScene();
        }
    }
}
