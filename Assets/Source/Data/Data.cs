using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Data
{
    internal static class DataIO
    {
        public static string LangCode
        {
            get
            {
                string res = "en";
                if (langCode == null || langCode.Length == 0)
                {
                    try
                    {
                        res = File.ReadAllText(Application.persistentDataPath + "/lang.dat");
                    }
                    catch (FileNotFoundException)
                    {
                        File.WriteAllText(Application.persistentDataPath + "/lang.dat", res);
                    }
                }
                else res = langCode;
                return res;
            }
            set
            {
                langCode = value;
                File.WriteAllText(Application.persistentDataPath + "/lang.dat", value);
            }
        }

        private static string langCode;
        public static Resolution Resolution
        {
            get
            {
                Resolution res = Screen.currentResolution;
                if (resolution.height < 1 || resolution.width < 1 || resolution.refreshRate < 1)
                {
                    try
                    {
                        int[] dimensions = File.ReadAllText(Application.persistentDataPath + "/res.dat").Split("x").Select(x => Convert.ToInt32(x)).ToArray();
                        res = new Resolution() { width = dimensions[0], height = dimensions[1], refreshRate = 60 };
                    }
                    catch (FileNotFoundException)
                    {
                        File.WriteAllText(Application.persistentDataPath + "/res.dat", res.width + "x" + res.height);
                    }
                }
                else res = resolution;
                return res;
            }
            set
            {
                resolution = value;
                File.WriteAllText(Application.persistentDataPath + "/res.dat", value.width + "x" + value.height);
            }
        }

        private static Resolution resolution;
    }
}
