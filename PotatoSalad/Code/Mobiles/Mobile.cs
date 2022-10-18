using PotatoSalad.Code;
using PotatoSalad.Code.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
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
        public string faction;
        public Tile location;   // A reference to the containing tile.
        public int FOVRange;
        public int AI_type;     // I can't work out how to pull this selectively.
        public string[,] skillArray;   // Stores skillName [0], skillRating [1], and skillChecks [2]
        public Inventory inventory;
        public string description;

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
            skillArray[0, 1] = "5";
            skillArray[0, 2] = "0";

            // Stuff
            inventory = new Inventory();
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

            if (destX < 0 || destY < 0 || destX > Game.DungeonMap.XDimension || destY > Game.DungeonMap.YDimension)
            {
                // You're trying to move off the edge of the map. Nope.
                //Game.GAPI.RenderText($"No movement! {this.id} is trying to walk off the map at {destX}, {destY}.");
                return false;
            }

            Tile newLoc = Game.DungeonMap.TileArray[destX, destY];

            // Check if the tile is walkable.
            if (newLoc.BlockMovement)
            {
                //Game.GAPI.RenderText($"No movement! {this.id} bumped into {newLoc.Name}.");
                return false;
            }

            // Check if the tile has a mob in it.
            if (newLoc.Occupier != null)
            {
                //Game.GAPI.RenderText($"No movement! {this.id} bumped into {newLoc.Occupier.name}.");
                //Game.GAPI.RenderText("Attack!");
                // Wait, monsters shouldn't fight each other. Usually.
                
                // If two people on the same faction move into each other, movement
                // is cancelled.

                // If THE PLAYER moves into an ally, they should switch locations.
                // TODO -- handle that later.

                if(this.faction != newLoc.Occupier.faction)
                {
                    Game.ViolenceHandler.MakeBumpAttack(this, newLoc.Occupier);
                    return true;
                }
                
                return false;
            }

            // If clear, move self there.
            location.Occupier = null;
            this.location = newLoc;
            newLoc.Occupier = this;

            // Update the form.
            // This may need to move to a turn update method at some point.

            Game.GAPI.DrawMap(Game.DungeonMap);
            return true;
        }

        public virtual string GenerateSaveXML()
        {
            // For overwriting.
            string gsx = "";
            return gsx;
        }

        public void DamageHealth(int damage)
        {
            this.health = this.health - damage;
            if(this.health <= 0)
            {
                this.KillSelf();
            }
        }

        public virtual void KillSelf()
        {
            // send message
            Game.GAPI.RenderText($"{Game.GAPI.CapitaliseString(this.name)} dies!");

            // TODO: drop inventory
            // TODO: give XP

            // remove from map
            location.Occupier = null;
            this.location = null;

            // remove from display
            Game.GAPI.DrawMap(Game.DungeonMap);
            // I feel like there must be a more efficient method
            // of updating a single tile than this, but still.

            // remove from mobilelist
            Game.DungeonMap.MobileArray.Remove(this);

            // overwrite/extend for player
        }

        public int GetSkillRatingBySkill(string skill)
        {
            int rating = 0;

            for (int i = 0; i < this.skillArray.GetLength(0); i++)
            {
                if (this.skillArray[i, 0] == skill)
                {
                    rating = Convert.ToInt32(this.skillArray[i, 1]);
                    break;
                }
            }

            return rating;
        }

        public void AddSkillCheck(string skill)
        {
            for (int i = 0; i < this.skillArray.GetLength(0); i++)
            {
                if (this.skillArray[i, 0] == skill)
                {
                    this.skillArray[i, 2] = Convert.ToString(Convert.ToInt32(this.skillArray[i,2]) + 1);
                    if (Convert.ToInt32(this.skillArray[i,2]) ==  5)
                    {
                        this.skillArray[i, 2] = "0";
                        this.skillArray[i, 1] = Convert.ToString(Convert.ToInt32(this.skillArray[i, 1]) + 1);
                        Game.GAPI.RenderText($"{Game.GAPI.CapitaliseString(skill)} has risen to {skillArray[i,1]}!");
                    }

                    break;
                }
            }
        }
    }
}
