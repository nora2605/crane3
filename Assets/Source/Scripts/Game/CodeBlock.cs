using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Assets.Source.Scripts.Game
{
    public enum Codes
    {
        UpRight,
        DownLeft,
        HookUp,
        HookDown,
        Attach,
        Detach,
        Extend,
        Retract,
        TurnRight,
        TurnLeft,
        LoopLeft,
        LoopRight,
        ConditionalLeft,
        ConditionalRight,
        IsCrate,
        Negate,
        IsTarget,
        IsSwitch,
        Infinite,
        Break,
        AtEndUpRight,
        AtEndDownLeft,
        Number
    }
    public class CodeBlock
    {
        public Image gObject;

        public CodeBlock(Image gObject)
        {
            this.gObject = gObject;
        }

        public Codes codeType;
        [HideInInspector]
        public int? number;
        public static Dictionary<Codes, string> codeImgDict = new()
        {
            { Codes.UpRight, "upright" },
            { Codes.DownLeft, "downleft" },
            { Codes.HookUp, "hookup" },
            { Codes.HookDown, "hookdown" },
            { Codes.Attach, "attach" },
            { Codes.Detach, "detach" },
            { Codes.Extend, "extend" },
            { Codes.Retract, "retract" },
            { Codes.TurnRight, "right" },
            { Codes.TurnLeft, "left" },
            { Codes.LoopLeft, "loopstart" },
            { Codes.LoopRight, "loopend" },
            { Codes.ConditionalLeft, "condstart" },
            { Codes.ConditionalRight, "condend" },
            { Codes.IsCrate, "crate" },
            { Codes.IsTarget, "target" },
            { Codes.IsSwitch, "switch" },
            { Codes.Break, "break" },
            { Codes.Negate, "negate" },
            { Codes.Infinite, "infinite" },
            { Codes.AtEndDownLeft, "atenddl" },
            { Codes.AtEndUpRight, "atendur" },
            { Codes.Number, null }
        };
        public static Dictionary<Codes, string> codeReprDict = new()
        {
            { Codes.UpRight, ">" },
            { Codes.DownLeft, "<" },
            { Codes.HookUp, "/" },
            { Codes.HookDown, "\\" },
            { Codes.Attach, "." },
            { Codes.Detach, "," },
            { Codes.Extend, "+" },
            { Codes.Retract, "-" },
            { Codes.TurnRight, "d" },
            { Codes.TurnLeft, "a" },
            { Codes.LoopLeft, "[" },
            { Codes.LoopRight, "]" },
            { Codes.ConditionalLeft, "(" },
            { Codes.ConditionalRight, ")" },
            { Codes.IsCrate, "C" },
            { Codes.IsTarget, "T" },
            { Codes.IsSwitch, "S" },
            { Codes.Break, ";" },
            { Codes.Negate, "!" },
            { Codes.Infinite, "*" },
            { Codes.AtEndDownLeft, "_" },
            { Codes.AtEndUpRight, "|" },
            { Codes.Number, null }
        };

        public void InitializeCodeType()
        {
            gObject.sprite = Addressables.LoadAssetAsync<Sprite>("block_" + (codeType == Codes.Number ? number.ToString() : codeImgDict[codeType])).WaitForCompletion();
        }

        public string ToRepresentativeCode()
        {
            return codeReprDict[codeType] ?? Convert.ToString(number ?? 1);
        }
    }
}
