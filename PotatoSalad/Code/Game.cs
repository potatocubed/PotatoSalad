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

        [STAThread]
        static void Main()
        {
            DungeonMap = new Map();
            DungeonMap.Generate(10, 10);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            // Work out how to display tiles on the form.

            // Bresenham's Line Algorithm is key for determining FOV.
            // It's an octant thing.
            // It's the method used in Roguesharp, so I'll steal it.
        }
    }
}
