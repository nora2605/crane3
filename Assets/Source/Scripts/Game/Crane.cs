using System;
using System.Collections.Generic;
using System.Drawing;

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
        public Point AimingAt => direction switch // Coordinate Origin is Top-left
        {
            Direction.Right => new Point(position.X + (extended ? 2 : 1), position.Y),
            Direction.Left => new Point(position.X - (extended ? 2 : 1), position.Y),
            Direction.Up => new Point(position.X, position.Y - (extended ? 2 : 1)),
            Direction.Down => new Point(position.X, position.Y + (extended ? 2 : 1)),
            Direction.RightDown => new Point(position.X + 1, position.Y + 1),
            Direction.LeftDown => new Point(position.X - 1, position.Y + 1),
            Direction.RightUp => new Point(position.X + 1, position.Y - 1),
            Direction.LeftUp => new Point(position.X - 1, position.Y - 1),
            _ => new Point(position.X + 1, position.Y),
        };

        public Point? BoomPositionRelative()
        {
            return !extended || (int)direction % 2 == 1
                ? null
                : direction switch
                {
                    Direction.Right => (Point?)new Point(1, 0),
                    Direction.Left => (Point?)new Point(-1, 0),
                    Direction.Up => (Point?)new Point(0, -1),
                    Direction.Down => (Point?)new Point(0, 1),
                    _ => null,
                };
        }
        public static Dictionary<Tile, Prop> propDict = new() {
            { Tile.Crate, Prop.Crate },
            { Tile.Prop, Prop.Prop }
        };

        public static Dictionary<Prop, Tile> inversePropDict = new()
        {
            { Prop.Crate, Tile.Crate },
            { Prop.Prop, Tile.Prop }
        };
        private bool IsFree(int cx, int cy)
        {
            return cx < 0 ||
            cy < 0 ||
            cx >= Level.dimensions.Item1 ||
            cy >= Level.dimensions.Item2 ||
            ((Level.levelMap[cx, cy] != Tile.Slab || high) &&
            Level.levelMap[cx, cy] != Tile.Block);
        }

        public void MoveUpRight()
        {
            if (position.Y - 1 < 0 && position.X + 1 >= Level.dimensions.Item1)
            {
                return;
            }

            if (Level.levelMap[position.X, position.Y - 1] == Tile.Track) // Prioritize Up over Right
            {
                if (IsFree(AimingAt.X, AimingAt.Y - 1))
                {
                    position = new Point(position.X, position.Y - 1);
                    CranePositionChanged.Invoke();
                }
                else
                {
                    // SFX(Collision);
                }
            }
            else if (Level.levelMap[position.X + 1, position.Y] == Tile.Track)
            {
                if (IsFree(AimingAt.X + 1, AimingAt.Y))
                {
                    position = new Point(position.X + 1, position.Y);
                    CranePositionChanged.Invoke();
                }
            }
        }

        public void MoveDownLeft()
        {
            if (position.X - 1 < 0 && position.Y + 1 >= Level.dimensions.Item2)
            {
                return;
            }

            if (Level.levelMap[position.X - 1, position.Y] == Tile.Track) // Prioritize Left over Down
            {
                if (IsFree(AimingAt.X - 1, AimingAt.Y))
                {
                    position = new Point(position.X - 1, position.Y);
                    CranePositionChanged.Invoke();
                }
            }
            else if (Level.levelMap[position.X, position.Y + 1] == Tile.Track)
            {
                if (IsFree(AimingAt.X, AimingAt.Y + 1))
                {
                    position = new Point(position.X, position.Y + 1);
                    CranePositionChanged.Invoke();
                }
            }
        }

        public void HookUp()
        {
            high = true;
            CraneHookChanged.Invoke(true);
        }

        public void HookDown()
        {
            if (!AimingInBounds() || Level.levelMap[AimingAt.X, AimingAt.Y] != Tile.Slab)
            {
                high = false;
                CraneHookChanged.Invoke(false);
            }
            else
            {
                // SFX(Collision);
            }
        }

        private bool AimingInBounds()
        {
            return AimingAt.X >= 0 && AimingAt.X < Level.dimensions.Item1 && AimingAt.Y >= 0 && AimingAt.Y < Level.dimensions.Item2;
        }

        public void Attach()
        {
            if (!high && !attached && AimingInBounds())
            {
                if (propDict.ContainsKey(Level.levelMap[AimingAt.X, AimingAt.Y]))
                {
                    holding = propDict[Level.levelMap[AimingAt.X, AimingAt.Y]];
                    attached = true;
                    Level.levelMap[AimingAt.X, AimingAt.Y] = Tile.Empty;
                    CraneHoldingChanged.Invoke(true);
                }
                else if (Level.propMap[AimingAt.X, AimingAt.Y] != Prop.None)
                {
                    holding = Level.propMap[AimingAt.X, AimingAt.Y];
                    attached = true;
                    Level.propMap[AimingAt.X, AimingAt.Y] = Prop.None;
                    CraneHoldingChanged.Invoke(true);
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
                CraneHoldingChanged.Invoke(false);
            }
        }

        public void Extend()
        {
            extended = true;
            if (IsFree(AimingAt.X, AimingAt.Y))
            {
                CraneBoomChanged.Invoke(true);
                return;
            }
            extended = false;
        }

        public void Retract()
        {
            extended = false;
            if (IsFree(AimingAt.X, AimingAt.Y))
            {
                CraneBoomChanged.Invoke(false);
                return;
            }
            extended = true;
        }

        public void TurnRight()
        {
            direction = (Direction)(((int)direction + 1) % 8);
            if ((IsFree(AimingAt.X, AimingAt.Y)
                && !BoomPositionRelative().HasValue)
                || IsFree(position.X + BoomPositionRelative().Value.X, position.Y + BoomPositionRelative().Value.Y))
            {
                CraneDirectionChanged.Invoke(false);
                return;
            }
            direction = (Direction)(((int)direction + 7) % 8);
        }

        public void TurnLeft()
        {
            direction = (Direction)(((int)direction + 7) % 8);
            if ((IsFree(AimingAt.X, AimingAt.Y)
                && !BoomPositionRelative().HasValue)
                || IsFree(position.X + BoomPositionRelative().Value.X, position.Y + BoomPositionRelative().Value.Y))
            {
                CraneDirectionChanged.Invoke(true);
                return;
            }
            direction = (Direction)(((int)direction + 1) % 8);
        }

        public bool IsAboveTile(Tile tile)
        {
            return Level.levelMap[AimingAt.X, AimingAt.Y] == tile
            || (propDict.ContainsKey(tile) && Level.propMap[AimingAt.X, AimingAt.Y] == propDict[tile]);
        }

        public bool IsAtEnd(bool upright)
        {
            return !(upright
            ? (Level.levelMap[position.X, position.Y - 1] == Tile.Track || Level.levelMap[position.X + 1, position.Y] == Tile.Track)
            : (Level.levelMap[position.X - 1, position.Y] == Tile.Track || Level.levelMap[position.X, position.Y + 1] == Tile.Track));
        }

        public void ExecuteSwitchMap(List<(Point, Point, Tile, Tile)> switchmap)
        {
            foreach ((Point, Point, Tile, Tile) sm in switchmap)
            {
                Level.levelMap[sm.Item2.X, sm.Item2.Y] = Level.propMap[sm.Item1.X, sm.Item1.Y] != Prop.None ? sm.Item4 : sm.Item3;
            }
        }

        public event Action CranePositionChanged;
        public event Action<bool> CraneDirectionChanged;
        public event Action<bool> CraneBoomChanged;
        public event Action<bool> CraneHookChanged;
        public event Action<bool> CraneHoldingChanged;
    }
}
