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
            // If a savedir for that name already exists, append a number to the end.
            pcName = pcName.Replace(" ", "");
            string saveDir = dataDir + "/" + pcName;
            if (!Directory.Exists(saveDir))
            {
                Directory.CreateDirectory(saveDir);
                CreateDataFile(saveDir + "/character.xml", "CharData");
                Game.Player.SaveDataFile = saveDir;
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
