using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Scripts.Menu.PlayMenu
{
    public class SaveStatSelector : MonoBehaviour
    {
        public string saveName;
        private UnityEngine.UI.Button saveStat;

        void Start()
        {
            saveStat = GetComponent<UnityEngine.UI.Button>();
            saveStat.onClick.AddListener(ChangeSelected);
        }

        void ChangeSelected()
        {
            Indexer.currentSelected = saveName;
            Indexer.selected = true;
        }
    }
}
