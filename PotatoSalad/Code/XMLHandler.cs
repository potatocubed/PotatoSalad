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
            saveData = dataDir + "/savedata.xml";
            if (!File.Exists(saveData))
            {
                CreateDataFile(saveData, "SaveData");
            }
        }

        public void CreateNewSaveData(string pcName, int iteration = 0)
        {
            // We're starting a new game.
            // If a savedir for that name already exists, append a number to the end.
            pcName = pcName.Replace(" ", "");
            saveDir = dataDir + "/" + pcName;
            if (!Directory.Exists(saveDir))
            {
                Directory.CreateDirectory(saveDir);
                CreateDataFile(saveDir + "/character.xml", "CharData");
            }
            else
            {
                iteration++;
                pcName = pcName + "-" + iteration.ToString();
                CreateNewSaveData(pcName, iteration);
            }
        }

        public void CreateNewLevelData()
        {
            // Creates save data for the current player, for the current map.
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

            // The mobile list and the map itself will be added later.

            //public Tile[,] TileArray;
            //public List<Mobile> MobileArray = new List<Mobile>();

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

            // The mobile list and the map itself will be added later.

            //public Tile[,] TileArray;
            //public List<Mobile> MobileArray = new List<Mobile>();

            mapxml.Save(mapDir);
        }

        public void UpdateCharData()
        {

        }
        
        private bool CreateDataFile(string fileName, string rootElementName)
        {
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
