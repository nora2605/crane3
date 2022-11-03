using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Data.Texts
{
    internal class TextLoader
    {
        public static TextAsset[] langs;

        public static Schemas.Texts LoadLanguage(string langCode)
        {
            langs = Addressables.LoadAssetsAsync("Texts", (TextAsset t) => { }).WaitForCompletion().ToArray();
            return Schemas.Texts.FromJson(
                (langs.Any(x => x.name == langCode) ? langs.First(x => x.name == langCode) : langs.First(x => x.name == "en")).text
            );
        }
    }
}
