using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace PotatoSalad
{
    class XMLHandler
    {
        // Contains methods for reading from and writing to XML.
        private string here = Directory.GetCurrentDirectory();
        private string dataDir;
        private string saveData;
        public string saveDir;  // This is the active game.

        public XMLHandler()
        {
            dataDir = here + "/data";
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }
            saveData = dataDir + "/savedata.xml";   // This bit may be unnecessary.
            if (!File.Exists(saveData))
            {
                CreateDataFile(saveData, "SaveData");
            }
        }

        public void CreateNewSaveData(string pcName, int iteration = 0)
        {
            // We're starting a new game.
            // If a savedir for that name already exists, append a number to the end.
            // MUST BE CALLED AFTER CHARACTER IS INITIALISED.
            pcName = pcName.Replace(" ", "");
            saveDir = dataDir + "/" + pcName;
            if (!Directory.Exists(saveDir))
            {
                Directory.CreateDirectory(saveDir);
                CreateDataFile(saveDir + "/character.xml", "CharData");
                //UpdateCharData();
                // This has to be called later, because mobs can't exist without a map to contain them...
                // ...but the map can't exist without the player directory to exist in.
                // This is Bad Design. But I don't feel like refactoring it right now.
            }
            else
            {
                iteration++;
                pcName = pcName + "-" + iteration.ToString();
                CreateNewSaveData(pcName, iteration);
            }
        }

        public string[] LoadTile(string tileType, string tileFile)
        {
            string[] output = new string[7];
            XmlDocument tiles = new XmlDocument();
            tiles.Load(tileFile);

            // Every part of this should exist; if not, it'll fall into a default value.
            XmlElement xRoot = tiles.DocumentElement;
            XmlNode xNode;
            XmlElement xElem;
            XmlAttribute xAtt;

            xNode = xRoot.SelectSingleNode($"./Tile[Name = \"{tileType}\"]");
            output[0] = tileType;

            xElem = (XmlElement)xNode.SelectSingleNode("./Block");
            xAtt = (XmlAttribute)xElem.SelectSingleNode("@sight");
            output[1] = xAtt.InnerText;
            xAtt = (XmlAttribute)xElem.SelectSingleNode("@effect");
            output[2] = xAtt.InnerText;
            xAtt = (XmlAttribute)xElem.SelectSingleNode("@movement");
            output[3] = xAtt.InnerText;

            xElem = (XmlElement)xNode.SelectSingleNode("./DisplayChar");
            output[4] = xElem.InnerText;
            xElem = (XmlElement)xNode.SelectSingleNode("./Description");
            output[5] = xElem.InnerText;
            xElem = (XmlElement)xNode.SelectSingleNode("./Usable");
            output[6] = xElem.InnerText;

            return output;
        }

        public void CreateNewLevelData()
        {
            // Creates save data for the current player, for the current map.
            // REQUIRES THE MAP TO BE GENERATED AND STORED IN DUNGEONMAP FIRST.
            // A few quick prelims first:

            string mapDir = saveDir + "/data/" + Game.DungeonMap.MapID;

            if (!Directory.Exists(saveDir + "/data"))
            {
                Directory.CreateDirectory(saveDir + "/data");
            }
            if (!Directory.Exists(mapDir))
            {
                Directory.CreateDirectory(mapDir);
            }
            CreateDataFile(mapDir + "/mapdata.xml", "MapData");
            UpdateMapData();

            string[] mText = Game.DungeonMap.MapText();
            File.WriteAllLines(mapDir + "/geography.txt", mText);
        }

        public void UpdateGeography()
        {
            string mapDir = saveDir + "/data/" + Game.DungeonMap.MapID;
            string[] mText = Game.DungeonMap.MapText();
            File.WriteAllLines(mapDir + "/geography.txt", mText);
        }

        public string PossibleExits(string currentMapID)
        {
            string s = "";
            XmlDocument exitsDoc = new XmlDocument();
            exitsDoc.Load(here + "../../../Code/MapGeneration/StairDestinations.xml");
            XmlElement xElem = (XmlElement)exitsDoc.DocumentElement.SelectSingleNode($"level[@id='{currentMapID}']");
            XmlNodeList xNodes = xElem.SelectNodes("exit");
            foreach(XmlNode n in xNodes)
            {
                if (s != "")
                {
                    s = $"{s}-";
                }
                s = $"{s}{n.SelectSingleNode("@id").InnerText}";
            }
            return s;
        }

        public List<string> PossibleEntrances(string mapID)
        {
            List<string> result = new List<string>();
            XmlDocument exitsDoc = new XmlDocument();
            exitsDoc.Load($"{saveDir}/data/{mapID}/mapdata.xml");
            XmlNodeList xNodes = exitsDoc.SelectNodes("//terrain/item[type='stairsdown']");

            foreach (XmlNode n in xNodes)
            {
                result.Add(n.SelectSingleNode("id").InnerText);
            }

            return result;
        }

        public void UpdateMapData()
        {
            string mapDir = saveDir + "/data/" + Game.DungeonMap.MapID + "/mapdata.xml";
            XmlDocument mapxml = new XmlDocument();
            mapxml.Load(mapDir);

            // Any given part of this may or may not already exist.
            XmlElement xRoot = mapxml.DocumentElement;
            XmlNode xNode;
            XmlElement xElem;

            xNode = xRoot.SelectSingleNode("./MapID");
            if (xNode == null)
            {
                xNode = mapxml.CreateElement("MapID");
                xRoot.AppendChild(xNode);
            }
            xNode.InnerText = Game.DungeonMap.MapID;

            xNode = xRoot.SelectSingleNode("./MapName");
            if (xNode == null)
            {
                xNode = mapxml.CreateElement("MapName");
                xRoot.AppendChild(xNode);
            }
            xNode.InnerText = Game.DungeonMap.MapName;

            xNode = xRoot.SelectSingleNode("./LevelNumber");
            if (xNode == null)
            {
                xNode = mapxml.CreateElement("LevelNumber");
                xRoot.AppendChild(xNode);
            }
            xNode.InnerText = Game.DungeonMap.LevelNumber;

            xNode = xRoot.SelectSingleNode("./Depth");
            if (xNode == null)
            {
                xNode = mapxml.CreateElement("Depth");
                xRoot.AppendChild(xNode);
            }
            xNode.InnerText = Game.DungeonMap.Depth;

            xNode = xRoot.SelectSingleNode("./Details");
            if (xNode == null)
            {
                xNode = mapxml.CreateElement("Details");
                xRoot.AppendChild(xNode);
            }
            xElem = (XmlElement)xNode;
            xElem.SetAttribute("x", (Game.DungeonMap.XDimension + 1).ToString());
            xElem.SetAttribute("y", (Game.DungeonMap.YDimension + 1).ToString());
            xElem.SetAttribute("maptype", Game.DungeonMap.mapType);

            // Since the geography of the level will change only rarely, I'll handle that
            // as and when it pops up. Using UpdateGeography().

            // This is where the call to update the mobile list will be.
            // It'll return an XML fragment which can be incorporated into the map file.
            // TODO: This whole thing is done as a string, which is kind of not great. But that's a fix for later.
            // We have to cut any pre-existing mobile list so it doesn't duplicate.
            xNode = xRoot.SelectSingleNode("./mobiles");
            if (!(xNode == null))
            {
                xRoot.RemoveChild(xNode);
            }
            XmlNode importNode = xRoot.OwnerDocument.ImportNode(UpdateMobileData(), true);
            xRoot.AppendChild(importNode);

            // Here's the terrain section like the mobiles section.
            xNode = xRoot.SelectSingleNode("./terrain");
            if (!(xNode == null))
            {
                xRoot.RemoveChild(xNode);
            }
            importNode = xRoot.OwnerDocument.ImportNode(UpdateTerrainData(), true);
            xRoot.AppendChild(importNode);

            // TODO: Once (if?) this starts getting uwieldy, I'll cut this bit and store the XML
            // in memory until the player exits, at which point it gets written to drive.
            mapxml.Save(mapDir);
        }

        XmlDocumentFragment UpdateTerrainData()
        {
            XmlDocumentFragment umd = new XmlDocument().CreateDocumentFragment();
            string s = "<terrain>";

            foreach(Tile t in Game.DungeonMap.TileArray)
            {
                string[] usable_split = t.Usable.Split('-');
                if (usable_split.Length != 0)
                {
                    try
                    {
                        if (usable_split[0] == "stairsdown")
                        {
                            s = $"{s}<item x=\"{t.X}\" y=\"{t.Y}\"><type>stairsdown</type><id>{usable_split[1]}-{usable_split[2]}</id></item>";
                        }
                    }
                    catch (Exception)
                    {
                        // Just skip it, try again in a moment.
                    }

                    try
                    {
                        if (usable_split[0] == "stairsup")
                        {
                            s = $"{s}<item x=\"{t.X}\" y=\"{t.Y}\"><type>stairsup</type><id>{usable_split[1]}-{usable_split[2]}</id></item>";
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            s = $"{s}</terrain>";

            umd.InnerXml = s;
            return umd;
        }

        XmlDocumentFragment UpdateMobileData()
        {
            XmlDocumentFragment umd = new XmlDocument().CreateDocumentFragment();
            string s = "<mobiles>";

            foreach(Mobile mob in Game.DungeonMap.MobileArray)
            {
                if(mob is Monster)
                {
                    // Call some sort of XML-generating function in the monster class.
                    string s2 = mob.GenerateSaveXML();
                    s = $"{s}{s2}";
                }
            }

            s = $"{s}</mobiles>";
            umd.InnerXml = s;
            return umd;
        }

        public void UpdateCharData()
        {
            // saveDir is already pointing to the right place.
            // THE PLAYER MUST BE INITIALISED BEFORE THIS IS CALLED.
            XmlDocument charxml = new XmlDocument();
            charxml.Load(saveDir + "/character.xml");

            // Any given part of this may or may not already exist.
            XmlElement xRoot = charxml.DocumentElement;
            XmlNode xNode;
            XmlElement xElem;

            xNode = xRoot.SelectSingleNode("./CharacterID");
            if (xNode == null)
            {
                xNode = charxml.CreateElement("CharacterID");
                xRoot.AppendChild(xNode);
            }
            xNode.InnerText = Game.Player.id;

            xNode = xRoot.SelectSingleNode("./Name");
            if (xNode == null)
            {
                xNode = charxml.CreateElement("Name");
                xRoot.AppendChild(xNode);
            }
            xNode.InnerText = Game.Player.name;

            xNode = xRoot.SelectSingleNode("./DisplayGraphic");
            if (xNode == null)
            {
                xNode = charxml.CreateElement("DisplayGraphic");
                xRoot.AppendChild(xNode);
            }
            xNode.InnerText = Game.Player.displayGraphic;

            // Stats
            xNode = xRoot.SelectSingleNode("./Stats");
            if (xNode == null)
            {
                xNode = charxml.CreateElement("Stats");
                xRoot.AppendChild(xNode);
            }
            xElem = (XmlElement)xNode;
            xNode = xElem.SelectSingleNode("./FOVRange");
            if (xNode == null)
            {
                xNode = charxml.CreateElement("FOVRange");
                xElem.AppendChild(xNode);
            }
            xNode.InnerText = Game.Player.FOVRange.ToString();
            xNode = xElem.SelectSingleNode("./Health");
            if (xNode == null)
            {
                xNode = charxml.CreateElement("Health");
                xElem.AppendChild(xNode);
            }
            xNode.InnerText = Game.Player.health.ToString();
            xNode = xElem.SelectSingleNode("./Mana");
            if (xNode == null)
            {
                xNode = charxml.CreateElement("Mana");
                xElem.AppendChild(xNode);
            }
            xNode.InnerText = Game.Player.mana.ToString();

            // Skills
            xNode = xRoot.SelectSingleNode("./Skills");
            if (xNode == null)
            {
                xNode = charxml.CreateElement("Skills");
                xRoot.AppendChild(xNode);
            }
            xElem = (XmlElement)xNode;
            for (int i = 0; i < Game.Player.skillArray.GetLength(0); i++)
            {
                xNode = xElem.SelectSingleNode($"./Skill[@name = '{Game.Player.skillArray[0,0]}']");
                if (xNode == null)
                {
                    XmlElement xElem2;
                    xElem2 = charxml.CreateElement("Skill");
                    xElem2.SetAttribute("name", Game.Player.skillArray[0, 0]);
                    xElem2.SetAttribute("rating", Game.Player.skillArray[0, 1]);
                    xElem2.SetAttribute("checks", Game.Player.skillArray[0, 2]);
                    xElem.AppendChild(xElem2);
                }
            }

            // Location
            xNode = xRoot.SelectSingleNode("./Location");
            if (xNode == null)
            {
                xNode = charxml.CreateElement("Location");
                xRoot.AppendChild(xNode);
            }
            xElem = (XmlElement)xNode;
            xElem.SetAttribute("mapID", Game.DungeonMap.MapID); // Got to note the level!
            xElem.SetAttribute("x", Game.Player.location.X.ToString());
            xElem.SetAttribute("y", Game.Player.location.Y.ToString());
            xElem.InnerText = Game.DungeonMap.MapName + ", Level " + Game.DungeonMap.LevelNumber;   // This is for easy display when loading the game again.

            charxml.Save(saveDir + "/character.xml");
        }

        public void UpdatePlayerLocation(XmlDocument charxml)
        {
            // Updating the real-time file.
            XmlElement xRoot = charxml.DocumentElement;
            XmlNode xNode;
            XmlElement xElem;

            xNode = xRoot.SelectSingleNode("Location");
            // The character XML should be complete, so this error should never fire.
            // But if it does... we're just not going to record the location. Game's going to be fucked anyway.
            if (xNode == null)
            {
                // Do nothing.
                return;
            }
            xElem = (XmlElement)xNode;
            xElem.SetAttribute("x", Game.Player.location.X.ToString());
            xElem.SetAttribute("y", Game.Player.location.Y.ToString());
        }
        
        private bool CreateDataFile(string fileName, string rootElementName)
        {
            // Creates a blank XML doc with the specified root.

            XmlDocument doc = new XmlDocument();
            XmlElement xElem;

            // Takes complete path.
            if (File.Exists(fileName))
            {
                // It already exists.
                return false;
            }

            xElem = doc.CreateElement(rootElementName);
            doc.AppendChild(xElem);
            doc.Save(fileName);

            return true;
        }
    }
}
