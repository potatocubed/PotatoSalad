using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotatoSalad
{
    public class Tile
    {
        // A class (object?) which holds all the information for any given map tile.
        public int X;
        public int Y;
        public bool BlockSight;
        public bool BlockEffect;
        public bool BlockMovement;
        public bool IsInFOV;
        public bool IsExplored;
        public string DisplayChar;
        public string TileGraphic;

        private string TileDir = "../../Graphics/Tiles";

        public Tile(int x, int y,
            bool bs = false, bool be = false, bool bm = false,
            bool fov = false, bool exp = false,
            string dc = "X", string graphic = "../../Graphics/Tiles/default.png")
        {
            // The default tile is blank empty space, unexplored and currently unseen.
            X = x;
            Y = y;
            BlockSight = bs;
            BlockEffect = be;
            BlockMovement = bm;
            IsInFOV = fov;
            IsExplored = exp;
            DisplayChar = dc;
            TileGraphic = graphic;
        }

        public void MakeTile(string tileType = "")
        {
            switch (tileType)
            {
                case "floor":
                    BlockSight = false;
                    BlockEffect = false;
                    BlockMovement = false;
                    DisplayChar = ".";
                    TileGraphic = $"{TileDir}/floor.png";
                    break;
                case "wall":
                    BlockSight = true;
                    BlockEffect = true;
                    BlockMovement = true;
                    DisplayChar = "#";
                    TileGraphic = $"{TileDir}/wall.png";
                    break;
                default:
                    // If passed with no parameter then nothing happens.
                    break;
            }
        }
    }
}
