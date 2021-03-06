﻿using System;
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

            // The mobile list will be added later.

            //public List<Mobile> MobileArray = new List<Mobile>();

        }

        public void UpdateGeography()
        {
            string mapDir = saveDir + "/data/" + Game.DungeonMap.MapID;
            string[] mText = Game.DungeonMap.MapText();
            File.WriteAllLines(mapDir + "/geography.txt", mText);
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

            // The mobile list will be added later.

            //public List<Mobile> MobileArray = new List<Mobile>();

            mapxml.Save(mapDir);
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

            xNode = xRoot.SelectSingleNode("Location");
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
