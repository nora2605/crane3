using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Source.Scripts.Game
{
    public class Crane
    {
        public Prop holding = Prop.None;
        public Point position;
        public Direction direction = Direction.Right;
        public bool extended = false;
        public bool high = false;
        public bool attached = false;
        public Point AimingAt { get {
                switch (direction) // Coordinate Origin is Top-left
                {
                    case Direction.Right:
                        return new Point(position.X + (extended ? 2 : 1), position.Y);
                    case Direction.Left:
                        return new Point(position.X - (extended ? 2 : 1), position.Y);
                    case Direction.Up:
                        return new Point(position.X, position.Y - (extended ? 2 : 1));
                    case Direction.Down:
                        return new Point(position.X, position.Y + (extended ? 2 : 1));
                    case Direction.RightDown:
                        return new Point(position.X + 1, position.Y + 1);
                    case Direction.LeftDown:
                        return new Point(position.X - 1, position.Y + 1);
                    case Direction.RightUp:
                        return new Point(position.X + 1, position.Y - 1);
                    case Direction.LeftUp:
                        return new Point(position.X - 1, position.Y - 1);
                    default:
                        return new Point(position.X + 1, position.Y);
                }
            }
        }
        public Dictionary<Tile, Prop> propDict = new () {
            { Tile.Crate, Prop.Crate },
            { Tile.Prop, Prop.Prop }
        };

        private bool IsFree(int cx, int cy) => 
            cx < 0 ||
            cy < 0 ||
            cx >= Level.dimensions.Item1 ||
            cy >= Level.dimensions.Item2 ||
            (Level.levelMap[cx, cy] != Tile.Slab || high) &&
            Level.levelMap[cx, cy] != Tile.Block;

        public void MoveUpRight()
        {
            if (position.Y - 1 < 0 || position.X + 1 >= Level.dimensions.Item1) return;
            if (Level.levelMap[position.X, position.Y - 1] == Tile.Track) // Prioritize Up over Right
            {
                if (IsFree(AimingAt.X, AimingAt.Y - 1))
                    position = new Point(position.X, position.Y - 1);
                else
                {
                    // SFX(Collision);
                }
            }
            else if (Level.levelMap[position.X + 1, position.Y] == Tile.Track) { 
                if (IsFree(AimingAt.X + 1, AimingAt.Y))
                    position = new Point(position.X + 1, position.Y);
            }
        }

        public void MoveDownLeft()
        {
            if (position.X - 1 < 0 || position.Y + 1 >= Level.dimensions.Item2) return;
            if (Level.levelMap[position.X - 1, position.Y] == Tile.Track) // Prioritize Left over Down
            {
                if (IsFree(AimingAt.X - 1, AimingAt.Y))
                    position = new Point(position.X - 1, position.Y);
            }
            else if (Level.levelMap[position.X, position.Y + 1] == Tile.Track)
            {
                if (IsFree(AimingAt.X, AimingAt.Y + 1))
                    position = new Point(position.X, position.Y + 1);
            }
        }

        public void HookUp()
        {
            high = true;

        }

        public void HookDown()
        {
            if (Level.levelMap[AimingAt.X, AimingAt.Y] != Tile.Slab)
                high = false;
            else
            {
                // SFX(Collision);
            }
        }

        private bool AimingInBounds() => AimingAt.X >= 0 && AimingAt.X < Level.dimensions.Item1 && AimingAt.Y >= 0 && AimingAt.Y < Level.dimensions.Item2;

        public void Attach()
        {
            if (!high && !attached && AimingInBounds())
            {
                if (propDict.ContainsKey(Level.levelMap[AimingAt.X, AimingAt.Y]))
                {
                    holding = propDict[Level.levelMap[AimingAt.X, AimingAt.Y]];
                    attached = true;
                    Level.levelMap[AimingAt.X, AimingAt.Y] = Tile.Empty;
                }
                else if (Level.propMap[AimingAt.X, AimingAt.Y] != Prop.None)
                {
                    holding = Level.propMap[AimingAt.X, AimingAt.Y];
                    attached = true;
                    Level.propMap[AimingAt.X, AimingAt.Y] = Prop.None;
                }
            }
        }

        public void Detach()
        {
            if (!high && attached && AimingInBounds())
            {
                Level.propMap[AimingAt.X, AimingAt.Y] = holding;
                holding = Prop.None;
                attached = false;
            }
        }

        public void Extend()
        {
            extended = true;
            if (IsFree(AimingAt.X, AimingAt.Y)) return;
            extended = false;
        }

        public void Retract()
        {
            extended = false;
            if (IsFree(AimingAt.X, AimingAt.Y)) return;
            extended = true;
        }

        public void TurnRight()
        {
            direction = (Direction)(((int)direction + 1) % 8);
            if (IsFree(AimingAt.X, AimingAt.Y)) return;
            direction = (Direction)(((int)direction + 7) % 8);
        }

        public void TurnLeft()
        {
            direction = (Direction)(((int)direction + 7) % 8);
            if (IsFree(AimingAt.X, AimingAt.Y)) return;
            direction = (Direction)(((int)direction + 1) % 8);
        }

        public bool IsAboveTile(Tile tile) => Level.levelMap[AimingAt.X, AimingAt.Y] == tile 
            || Level.propMap[AimingAt.X, AimingAt.Y] == propDict[tile];

        public bool IsAtEnd(bool upright) => !(upright
            ? (Level.levelMap[position.X, position.Y - 1] == Tile.Track || Level.levelMap[position.X + 1, position.Y] == Tile.Track)
            : (Level.levelMap[position.X - 1, position.Y] == Tile.Track || Level.levelMap[position.X, position.Y + 1] == Tile.Track));

        public void ExecuteSwitchMap(List<(Point, Point, Tile, Tile)> switchmap)
        {
            foreach (var sm in switchmap)
            {
                if (Level.propMap[sm.Item1.X, sm.Item1.Y] != Prop.None)
                {
                    Level.levelMap[sm.Item2.X, sm.Item2.Y] = sm.Item4;
                }
                else
                {
                    Level.levelMap[sm.Item2.X, sm.Item2.Y] = sm.Item3;
                }
            }
        }
    }
}
