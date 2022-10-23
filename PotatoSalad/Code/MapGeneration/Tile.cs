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
        public string Name;
        public int X;
        public int Y;
        public bool BlockSight;
        public bool BlockEffect;
        public bool BlockMovement;
        public bool IsInFOV;
        public bool IsExplored;
        public string DisplayChar;
        public string TileGraphic;
        public string DarkTileGraphic;
        public Mobile Occupier;
        public string Blackout;
        public string Description;
        public string Usable;   // If blank, not usable.

        private static string TileDir = "../../Tiles/Graphics";
        private string TileXML = "../../Tiles/Tiles.xml";

        public Tile(int x, int y,
            bool bs = false, bool be = false, bool bm = false,
            bool fov = false, bool exp = false,
            string dc = "X", string graphic = "../../Tiles/Graphics/default.png",
            string dGraphic = "../../Tiles/Graphics/default.png", string us = "")
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
            DarkTileGraphic = dGraphic;
            Blackout = TileDir + "/black.png";
            Description = "A default tile.";
            Usable = us;
        }

        public void MakeTile(string tileType = "")
        {
            string[] t = Game.XMLHandler.LoadTile(tileType, TileXML);

            Name = t[0];
            if (t[1] == "yes")
            {
                BlockSight = true;
            }
            else
            {
                BlockSight = false;
            }
            if (t[2] == "yes")
            {
                BlockEffect = true;
            }
            else
            {
                BlockEffect = false;
            }
            if (t[3] == "yes")
            {
                BlockMovement = true;
            }
            else
            {
                BlockMovement = false;
            }
            DisplayChar = t[4];
            Description = t[5];
            Usable = t[6];

            TileGraphicSetting(tileType);
        }

        private void TileGraphicSetting(string baseTile)
        {
            TileGraphic = $"{TileDir}/{baseTile}.png";
            DarkTileGraphic = $"{TileDir}/{baseTile}_dark.png";
        }
    }
}
