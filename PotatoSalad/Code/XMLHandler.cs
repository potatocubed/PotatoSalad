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
                Game.PlayerXML = new XmlDocument();
                Game.PlayerXML.Load(saveDir + "/character.xml");
                Game.LevelXML = new XmlDocument();
                
                // So here's the thing: The level is generated BEFORE this event fires, but should probably go after.
                // That way you can store the level data right there in the XML.

                // I think I need to work out what the game structure is going to be before I do much more on this.
            }
            else
            {
                iteration++;
                pcName = pcName + "-" + iteration.ToString();
                CreateNewSaveData(pcName, iteration);
            }
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
