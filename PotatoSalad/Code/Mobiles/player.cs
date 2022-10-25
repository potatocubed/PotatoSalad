using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;

namespace PotatoSalad
{
    class Player : Mobile
    {
        //public string SaveDataFile;

        public Player(Tile loc)
            : base(loc, "UniqueIDPlayer")
        {
            name = "Player";
            displayGraphic = "../../Graphics/Mobiles/player.png";
            unique = true;
        }

        public void LoadPlayer(string n, string id, string dg, int fovr)
        {
            // DEPRECATED, DO NOT USE

            name = n;
            this.id = id;
            displayGraphic = dg;
            FOVRange = fovr;

            // need to get health, mana, skills
        }

        public void LoadPlayerXML(XmlNode xmlSnippet)
        {
            // IMPORTANT: Does not load location, only stats.
            // Which is GOOD.

            XmlElement xElem;
            xElem = (XmlElement)xmlSnippet.SelectSingleNode("./CharacterID");
            this.id = xElem.InnerText;
            xElem = (XmlElement)xmlSnippet.SelectSingleNode("./Name");
            this.name = xElem.InnerText;
            xElem = (XmlElement)xmlSnippet.SelectSingleNode("./DisplayGraphic");
            this.displayGraphic = xElem.InnerText;

            // The player is always allied with themselves.
            this.faction = "player";

            this.description = "It's you."; //TODO -- make this reflect inventory.

            xElem = (XmlElement)xmlSnippet.SelectSingleNode("./Stats/FOVRange");
            this.FOVRange = Convert.ToInt32(xElem.InnerText);
            xElem = (XmlElement)xmlSnippet.SelectSingleNode("./Stats/Health");
            this.health = Convert.ToInt32(xElem.InnerText);
            xElem = (XmlElement)xmlSnippet.SelectSingleNode("./Stats/Mana");
            this.mana = Convert.ToInt32(xElem.InnerText);

            XmlNodeList xList;
            xList = xmlSnippet.SelectNodes("./Skills/child::*");
            int targRow = 0;
            foreach(XmlElement xm in xList)
            {
                this.skillArray[targRow, 0] = xm.GetAttribute("name");
                this.skillArray[targRow, 1] = xm.GetAttribute("rating");
                this.skillArray[targRow, 2] = xm.GetAttribute("checks");
                targRow++;
            }

        }

        public override void KillSelf()
        {
            // Doesn't actually do anything yet

            // send message
            Game.GAPI.RenderText($"{Game.GAPI.CapitaliseString("this.name")} dies!");

            // remove from map
            //location.Occupier = null;
            //this.location = null;

            // remove from mobilelist
            //Game.DungeonMap.MobileArray.Remove(this);

            // overwrite/extend for player
        }

        public bool UseTerrain(Tile t)
        {
            if (t.Usable == "")
            {
                Game.GAPI.RenderText("There is nothing to use here.");
                return false;
            }

            switch (t.Name)
            {
                case "stairsdown":
                case "stairsup":
                    // Need to determine where it's going from the stair.
                    string destination = this.location.Usable.Split('-')[1];
                    // And then pick up the various details from a big XML
                    // of dungeon levels.
                    string[] goHere = Game.XMLHandler.GetLevelDetails(destination);
                    Game.EnterNewLevel(
                        goHere[0],
                        goHere[1],
                        Convert.ToInt32(goHere[2]),
                        Convert.ToInt32(goHere[3]),
                        Convert.ToInt32(goHere[4]),
                        Convert.ToInt32(goHere[5]),
                        goHere[6]);
                    break;
                default:
                    break;
            }

            return true;
        }
    }
}
