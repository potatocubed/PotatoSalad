﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PotatoSalad.Windows
{
    public partial class MainForm : Form
    {
        private Size TileSize;
        private int apertureCount;  // Window width AND height in tiles.
        private List<Tile> oldFOV;
        private Bitmap MapImage;
        private Graphics MapDrawer;
        public ListBox consoleOutput;
        public TextBox informationBox;

        public MainForm()
        {
            InitializeComponent();
            consoleOutput = this.consoleListBox;
            informationBox = this.infoBox;

            TileSize = new Size(32, 32);
            apertureCount = 15;

            oldFOV = new List<Tile>();

            MapImage = new Bitmap((Game.DungeonMap.XDimension + 1) * 32, (Game.DungeonMap.YDimension + 1) * 32);
            MapDrawer = Graphics.FromImage(MapImage);
            MapDrawer.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            MapDrawer.FillRectangle(Brushes.Black, 0, 0, MapImage.Width, MapImage.Height);

            DrawMapOnLoad(Game.DungeonMap);
            InitialJumpToPlayer();
        }

        private void worldMapPanel_Paint(object sender, PaintEventArgs e)
        {
            //DrawMap(Game.DungeonMap);
            //MessageBox.Show("Paint Event!");
            this.worldMapPanel.Invalidate();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // I stole this from Stack Overflow to remove the flickering problem.
            // I have no idea how it works, but it does.
            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null,
                worldMapPanel,
                new object[] { true });

            int GubbinsHeight = this.Height - this.ClientSize.Height;
            int GubbinsWidth = this.Width - this.ClientSize.Width;
            this.MinimumSize = new Size(96 + GubbinsWidth, 122 + GubbinsHeight);
        }

        public void DrawMapOnLoad(Map m)
        {
            foreach (Tile t in m.TileArray)
            {
                if (t.IsExplored)
                {
                    MapDrawer.DrawImage(Image.FromFile(t.DarkTileGraphic), t.X * 32, t.Y * 32);
                }
            }
        }

        public void CursorDrawMap(Map m, PotatoSalad.Code.Cursor c)
        {
            List<Tile> fovList = new List<Tile>();
            fovList.Add(Game.SaladCursor.location);
            if (Game.SaladCursor.location != Game.SaladCursor.previousLocation)
            {
                fovList.Add(Game.SaladCursor.previousLocation);
            }

            Panel p = this.worldMapPanel;
            Graphics g = p.CreateGraphics();
            Bitmap bmp;

            foreach (Tile t in fovList)
            {
                if (t.IsInFOV)
                {
                    MapDrawer.DrawImage(Image.FromFile(t.TileGraphic), t.X * 32, t.Y * 32);
                    if (t.Occupier != null)
                    {
                        MapDrawer.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                        MapDrawer.DrawImage(Image.FromFile(t.Occupier.displayGraphic), t.X * 32, t.Y * 32);
                        MapDrawer.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                    }
                }
                else if (t.IsExplored)
                {
                    MapDrawer.DrawImage(Image.FromFile(t.DarkTileGraphic), t.X * 32, t.Y * 32);
                }
                else
                {
                    MapDrawer.DrawImage(Image.FromFile(t.Blackout), t.X * 32, t.Y * 32);
                    //MapDrawer.FillRectangle(Brushes.Black, t.X * 32, t.Y * 32, 32, 32);
                }

                MapDrawer.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                MapDrawer.DrawImage(Image.FromFile(Game.SaladCursor.graphic), Game.SaladCursor.X() * 32, Game.SaladCursor.Y() * 32, 32, 32);
                MapDrawer.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            }

            // Whenever we draw the map, we take a 480 x 480 slice from map image with the player
            // at the centre, and draw that as the background image.

            int sideMeasure = apertureCount * TileSize.Width;  // Default 480 pixels for a 15x15 tile layout at 32x32 size.
            Size apertureSize = new Size(sideMeasure, sideMeasure);
            int offset = (sideMeasure - TileSize.Width) / 2;
            Point origin = new Point((Game.SaladCursor.X() * 32) - offset, (Game.SaladCursor.Y() * 32) - offset);
            if (origin.X < 0)
            {
                origin.X = 0;
            }
            if (origin.Y < 0)
            {
                origin.Y = 0;
            }
            if (origin.X + apertureSize.Width > MapImage.Width)
            {
                origin.X = MapImage.Width - apertureSize.Width;
            }
            if (origin.Y + apertureSize.Height > MapImage.Height)
            {
                origin.Y = MapImage.Height - apertureSize.Height;
            }
            // It turns out that clone breaks if the rectangle is outside the image bounds.
            Rectangle cropper = new Rectangle(origin, apertureSize);
            bmp = (Bitmap)MapImage.Clone(cropper, MapImage.PixelFormat);
            worldMapPanel.BackgroundImage = bmp;
        }

        public void DrawMap(Map m)
        {
            List<Tile> newFOV = Game.FOVCalculator.ReturnFOVList(
                m.TileArray,
                Game.Player.X(),
                Game.Player.Y(),
                Game.Player.FOVRange);
            List<Tile> fovList = oldFOV.Union(newFOV).ToList();
            List<Tile> mobLocs = new List<Tile>();
            
            foreach (Mobile mob in Game.DungeonMap.MobileArray)
            {
                mobLocs.Add(mob.location);
            }

            List<Tile> newTiles = newFOV.Except(oldFOV).ToList();
            List<Tile> oldTiles = oldFOV.Except(newFOV).ToList();
            List<Tile> mobTiles = newFOV.Intersect(mobLocs).ToList();

            // Draw the map.
            // Okay, so, all is black by default.
            // If you can see it, paint it on the map.
            // Then arrange all the mob pictureboxes in the right places.

            //Panel p = this.worldMapPanel;
            //Graphics g = p.CreateGraphics();
            Bitmap bmp;

            foreach (Tile t in oldTiles)
            {
                t.IsInFOV = false;
                MapDrawer.DrawImage(Image.FromFile(t.DarkTileGraphic), t.X * 32, t.Y * 32);
            }

            foreach (Tile t in newFOV)
            {
                t.IsExplored = true;
                t.IsInFOV = true;
                MapDrawer.DrawImage(Image.FromFile(t.TileGraphic), t.X * 32, t.Y * 32);
                /*
                if (t.Occupier != null)
                {
                    MapDrawer.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                    MapDrawer.DrawImage(Image.FromFile(t.Occupier.displayGraphic), t.X * 32, t.Y * 32);
                    MapDrawer.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                }
                */
            }

            foreach (Tile t in mobTiles)
            {
                // Since it's an intersection with newFOV I know it's already in field of view.
                if (t.Occupier != null)
                {
                    // This should never be null, but just in case...
                    MapDrawer.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                    MapDrawer.DrawImage(Image.FromFile(t.Occupier.displayGraphic), t.X * 32, t.Y * 32);
                    MapDrawer.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                }
            }

            // TODO: Replace newFOV up there with newTiles, and find a better way of drawing and removing monsters.

            oldFOV = newFOV;

            // Whenever we draw the map, we take a 480 x 480 slice from map image with the player
            // at the centre, and draw that as the background image.

            int sideMeasure = apertureCount * TileSize.Width;  // Default 480 pixels for a 15x15 tile layout at 32x32 size.
            Size apertureSize = new Size(sideMeasure, sideMeasure);
            int offset = (sideMeasure - TileSize.Width) / 2;
            Point origin = new Point((Game.Player.X() * 32) - offset, (Game.Player.Y() * 32) - offset);
            if (origin.X < 0)
            {
                origin.X = 0;
            }
            if (origin.Y < 0)
            {
                origin.Y = 0;
            }
            if (origin.X + apertureSize.Width > MapImage.Width)
            {
                origin.X = MapImage.Width - apertureSize.Width;
            }
            if (origin.Y + apertureSize.Height > MapImage.Height)
            {
                origin.Y = MapImage.Height - apertureSize.Height;
            }
            // It turns out that clone breaks if the rectangle is outside the image bounds.
            Rectangle cropper = new Rectangle(origin, apertureSize);
            bmp = (Bitmap)MapImage.Clone(cropper, MapImage.PixelFormat);
            worldMapPanel.BackgroundImage = bmp;
        }

        public void InitialJumpToPlayer()
        {
            DrawMap(Game.DungeonMap);
        }

        public void EraseMob(int x, int y, Map m)
        {
            // Repaints a single tile with the basic texture, overwriting any mobs (OR ITEMS) there.
            // UNUSED but I'll keep it in case it becomes handy.
            Tile t = m.TileArray[x, y];

            if (t.IsInFOV)
            {
                MapDrawer.DrawImage(Image.FromFile(t.TileGraphic), t.X * 32, t.Y * 32);
            }
            else
            {
                MapDrawer.DrawImage(Image.FromFile(t.DarkTileGraphic), t.X * 32, t.Y * 32);
            }
            worldMapPanel.Refresh();
        }
    }
}
