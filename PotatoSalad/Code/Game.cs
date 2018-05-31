﻿using System;
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

        public static TheWorld WorldForm;
        public static ConsoleForm ConsoleForm;
        public static MainMenu MainMenu;

        public static XMLHandler XMLHandler;
        public static InputHandler InputHandler;
        public static StateMachine StateMachine;
        public static Globals Globals;
        public static Player Player;
        public static FOVCalculator FOVCalculator;
        public static GraphicsAPI GAPI;
        public static Dice Dice;
        public static List<Tile> TileList;

        public static XmlDocument PlayerXML;    // This allows us to edit the deets in real-time.
        public static XmlDocument LevelXML;     // Likewise, but for the world map.
        // These variables aren't filled until the game is started -- whether by newgame or loadgame.

        [STAThread]
        static void Main()
        {
            Dice = new Dice();
            Globals = new Globals();
            XMLHandler = new XMLHandler();
            InputHandler = new InputHandler();
            StateMachine = new StateMachine(Globals.STATE_PLAYER_TURN);
            FOVCalculator = new FOVCalculator();
            GAPI = new GraphicsAPI();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Game.MainMenu = new MainMenu();
            MainMenu.Show();

            Application.Run();

            // Starting screen, menu, etc.
            // Save/Load
            // Hold the XML docs open as necessary, for continuous writing.
            // Need to add auto-updates to the XML docs.
            // Turn-by-turn updates are applied to the open XML files.
            // Remember to auto-save when the app closes. -- The event's in place, just needs the code.
            // Procgen pantheons
            // Form layout.
            // Fix showforms.

            // (Apparently you can hijack the 'on close' event to just hide the windows. When all are hidden, kill the app.)
        }

        private static void InitialiseTileList()
        {
            // At some point I can initialise this from a separate data file.
            // All the data is currently stored in Tile.MakeTile.
            Tile t = new Tile(-1, -1);

            // floor
            t.MakeTile("floor");
            TileList.Add(t);

            // wall
            t.MakeTile("wall");
            TileList.Add(t);
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

        public static void LoadGame(string saveDir, string mapID)
        {
            // We need to load the map first.
            // Into DungeonMap and also LevelXML.
            // (Could do LevelXML first then have DungeonMap draw from that?)
            DungeonMap = new Map();
            LevelXML = new XmlDocument();
            LevelXML.Load(saveDir + "/data/" + mapID + "/mapdata.xml");
            // Call the as-yet-nonexistent DungeonMap.LoadMap method, which will load from the XML.

            // Then we load the player.
            PlayerXML = new XmlDocument();
            PlayerXML.Load(saveDir + "/character.xml");
            // Call the as-yet-nonexistent Player.LoadPlayer method, which will load from the XML.

            // Then we close this form and on with the show.
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
            XMLHandler.UpdateCharData();
            PlayerXML = new XmlDocument();
            PlayerXML.Load(XMLHandler.saveDir + "/character.xml");

            // And now we start the show.
            Game.ShowForms();
        }

        public static void ShowForms()
        {
            // This doesn't quite do what I want.
            // I just want to hide the forms instead of closing them.
            // The main control form is the bit that exists in the taskbar.

            // RIGHT NOW, the app exits when all windows are closed.
            // I think the desired outcome is that:
            // 1. the app exits when all non-main windows are closed. -- TICK
            // 2. the app exits when the main window is closed. -- TICK
            // 3. when the main window is SELECTED (e.g. from the taskbar) it brings all non-main windows to the front. -- TICK
            // 4. If all non-main windows are minimised, it doesn't bounce back to foreground. -- TICK
            // 5. When any form is minimised, all forms are minimised. -- TICK
            // 6. When all forms are minimised, if any form is restored then all are restored. -- TICK
            // 8. When all forms are minimised, clicking the TOF on the taskbar restores them all. -- TICK

            // 7. When any form is closed, all forms are closed. (???)
            // 9. Closing any of the sub-forms just minimises them.

            int formCount = 0;
            List<Form> formList = new List<Form>(); // The ControlForm doesn't live on this list.

            //Form1 worldForm = new Form1();
            WorldForm = new TheWorld();
            ConsoleForm = new ConsoleForm();
            
            formList.Add(WorldForm);
            formList.Add(ConsoleForm);

            // We fire up the control form last of all.
            TheOneForm ControlForm = new TheOneForm(WorldForm, ConsoleForm)
            {
                ShowInTaskbar = true
            };

            ControlForm.FormClosed += (sender, e) =>
            {
                Application.ExitThread();
            };

            ControlForm.Show();

            foreach (Form f in formList)
            {
                formCount++;
                f.ShowInTaskbar = false;
                f.KeyPreview = true;
                f.FormClosed += (sender, e) =>
                {
                    if (--formCount > 0)
                    {
                        return;
                    }

                    Application.ExitThread();
                };
                f.Resize += (sender, e) =>
                {
                    if (f.WindowState == FormWindowState.Minimized)
                    {
                        ControlForm.AllFormsMinimised = true;
                        foreach (Form f2 in formList)
                        {
                            f2.WindowState = FormWindowState.Minimized;
                        }
                        ControlForm.WindowState = FormWindowState.Minimized;
                    }
                    else if(f.WindowState == FormWindowState.Normal)
                    {
                        if (ControlForm.AllFormsMinimised)
                        {
                            ControlForm.AllFormsMinimised = false;
                            foreach(Form f2 in formList)
                            {
                                f2.WindowState = FormWindowState.Normal;
                            }
                            ControlForm.WindowState = FormWindowState.Normal;
                        }
                    }
                };
                f.KeyPress += (sender, e) =>
                {
                    InputHandler.KeyIn(e.KeyChar.ToString());
                };
                f.Show();
            }
        }
    }
}
