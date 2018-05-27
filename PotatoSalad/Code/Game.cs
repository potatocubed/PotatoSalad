using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        public static InputHandler InputHandler;
        public static StateMachine StateMachine;
        public static Globals Globals;
        public static Player Player;
        public static FOVCalculator FOVCalculator;
        public static GraphicsAPI GAPI;
        public static Dice Dice;

        [STAThread]
        static void Main()
        {
            Dice = new Dice();
            Globals = new Globals();
            InputHandler = new InputHandler();
            StateMachine = new StateMachine(Globals.STATE_PLAYER_TURN);
            FOVCalculator = new FOVCalculator();
            GAPI = new GraphicsAPI();

            DungeonMap = new Map();
            DungeonMap.Generate(80, 25, "dungeon");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ShowForms();
            Application.Run();

            // Implement map scrolling. -- I've done it, but holy BALLS it's slow.
            // I think 2000 pictureboxes is probably too many.
            // System.Drawing is apparently the way to go??
            // Save/Load
            // Procgen pantheons
            // Starting screen, menu, etc.
            // Form layout.

            // (Apparently you can hijack the 'on close' event to just hide the windows. When all are hidden, kill the app.)
        }

        static void ShowForms()
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
