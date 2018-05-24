using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotatoSalad
{
    public class Map
    {
        // A map is a collection of tiles.
        public int XDimension;
        public int YDimension;
        public Tile[,] TileArray;
        public List<Mobile> MobileArray = new List<Mobile>();
        public string MapID;    // Includes name, depth, etc.

        public void Generate(int xSize = 10, int ySize = 10)
        {
            XDimension = xSize - 1;
            YDimension = ySize - 1;

            // We'll add some more creation parameters and variables later. Like depth, tileset, reference, etc.
            // The default is a 10x10 map, blank in the middle, with walls all around the edge.
            TileArray = new Tile[xSize, ySize];
            for (int i = 0; i <= TileArray.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= TileArray.GetUpperBound(1); j++)
                {
                    TileArray[i, j] = new Tile(i, j);
                    TileArray[i, j].MakeTile("floor");
                }
            }

            for (int i = 0; i <= TileArray.GetUpperBound(0); i++)
            {
                // Loop through the x-axis, adding walls on top and bottom.
                TileArray[i, 0].MakeTile("wall");
                TileArray[i, TileArray.GetUpperBound(1)].MakeTile("wall");
            }
            for (int i = 0; i <= TileArray.GetUpperBound(1); i++)
            {
                TileArray[0, i].MakeTile("wall");
                TileArray[TileArray.GetUpperBound(0), i].MakeTile("wall");
            }

            // Now we've got some terrain, let's add some beasts.

            // For the default map, lets stick the player just inside the top-left corner.
            InstantiatePlayer(TileArray[2, 2]);
        }

        public void InstantiatePlayer(Tile loc)
        {
            Game.Player = new Player(loc);
            loc.Occupier = Game.Player;
            MobileArray.Add(Game.Player);
        }
    }
}
