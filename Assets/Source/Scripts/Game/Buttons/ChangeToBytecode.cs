using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.Scripts.Game.Buttons
{
    public class ChangeToBytecode : MonoBehaviour
    {
        private Button cbbc;
        public static bool byteedit = false; // Maybe going into a .dat for preferences
        public GameObject[] VisEdit;
        public GameObject[] ByteEdit;

        private void Start()
        {
            cbbc = GetComponent<Button>();
            cbbc.onClick.AddListener(ChangeToByteCode);
        }

        private void ChangeToByteCode()
        {
            byteedit = !byteedit;
            foreach (GameObject go in VisEdit)
            {
                go.SetActive(!byteedit);
            }
            foreach (GameObject go in ByteEdit)
            {
                go.SetActive(byteedit);
            }
        }
    }
}
