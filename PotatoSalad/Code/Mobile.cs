using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PotatoSalad
{
    public class Mobile
    {
        // A mobile is the basis for the player and monster classes.
        // Every Tile has one slot for a mobile.

        public string id;   // Every mobile needs a unique ID.
        public string name; // The display name.
        public string displayGraphic;   // The image which is the thing.
        public Tile location;   // A reference to the containing tile.
        public int FOVRange;
        public int AI_type;     // I can't work out how to pull this selectively.
        public string[,] skillArray;   // Stores skillName [0], skillRating [1], and skillChecks [2]

        public int health;
        public int mana;

        public Mobile(Tile loc, string uniqueID)
        {
            name = "mobile";
            id = uniqueID;
            location = loc;
            displayGraphic = "../../Graphics/Mobiles/player.png";
            FOVRange = 5;   // Just a default value, for testing.

            // At some point we'll have to initiate all skills.
            skillArray = new string[1, 3];
            skillArray[0, 0] = "unarmed";
            skillArray[0, 1] = "5";     //Too low for most skills; fix in final version
            skillArray[0, 2] = "0";
        }

        public int X()
        {
            return location.X;
        }

        public int Y()
        {
            return location.Y;
        }

        public void MoveTo(int destX, int destY)
        {
            int origX = X();
            int origY = Y();

            if (destX < 0 || destY < 0 || destX > Game.DungeonMap.XDimension || destY > Game.DungeonMap.YDimension)
            {
                // You're trying to move off the edge of the map. Nope.
                //Game.GAPI.RenderText($"No movement! {this.id} is trying to walk off the map at {destX}, {destY}.");
                return;
            }

            Tile newLoc = Game.DungeonMap.TileArray[destX, destY];

            // Check if the tile is walkable.
            if (newLoc.BlockMovement)
            {
                //Game.GAPI.RenderText($"No movement! {this.id} bumped into {newLoc.Name}.");
                return;
            }

            // Check if the tile has a mob in it.
            if (newLoc.Occupier != null)
            {
                //Game.GAPI.RenderText($"No movement! {this.id} bumped into {newLoc.Occupier.name}.");
                return;
            }

            // If clear, move self there.
            location.Occupier = null;
            this.location = newLoc;
            newLoc.Occupier = this;

            // Update the form.
            // This may need to move to a turn update method at some point.

            Game.GAPI.DrawMap(Game.DungeonMap);
        }

        public virtual string GenerateSaveXML()
        {
            // For overwriting.
            string gsx = "";
            return gsx;
        }
    }
}
