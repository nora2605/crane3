using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.Tilemaps;

namespace Assets.Source.Scripts.Game
{
    public class LevelLoader : MonoBehaviour
    {
        public GameObject ProtoBlock;
        public GameObject ProtoSlab;
        public GameObject ProtoTarget;
        public GameObject ProtoProp;
        public GameObject ProtoSwitch;
        public GameObject ProtoCrate;
        public GameObject ProtoTrack;
        public GameObject ProtoCrane;

        public GameObject Plane;
        public Transform Anchor;

        internal CraneController crane;
        internal Transform craneInstance;
        internal GameObject[,] tileInstances;
        internal GameObject[,] propInstances;

        public Dictionary<Tile, GameObject> tileDict;

        private void Start()
        {
            tileDict = new()
            {
                { Tile.Block, ProtoBlock },
                { Tile.Slab, ProtoSlab },
                {Tile.Target, ProtoTarget },
                { Tile.Prop, ProtoProp },
                { Tile.Switch, ProtoSwitch },
                { Tile.Crate, ProtoCrate },
                { Tile.Track, ProtoTrack },
            };
            string lvl = Addressables.LoadAssetAsync<TextAsset>("Level" + LocalGame.levelNumber).WaitForCompletion().text;
            Level.LoadLevel(lvl);

            tileInstances = new GameObject[Level.dimensions.Item1, Level.dimensions.Item2];
            propInstances = new GameObject[Level.dimensions.Item1, Level.dimensions.Item2];

            Plane.transform.localScale = new Vector3(Level.dimensions.Item2 * 10, 1, Level.dimensions.Item1 * 10);
            Anchor.position = new Vector3(-Level.dimensions.Item2 * 5, 0, -Level.dimensions.Item1 * 5);

            for (int x = 0; x < Level.dimensions.Item1; x++)
            {
                for (int y = 0; y < Level.dimensions.Item2; y++)
                {
                    if (Level.levelMap[x, y] != Tile.Empty)
                    {
                        GameObject t = Instantiate(tileDict[Level.levelMap[x, y]], Anchor);
                        t.transform.localPosition = new Vector3(y * 10, 0, x * 10);
                        t.name = $"{Level.levelMap[x,y]}: {x};{y}";
                        t.SetActive(true);
                        tileInstances[x, y] = t;
                    }
                }
            }
            GameObject crane = Instantiate(ProtoCrane, Anchor);
            this.crane = crane.GetComponent<CraneController>();
            crane.name = "Crane";
            crane.transform.localPosition = new Vector3(Level.crane.position.Y * 10, 0, Level.crane.position.X * 10); // Remember: X and Y are Z and X, not X and Z
            craneInstance = crane.transform;
        }

        // responsible for:
        /*
         * - instantiating prototypes in the 3d view
         */
    }
}
