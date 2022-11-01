using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Scripts.Game
{
    public class Level3DController : MonoBehaviour
    {
        public LevelLoader loader;
        internal CraneController cc;

        private void Start()
        {
            cc = loader.crane;
        }

        private void Update()
        {
            // Responsible for animating the Enumeration of the bytecode interpretation in the 3d view
        }
    }
}
