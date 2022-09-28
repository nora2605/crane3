using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;
using UnityEditor;
using System.Runtime.Serialization;
using System.Net.Http.Headers;

#pragma warning disable CS0649

namespace Data.Texts.Schemas
{
    [Serializable]
    internal class Texts
    {
        public Cutscene[] cutscenes;
        public Dialog[] dialog;
        public Dialog[] extras;
        public Dictionary<string, string> UI { get => ToDictionary(this.ui); }
        public KVPair[] ui;

        public Texts(Cutscene[] cutscenes, Dialog[] dialog, Dialog[] extras, KVPair[] ui)
        {
            this.cutscenes = cutscenes;
            this.dialog = dialog;
            this.extras = extras;
            this.ui = ui;
        }

        private Dictionary<string, string> ToDictionary(KVPair[] arr)
        {
            Dictionary<string, string> result = new();
            if (arr != null)
            {
                foreach (KVPair o in arr)
                {
                    result.Add(o.key, o.value);
                }
            }
            return result;
        }

        public static Texts FromJson(string json)
        {
            return JsonUtility.FromJson<Texts>(json);
        }
    }

    [Serializable]
    internal class KVPair
    {
        public string key;
        public string value;

        public KVPair(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
    }

    [Serializable]
    internal class Cutscene
    {
        public string name;
        public TimedText[] subtitles;

        public Cutscene(string name, TimedText[] subtitles)
        {
            this.name = name;
            this.subtitles = subtitles;
        }
    }
    [Serializable]
    internal class TimedText {
        public string text; // text to be displayed
        public int time; // in ms

        public TimedText(string text, int time)
        {
            this.text = text;
            this.time = time;
        }
    }
    [Serializable]
    internal class Dialog
    {
        public string background; // name of background file (e.g. "bg_10km.png")
        public int flags; /*
                                    * 0x01: skippable
                                    * 0x02: auto
                                    * 0x04: nosprites
                                    * 0x08: no BG
                                    */
        public DialogText[] dialogs;

        public Dialog(string background, int flags, DialogText[] dialogs)
        {
            this.background = background;
            this.flags = flags;
            this.dialogs = dialogs;
        }
    }
    [Serializable]
    internal class DialogText
    {
        public DialogType type;
        public string text;
        public string author;
        public string sprite; // name of sprite file (e.g. "sprite_orma_crate.png")
        public uint color;
        public string voiceline; // name of voiceline file (e.g. "va_orma_007.wav"), might switch to Wwise

        public DialogText(DialogType type, string text, string author, string sprite, uint color, string voiceline)
        {
            this.type = type;
            this.text = text;
            this.author = author;
            this.sprite = sprite;
            this.color = color;
            this.voiceline = voiceline;
        }
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

#pragma warning restore 0649