using Assets.Source.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

namespace Menu.Settings
{
    internal class Resolution : MonoBehaviour
    {
        public Button apply;
        private TMP_Dropdown dropdown;

        public void Start()
        {
            apply.onClick.AddListener(Apply);

            dropdown = GetComponent<TMP_Dropdown>();

            dropdown.ClearOptions();
            UnityEngine.Resolution[] ress = Screen.resolutions;
            List<TMP_Dropdown.OptionData> dres = new();
            foreach (UnityEngine.Resolution res in ress)
            {
                dres.Add(new TMP_Dropdown.OptionData(res.width + "x" + res.height));
            }
            gameObject.GetComponent<TMP_Dropdown>().AddOptions(dres);

            dropdown.value = dropdown.options.IndexOf(
                dropdown.options.First(x => x.text == DataIO.Resolution.width + "x" + DataIO.Resolution.height)
            );

            Apply();
        }

        public void Apply()
        {
            int[] res = dropdown.options[dropdown.value].text.Split("x").Select(x => Convert.ToInt32(x)).ToArray();
            DataIO.Resolution = new UnityEngine.Resolution() { width = res[0], height = res[1], refreshRate = 60 };
            Screen.SetResolution(DataIO.Resolution.width, DataIO.Resolution.height, true); // true to be replaced with a checkbox or sth
        }
    }
}
