using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Source.Scripts.Game
{
    public static class Level
    {
        public static (int, int) dimensions;
        public static Tile[,] levelMap; // Level Map Info 
        public static List<(Point, Point, Tile, Tile)> switchMap = new(); // Switch Point, Target Point, Tile (false), Tile (true)
        public static Prop[,] propMap; // Moved/Movable objects
        public static Crane crane; // The crane (also technically a prop but treated s p e c i a l l y)

        public static Dictionary<string, Tile> tileDict = new()
        {
            { "N", Tile.Empty },
            { "T", Tile.Target },
            { "B", Tile.Block },
            { "t", Tile.Track },
            { "P", Tile.Prop },
            { "C", Tile.Crate },
            { "S", Tile.Switch },
            { "s", Tile.Slab }
        };

        public static void LoadLevel(string lvldata)
        {
            string[] lvl = lvldata.Split('\n').Select(x => x.Replace("\r", "")).ToArray();
            switchMap.Clear();
            dimensions = (int.Parse(lvl[0].Split(';')[0]), int.Parse(lvl[0].Split(';')[1]));
            crane = new()
            {
                position = new Point(int.Parse(lvl[1].Split(';')[0]), int.Parse(lvl[1].Split(';')[1]))
            };
            lvl = lvl.Skip(2).ToArray();
            levelMap = new Tile[dimensions.Item1, dimensions.Item2];
            propMap = new Prop[dimensions.Item1, dimensions.Item2];
            for (int i = 0; i < propMap.Length; i++) propMap[i % dimensions.Item1, i / dimensions.Item1] = Prop.None; // Init all props to none
            for (int i = 0; i < lvl.Length; i++)
            {
                levelMap[i % dimensions.Item1, i / dimensions.Item1] = lvl[i].Length > 0 ? tileDict[lvl[i][..1]] : Tile.Empty;
                if (lvl[i].StartsWith("S"))
                {
                    string dim = lvl[i].Split(' ')[1].Split(':')[0];
                    string falseb = lvl[i].Split(' ')[1].Split(':')[1];
                    string trueb = lvl[i].Split(' ')[1].Split(':')[2]; // Redundancy is cool... right?
                    switchMap.Add((
                        new Point(i % dimensions.Item1, i / dimensions.Item1),
                        new Point(int.Parse(dim.Split(';')[0]), int.Parse(dim.Split(';')[1])),
                        tileDict[falseb],
                        tileDict[trueb]
                    ));
                }
            }
        }

        /* Level Data is structured as follows:
         *  _________________
         *  | T | B | t | P |
         *  L___L___L___L___|
         *  |   |   | t |   |
         *  L___L___L___L___|
         *  | B | B | t | B |
         *  L___L___L___L___|
         *  | C |   | t | S |
         *  L___L___L___L___|
         * 
         * K = Crane
         * C = Crate
         * T = Target
         * B = Block (Obstacle)
         * t = Track
         * 
         * 
         *  1 | 4;4                             | x and y dimensions of the level
         *  2 | 2;1                             | Initial Crane Position
         *  3 | T                               | tile on 0, 0
         *  4 |                                 | tile on 1, 0
         *  5 | t                               | ...
         *  6 | 
         *  7 | 
         *  8 | 
         *  9 | t
         * 10 | 
         * 11 | B
         * 12 | 
         * 13 | t
         * 14 | 
         * 15 | C
         * 16 | 
         * 17 | t
         * 18 | S 1;0:B:N                       | Switch for block on 1;0, is B when not activated, is N (None) when activated
         * 
         * >.2d3<2a,4d+.-2a3>2d+, would be a solution for this
         * 
         */
    }
}
