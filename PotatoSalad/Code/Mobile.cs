﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Mobile(Tile loc, string uniqueID)
        {
            name = "mobile";
            id = uniqueID;
            location = loc;
            //displayGraphic = System.Environment.CurrentDirectory;
            displayGraphic = "../../Graphics/Mobiles/player.png";
            FOVRange = 5;   // Just a default value, for testing.
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
                Game.GAPI.RenderText($"No movement! Trying to walk off the map at {destX}, {destY}.");
                return;
            }

            Tile newLoc = Game.DungeonMap.TileArray[destX, destY];

            // Check if the tile is walkable.
            if (newLoc.BlockMovement)
            {
                Game.GAPI.RenderText($"No movement! Bumped into {newLoc.Name}.");
                return;
            }

            // Check if the tile has a mob in it.
            if (newLoc.Occupier != null)
            {
                Game.GAPI.RenderText($"No movement! Bumped into {newLoc.Occupier.name}.");
                return;
            }

            // If clear, move self there.
            location.Occupier = null;
            this.location = newLoc;
            newLoc.Occupier = this;

            // Update the form.
            // This may need to move to a turn update method at some point.

            // TODO: None of this is necessary?
            //if (id == "UniqueIDPlayer")
            //{
            //    // If it's the player, then we need to quickly update their location.
            //    Game.XMLHandler.UpdatePlayerLocation(Game.PlayerXML);
            //    // And the revealed sections of the map.
            //}
            Game.GAPI.DrawMap(Game.DungeonMap);
        }
    }
}
