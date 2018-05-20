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

        public Map(int xSize = 10, int ySize = 10)
        {
            // We'll add some more creation parameters and variables later. Like depth, tileset, reference, etc.
            // The default is a 10x10 map, blank in the middle, with walls all around the edge.
            TileArray = new Tile[xSize - 1, ySize - 1];
            for (int i = 0; i <= TileArray.GetUpperBound(1); i++)
            {
                // Loop through the x-axis, adding walls on top and bottom.
                TileArray[i, 0].MakeTile("wall");
                TileArray[i, TileArray.GetUpperBound(2)].MakeTile("wall");
            }
            for (int i = 0; i <= TileArray.GetUpperBound(2); i++)
            {
                TileArray[0, i].MakeTile("wall");
                TileArray[TileArray.GetUpperBound(1), i].MakeTile("wall");
            }

        }
    }
}
