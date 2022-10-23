﻿using PotatoSalad.Code;
using PotatoSalad.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace PotatoSalad
{
    static class Game
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        public static Map DungeonMap;

        //public static TheWorld WorldForm;
        //public static ConsoleForm ConsoleForm;
        //public static CursorInfoForm CursorInfoForm;
        public static MainMenu MainMenu;
        public static MainForm MainForm;

        public static AIHandler AIHandler;
        public static XMLHandler XMLHandler;
        public static InputHandler InputHandler;
        public static Violence ViolenceHandler;
        public static StateMachine StateMachine;
        public static Globals Globals;
        public static Player Player;
        public static FOVCalculator FOVCalculator;
        public static SaladGraphics GAPI;
        public static Dice Dice;
        public static List<Tile> TileList;
        public static MonsterPopulater MonPop;
        public static PotatoSalad.Code.Cursor SaladCursor;

        public static XmlDocument PlayerXML;    // This allows us to edit the deets in real-time.
        public static XmlDocument LevelXML;     // Likewise, but for the world map.
        // These variables aren't filled until the game is started -- whether by newgame or loadgame.
        // TODO: Are these variables even necessary? Saving the game doesn't use them...

        public static Item EmptyHand;   // Just creating it once to work as a reference.

        [STAThread]
        static void Main()
        {
            Dice = new Dice();
            Globals = new Globals();
            XMLHandler = new XMLHandler();
            InputHandler = new InputHandler();
            ViolenceHandler = new Violence();
            AIHandler = new AIHandler();
            StateMachine = new StateMachine(Globals.STATE_MAIN_MENU);
            FOVCalculator = new FOVCalculator();
            GAPI = new SaladGraphics();
            TileList = new List<Tile>();
            InitialiseTileList();
            MonPop = new MonsterPopulater();

            EmptyHand = new Item();
            EmptyHand.name = "empty hand";
            EmptyHand.type = "weapon-unarmed";
            EmptyHand.damage = "1d2-blunt";

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Game.MainMenu = new MainMenu();
            MainMenu.Show();

            Application.Run();

            // Starting screen, menu, etc.
            // Then ranged attacks.
            // XML files for monster details.
            // Save scripts in XMLHandler will need to be updated as I add new odds and ends.
        }

        private static void InitialiseTileList()
        {
            // This is part of a terrible, terrible loop that will need to be fixed eventually.

            Tile t = new Tile(-1, -1);
            string TileXML = "../../Tiles/Tiles.xml";

            XmlDocument tiles = new XmlDocument();
            tiles.Load(TileXML);
            XmlNodeList xNodes = tiles.SelectNodes("//Tile");

            foreach(XmlNode n in xNodes)
            {
                t.MakeTile(n.SelectSingleNode("Name").InnerText);
                TileList.Add(t);
            }
        }

        public static string GetTileTypeFromChar(char c)
        {
            foreach(Tile t in TileList)
            {
                if (t.DisplayChar == c.ToString())
                {
                    return t.Name;
                }
            }
            // Not in the list???
            return "floor";
        }

        public static void SaveGame()
        {
            XMLHandler.UpdateCharData();
            XMLHandler.UpdateMapData();
            XMLHandler.UpdateGeography();
        }

        public static void LoadGame(string saveDir, string mapID)
        {
            // TODO: We need to check the file you're about to
            // load is valid in all ways otherwise this risks
            // some crashes due to unhandled errors.

            // We need to load the map first.
            // LevelXML first then have DungeonMap draw from that.
            DungeonMap = new Map();
            LevelXML = new XmlDocument();
            LevelXML.Load(saveDir + "/data/" + mapID + "/mapdata.xml");
            
            // Call DungeonMap.LoadMap method, which will load from the XML.
            XmlElement xElem;
            xElem = (XmlElement)LevelXML.SelectSingleNode("/MapData/MapName");
            string mn = xElem.InnerText;
            string mid = mapID;
            xElem = (XmlElement)LevelXML.SelectSingleNode("/MapData/LevelNumber");
            int ln = Convert.ToInt32(xElem.InnerText);
            xElem = (XmlElement)LevelXML.SelectSingleNode("/MapData/Depth");
            int d = Convert.ToInt32(xElem.InnerText);
            xElem = (XmlElement)LevelXML.SelectSingleNode("/MapData/Details");
            int xSize = Convert.ToInt32(xElem.GetAttribute("x"));
            int ySize = Convert.ToInt32(xElem.GetAttribute("y"));
            string mType = xElem.GetAttribute("maptype");
            DungeonMap.LoadMap(mn, mid, ln, d, xSize, ySize, mType);

            // Then we load the player.
            PlayerXML = new XmlDocument();
            PlayerXML.Load(saveDir + "/character.xml");
            
            // Find the player location and call DungeonMap.InstantiatePlayer there.
            xElem = (XmlElement)PlayerXML.SelectSingleNode("/CharData/Location");
            xSize = Convert.ToInt32(xElem.GetAttribute("x"));
            ySize = Convert.ToInt32(xElem.GetAttribute("y"));
            DungeonMap.InstantiatePlayer(DungeonMap.TileArray[xSize, ySize]);

            // Call Player.LoadPlayer, which will load from the XML.
            xElem = (XmlElement)PlayerXML.SelectSingleNode("/CharData");
            Player.LoadPlayerXML(xElem);

            // Now we want to load all the mobiles.
            XmlNodeList nodes = LevelXML.SelectNodes("//monster");
            foreach (XmlElement n in nodes)
            {
                Game.MonPop.LoadMonsterXML(n, ref DungeonMap.TileArray, ref DungeonMap.MobileArray);
            }

            // Then we close the main menu and on with the show.
            Game.ShowForms2();
        }


        public static void NewGame(string playerName)
        {
            // This sets XMLHandler.saveDir to the current save directory.
            XMLHandler.CreateNewSaveData(playerName);
            
            // Sort the dungeon data, and in the process initialise the player.
            DungeonMap = new Map();
            DungeonMap.Generate("Dungeon", "D1", 1, 1, 80, 25, "dungeon");
            XMLHandler.CreateNewLevelData();
            LevelXML = new XmlDocument();
            LevelXML.Load(XMLHandler.saveDir + "/data/" + Game.DungeonMap.MapID + "/mapdata.xml");

            // Sort the player data.
            Game.Player.name = playerName;
            Player.health = 5;
            Player.mana = 5;
            XMLHandler.UpdateCharData();
            PlayerXML = new XmlDocument();
            PlayerXML.Load(XMLHandler.saveDir + "/character.xml");

            // And now we start the show.
            Game.ShowForms2();
        }

        private static void ShowForms2()
        {
            // This should just open MainForm, I guess?
            StateMachine.SetState(Globals.STATE_WORKING);
            MainForm = new MainForm();
            MainForm.FormClosed += (sender, e) =>
            {
                Application.ExitThread();
            };
            MainForm.KeyPreview = true;
            MainForm.KeyPress += (sender, e) =>
            {
                InputHandler.KeyIn(e.KeyChar);
            };
            MainForm.Show();
            MainMenu.Close();
            StateMachine.SetState(Globals.STATE_PLAYER_TURN);
        }
    }
}
