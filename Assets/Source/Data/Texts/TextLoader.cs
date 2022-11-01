using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Data.Texts
{
    internal class TextLoader
    {
        public static TextAsset[] langs;

        public static Schemas.Texts LoadLanguage(string langCode)
        {
            langs = Addressables.LoadAssetsAsync("Texts", (TextAsset t) => { }).WaitForCompletion().ToArray();
            return Schemas.Texts.FromJson(
                langs.FirstOrDefault(x => x.name == langCode)?.text ?? langs.First(x => x.name == "en").text
            );
        } 
    }
}
