using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;
using UnityEditor;

namespace Assets.Data.Texts.Schemas
{
    [Serializable]
    internal class Texts
    {
        public readonly Cutscene[] cutscenes;
        public readonly Dialog[] dialog;
        public readonly Dialog[] extras;
        public readonly Dictionary<string, string> UI;

        public static Texts FromJson(string json)
        {
            return JsonUtility.FromJson<Texts>(json);
        }
    }

    [Serializable]
    internal class Cutscene
    {
        public string name;
        public readonly TimedText[] subtitles;
    }
    [Serializable]
    internal struct TimedText {
        public string text; // text to be displayed
        public int time; // in ms
    }
    [Serializable]
    internal class Dialog
    {
        public readonly string background; // name of background file (e.g. "bg_10km.png")
        public readonly int flags; /*
                                    * 0x01: skippable
                                    * 0x02: auto
                                    * 0x04: nosprites
                                    * 0x08: no BG
                                    */
        public readonly DialogText[] dialogs;
    }
    [Serializable]
    internal struct DialogText
    {
        public readonly DialogType type;
        public readonly string text;
        public readonly string author;
        public readonly string sprite; // name of sprite file (e.g. "sprite_orma_crate.png")
        public readonly uint color;
        public readonly string voiceline; // name of voiceline file (e.g. "va_orma_007.wav"), might switch to Wwise
    }
    [Serializable]
    internal enum DialogType
    {
        Pause=0,
        Text=1,
        Prompt=2,
        Erikative=3
    }
}
