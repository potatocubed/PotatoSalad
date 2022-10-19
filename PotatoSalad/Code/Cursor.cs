using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotatoSalad.Code
{
    public class Cursor
    {
        public string graphic;
        public Tile location;
        public Tile previousLocation;

        public Cursor(Tile loc)
        {
            graphic = "../../Graphics/cursor.png";
            location = loc;
            previousLocation = loc;
        }

        public int X()
        {
            return location.X;
        }

        public int Y()
        {
            return location.Y;
        }

        public bool MoveTo(int destX, int destY)
        {
            int origX = X();
            int origY = Y();

            // don't move off the map
            if (destX < 0 || destY < 0 || destX > Game.DungeonMap.XDimension || destY > Game.DungeonMap.YDimension)
            {
                // You're trying to move off the edge of the map. Nope.
                //Game.GAPI.RenderText($"No movement! Cursor is trying to walk off the map at {destX}, {destY}.");
                return false;
            }

            Tile newLoc = Game.DungeonMap.TileArray[destX, destY];

            this.previousLocation = this.location;
            this.location = newLoc;

            //Game.GAPI.RenderText($"Cursor has moved to {this.X()}, {this.Y()}.");
            Game.GAPI.CursorDrawMap(Game.DungeonMap, this);
            return true;
        }

        public string GenerateDescription(int x, int y)
        {
            // bear in mind 'you can see' vs 'you last saw'
            string s = "";
            s = $"{this.location.Description}";
            if(this.location.Occupier != null)
            {
                string article = "A ";
                string startLetter = this.location.Occupier.name.Substring(0, 1).ToLower();
                if (this.location.Occupier.unique)
                {
                    article = "";
                }
                else if (startLetter == "a" ||
                        startLetter == "i" ||
                        startLetter == "e" ||
                        startLetter == "o" ||
                        startLetter == "u")
                {
                    article = "An ";
                }
                s = $"{s} {article}{this.location.Occupier.name}. {this.location.Occupier.description}";
            }

            // TODO -- list floor inventory.
            // TODO -- *enable* floor inventory as a list of Item objects
            // attached to any given tile.
            return "";
        }
    }
}
