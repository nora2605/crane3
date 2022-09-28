using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Data.Texts
{
    internal class TextLoader
    {
        public static TextAsset[] langs;

        public static Schemas.Texts LoadLanguage(string langCode)
        {
			langs = Resources.LoadAll("Texts/", typeof(TextAsset))
				.Select(x => (TextAsset)x).ToArray();
            return Schemas.Texts.FromJson(
                langs.First(x => x.name == langCode)?.text ?? langs.First(x => x.name == "en").text
            );
        } 
    }
}
