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
        public bool Usable;

        private string TileDir = "../../Graphics/Tiles";

        public Tile(int x, int y,
            bool bs = false, bool be = false, bool bm = false,
            bool fov = false, bool exp = false,
            string dc = "X", string graphic = "../../Graphics/Tiles/default.png",
            string dGraphic = "../../Graphics/Tiles/default.png", bool us = false)
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
            switch (tileType)
            {
                // TODO: Put all these details in an external file and draw from there.
                case "floor":
                    Name = "floor";
                    BlockSight = false;
                    BlockEffect = false;
                    BlockMovement = false;
                    DisplayChar = ".";
                    Description = "Plain dungeon flooring.";
                    TileGraphicSetting("floor");
                    Usable = false;
                    break;
                case "wall":
                    Name = "wall";
                    BlockSight = true;
                    BlockEffect = true;
                    BlockMovement = true;
                    DisplayChar = "#";
                    Description = "An unremarkable dungeon wall.";
                    TileGraphicSetting("wall");
                    Usable = false;
                    break;
                default:
                    // If passed with no parameter then nothing happens.
                    break;
            }
        }

        private void TileGraphicSetting(string baseTile)
        {
            TileGraphic = $"{TileDir}/{baseTile}.png";
            DarkTileGraphic = $"{TileDir}/{baseTile}_dark.png";
        }
    }
}
