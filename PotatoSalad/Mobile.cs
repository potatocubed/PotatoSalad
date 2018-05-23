using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotatoSalad
{
    class Mobile
    {
        // A mobile is the basis for the player and monster classes.
        // Every Tile has one slot for a mobile.

        private string id;   // Every mobile needs a unique ID.
        public string name; // The display name.
        public string displayGraphic;   // The image which is the thing.
        public Tile location;   // A reference to the containing tile.

        public Mobile(Tile loc)
        {
            name = "mobile";
            id = GenID();
            location = loc;
        }

        private string GenID()
        {
            Guid guid = new Guid();
            string s = guid.ToString();
            return s;
        }

        public int X()
        {
            return location.X;
        }

        public int Y()
        {
            return location.Y;
        }
    }
}
