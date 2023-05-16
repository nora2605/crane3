using System.Collections;
using UnityEngine;

namespace Assets.Source.Scripts.Game
{
    public class Level3DController : MonoBehaviour
    {
        public LevelLoader loader;
        internal CraneController cc;
        public ButtonStart sButton;

        private void Start()
        {
            cc = loader.crane;
            Level.crane.CraneBoomChanged += (bool extended) => StartCoroutine(cc.ChangeBoom(extended, sButton.MsSpeed));
            Level.crane.CraneHookChanged += (bool high) => StartCoroutine(cc.ChangeHeight(high, sButton.MsSpeed));
            Level.crane.CraneHoldingChanged += (bool attached) =>
            {
                StartCoroutine(cc.Tach(attached, Level.crane.attached ? loader.tileDict[Crane.inversePropDict[Level.crane.holding]] : null)); // Mess
            };
            Level.crane.CranePositionChanged += () =>
            {
                StartCoroutine(MoveCrane(loader.craneInstance, sButton.MsSpeed));
            };
            Level.crane.CraneDirectionChanged += (bool left) => StartCoroutine(cc.Rotate(left, sButton.MsSpeed));
        }

        public void PReset()
        {
            loader.craneInstance.localPosition = new Vector3(Level.crane.position.Y * 10, 0, Level.crane.position.X * 10);
            StartCoroutine(cc.ChangeBoom(false, 0));
            StartCoroutine(cc.ChangeHeight(false, 0));
            StartCoroutine(cc.Tach(false));
            StartCoroutine(cc.Rotate(false, 0));
            UpdateScene();
        }
        public IEnumerator MoveCrane(Transform movable, float lerpDuration)
        {
            Vector3 lerpto = new(Level.crane.position.Y * 10, 0, Level.crane.position.X * 10);
            Vector3 lerpfrom = movable.localPosition;
            Vector3 lerpdiff = lerpto - lerpfrom;
            float elapsedTime = 0.0f;
            while (elapsedTime < lerpDuration)
            {
                movable.localPosition = lerpfrom + (lerpdiff * elapsedTime / lerpDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            movable.localPosition = lerpto;
            yield return null;
        }

        public void UpdateScene()
        {
            // Place Props and Tiles correctly
            for (int x = 0; x < Level.dimensions.Item1; x++)
            {
                for (int y = 0; y < Level.dimensions.Item2; y++)
                {
                    if (loader.tileInstances[x, y] == null)
                    {
                        if (Level.levelMap[x, y] != Tile.Empty)
                        {
                            loader.tileInstances[x, y] = InstTileProp(true, x, y);
                        }
                        // else do nothing, that's why it has to be nested instead of &&
                    }
                    else if (!loader.tileInstances[x, y].name.StartsWith(Level.levelMap[x, y].ToString()))
                    {
                        // Update go
                        Destroy(loader.tileInstances[x, y]);
                        loader.tileInstances[x, y] = null;
                        if (Level.levelMap[x, y] != Tile.Empty)
                        {
                            loader.tileInstances[x, y] = InstTileProp(true, x, y);
                        }
                    }
                    if (loader.propInstances[x, y] == null)
                    {
                        if (Level.propMap[x, y] != Prop.None)
                        {
                            loader.propInstances[x, y] = InstTileProp(false, x, y);
                        }
                    }
                    else if (!loader.propInstances[x, y].name.StartsWith(Level.propMap[x, y].ToString()))
                    {
                        Destroy(loader.propInstances[x, y]);
                        loader.propInstances[x, y] = null;
                        if (Level.propMap[x, y] != Prop.None)
                        {
                            loader.propInstances[x, y] = InstTileProp(false, x, y);
                        }
                    }
                }
            }
        }

        private GameObject InstTileProp(bool tile, int x, int y)
        {
            GameObject saver = Instantiate(loader.tileDict[tile ? Level.levelMap[x, y] : Crane.inversePropDict[Level.propMap[x, y]]], loader.Anchor);
            saver.transform.localPosition = new Vector3(y * 10, 0, x * 10);
            saver.name = $"{Level.levelMap[x, y]}: {x};{y}";
            saver.SetActive(true);
            return saver; // Why did i name it that way lol
        }
    }
}
