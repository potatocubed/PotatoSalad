using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Schema;
using System.Xml;

namespace PotatoSalad
{
    class MonsterPopulater
    {
        public string GenerateUniqueID(string name, List<Mobile> mobArray)
        {
            // Takes a mobile's name and appends an ever-increasing number to it
            // until the ID is unique in the current mobile array.
            int iteration = 0;
            string testName = name + "-" + iteration.ToString();

            List<string> idList = new List<string>();

            foreach (Mobile m in mobArray)      // TODO: There's got to be a more efficient method than this.
            {
                idList.Add(m.id);
            }

            while (idList.Contains(testName))
            {
                iteration++;
                testName = name + "-" + iteration.ToString();
            }
            return testName;
        }

        public void LoadMonster(ref Tile[,] tileArray, ref List<Mobile> monList, string nm, string id,
            string mtype, int locx, int locy, string g, int fov, int ai)
        {
            // TODO
            // This creates a monster, adds all the details (as snagged from the XML),
            // and then adds it to the map in the correct place.

            // This LOADS AN EXISTING MONSTER
            // DEPRECATED DO NOT USE

            Monster mon = new Monster(tileArray[locx, locy], id);
            mon.name = nm;
            mon.id = id;
            mon.monType = mtype;
            mon.displayGraphic = g;
            mon.FOVRange = fov;
            mon.AI_type = ai;
            monList.Add(mon);
            tileArray[locx, locy].Occupier = mon;

            /*
            Monster mon = new Monster(TileArray[targX, targY], uid);
            mon.monType = mType;
            MobileArray.Add(mon);
            TileArray[targX, targY].Occupier = mon;

            <name>Goblin</name>
            <id>goblin-0</id>
            <location x="48" y="2" />
            <graphic>../../Graphics/Mobiles/goblin.png</graphic>
            <fov>5</fov>
            <ai>1</ai>
            */
        }

        public void LoadMonsterXML(XmlElement xmlSnippet, ref Tile[,] tileArray, ref List<Mobile> monList)
        {
            // Get location first for instantiating.
            int locx;
            int locy;
            string id;

            XmlElement xElem;
            xElem = (XmlElement)xmlSnippet.SelectSingleNode("./location");
            locx = Convert.ToInt32(xElem.GetAttribute("x"));
            locy = Convert.ToInt32(xElem.GetAttribute("y"));
            xElem = (XmlElement)xmlSnippet.SelectSingleNode("./id");
            id = xElem.InnerText;

            Monster mon = new Monster(tileArray[locx, locy], id);

            xElem = (XmlElement)xmlSnippet.SelectSingleNode("./name");
            mon.name = xElem.InnerText;

            xElem = (XmlElement)xmlSnippet.SelectSingleNode("./montype");
            mon.monType = xElem.InnerText;

            xElem = (XmlElement)xmlSnippet.SelectSingleNode("./graphic");
            mon.displayGraphic = xElem.InnerText;

            xElem = (XmlElement)xmlSnippet.SelectSingleNode("./fov");
            mon.FOVRange = Convert.ToInt32(xElem.InnerText);

            xElem = (XmlElement)xmlSnippet.SelectSingleNode("./health");
            mon.health = Convert.ToInt32(xElem.InnerText);

            xElem = (XmlElement)xmlSnippet.SelectSingleNode("./mana");
            mon.mana = Convert.ToInt32(xElem.InnerText);

            xElem = (XmlElement)xmlSnippet.SelectSingleNode("./ai");
            mon.AI_type = Convert.ToInt32(xElem.InnerText);

            // Skills
            XmlNodeList xList;
            xList = xmlSnippet.SelectNodes("./Skills/child::*");
            int targRow = 0;
            foreach (XmlElement xm in xList)
            {
                mon.skillArray[targRow, 0] = xm.GetAttribute("name");
                mon.skillArray[targRow, 1] = xm.GetAttribute("rating");
                mon.skillArray[targRow, 2] = xm.GetAttribute("checks");
                targRow++;
            }

            monList.Add(mon);
            tileArray[locx, locy].Occupier = mon;
        }

        public void MonsterSetUp(List<Mobile> monList)
        {
            // TODO
            // This assigns properties to mon based on the string fed in monster.monType.
            // If no string is fed in, we get 'generic monster'.

            // monType is also going to match an identifying string in the XML.
            // For future reference.

            // This CREATES A NEW MONSTER

            foreach (Mobile mob in monList)
            {
                if (mob is Monster)
                {
                    Monster mon = (Monster)mob;
                    switch (mon.monType)
                    {
                        // TODO: Put all these details in an external file and draw from there.
                        case "goblin":
                            mon.name = "Goblin";
                            mon.displayGraphic = "../../Graphics/Mobiles/goblin.png";
                            mon.health = 4;
                            mon.mana = 4;
                            break;
                        default:
                            // If passed with no parameter then nothing happens.
                            break;
                    }
                }
            }
        }
    }
}
