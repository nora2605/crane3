﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.Scripts.Game
{
    public class Level2DController : MonoBehaviour
    {
        public Image block;

        private Dictionary<Tile, Color> colorDictT = new()
        {
            { Tile.Empty, Color.white },
            { Tile.Block, Color.red },
            { Tile.Slab, new Color(0.6f, 0.23f, 0f) },
            { Tile.Crate, new Color(0.4f, 0.19f, 0.05f) },
            { Tile.Target, Color.green },
            { Tile.Prop, Color.yellow },
            { Tile.Switch, Color.black },
            { Tile.Track, Color.blue }
        };
        private Dictionary<Prop, Color> colorDictP = new()
        {
            { Prop.None, new Color(0, 0, 0, 0) },
            { Prop.Crate, new Color(0.4f, 0.19f, 0.05f) },
            { Prop.Prop, Color.yellow }
        };

        private Image[,] tiles;
        private Image[,] props;
        private Image crane;
        private int edgeLength;

        private void Start()
        {
            RectTransform canvas = GetComponentInParent<RectTransform>();

            edgeLength = (int)Mathf.Min(canvas.rect.width/Level.dimensions.Item1, canvas.rect.height/Level.dimensions.Item2);
            
            tiles = new Image[Level.dimensions.Item1, Level.dimensions.Item2];
            props = new Image[Level.dimensions.Item1, Level.dimensions.Item2];

            for (int x = 0; x < Level.dimensions.Item1; x++)
            {
                for (int y = 0; y < Level.dimensions.Item2; y++)
                {
                    
                        tiles[x, y] = Instantiate(block, transform);
                        tiles[x, y].name = $"{Level.levelMap[x, y]}: {x};{y}";
                        tiles[x, y].color = colorDictT[Level.levelMap[x, y]];
                        tiles[x, y].rectTransform.anchoredPosition = new Vector2(x * edgeLength, -y * edgeLength);
                        tiles[x, y].rectTransform.sizeDelta = new Vector2(edgeLength, edgeLength);
                        tiles[x, y].gameObject.SetActive(true);
                        props[x, y] = Instantiate(block, transform);
                        props[x, y].color = colorDictP[Level.propMap[x, y]];
                        props[x, y].name = $"{Level.propMap[x, y]}: {x};{y}";
                        props[x, y].rectTransform.anchoredPosition = new Vector2((x + 0.1f) * edgeLength, -(y + 0.1f) * edgeLength);
                        props[x, y].rectTransform.sizeDelta = new Vector2(edgeLength, edgeLength);
                        props[x, y].gameObject.SetActive(true);
                }
            }
            crane = Instantiate(block, transform);
            crane.name = "Crane";
            crane.color = new Color(0.8f, 0.5f, 0f);
            crane.rectTransform.sizeDelta = new Vector2(1.8f * edgeLength, 0.8f * edgeLength);
            crane.rectTransform.pivot = new Vector2(0.25f, 0.5f);
            crane.rectTransform.anchoredPosition = new Vector2((Level.crane.position.X + 0.1f) * edgeLength, -(Level.crane.position.Y + 0.1f) * edgeLength);
            crane.gameObject.SetActive(true);

        }
        // Reponsible for Animating what happens during enumeration in the 2d View

        private void Update()
        {
            UpdateTileMap();
        }

        private void UpdateTileMap()
        {
            for (int x = 0; x < Level.dimensions.Item1; x++)
            {
                for (int y = 0; y < Level.dimensions.Item2; y++)
                {
                    tiles[x, y].color = colorDictT[Level.levelMap[x, y]];
                    props[x, y].color = colorDictP[Level.propMap[x, y]];
                }
            }
            crane.rectTransform.anchoredPosition = new Vector2((Level.crane.position.X + 0.6f) * edgeLength, -(Level.crane.position.Y + 0.55f) * edgeLength);
            crane.rectTransform.sizeDelta = Level.crane.extended ? new Vector2(2.8f * edgeLength, 0.8f * edgeLength) : new Vector2(1.8f * edgeLength, 0.8f * edgeLength);
            crane.rectTransform.localRotation = Quaternion.Euler(0.0f,0.0f,-(int)Level.crane.direction * 45f);
            crane.gameObject.SetActive(true);
        }
    }
}
