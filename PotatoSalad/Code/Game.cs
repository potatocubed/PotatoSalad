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
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            // Step 1. Work out how to display tiles on the form.
            // Okay.

            // Bresenham's Line Algorithm is key for determining FOV.
            // It's an octant thing.
            // It's the method used in Roguesharp, so I'll steal it.

            // A map is a collection of tiles.
            // Instantiate a 'mapgenerator' class, then call a function in that which returns a map.
            //MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight);
            //DungeonMap = mapGenerator.CreateMap();
        }
    }
}
