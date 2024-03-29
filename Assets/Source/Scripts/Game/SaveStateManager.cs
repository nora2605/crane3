﻿using System.IO;
using System.Text;
using UnityEngine;

namespace Assets.Source.Scripts.Game
{
    public static class SaveStateManager
    {
        public static string gameName = "debugGame";
        public static void Save()
        {
            if (gameName.Length == 0)
            {
                return;
            }
            File.WriteAllText(Application.persistentDataPath + "/saves/" + gameName, $"{LocalGame.levelNumber}", Encoding.UTF8);
        }

        public static void Load()
        {
            if (gameName.Length == 0 || !File.Exists(Application.persistentDataPath + "/saves/" + gameName)) { return; }
            string gameFile = File.ReadAllText(Application.persistentDataPath + "/saves/" + gameName, Encoding.UTF8);
            LocalGame.levelNumber = int.Parse(gameFile);
            LocalGame.dialogNumber = LocalGame.levelNumber - 1;
            LocalGame.cutsceneNumber = -1;
        }
    }
}
