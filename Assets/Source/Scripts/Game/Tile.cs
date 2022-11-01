using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Source.Scripts.Game
{
    public enum Tile
    {
        Empty,
        Crate, // Get this on the target
        Block, // Big obstacle
        Slab, // Same as block but only 1 high
        Target, // get a crate on here
        Switch, // might do something to some blocks or slabs if a crate is placed upon
        Prop, // Movable as a crate, useless as a block
        Track // The ground on which the crane can move
    }
}
