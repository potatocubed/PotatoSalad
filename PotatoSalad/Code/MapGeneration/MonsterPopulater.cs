using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void MonsterSetUp(List<Mobile> monList)
        {
            // TODO
            // This assigns properties to mon based on the string fed in monster.monType.
            // If no string is fed in, we get 'generic monster'.

            // monType is also going to match an identifying string in the XML.
            // For future reference.

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
