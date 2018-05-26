using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PotatoSalad
{
    public partial class TheWorld : Form
    {
        //public List<PictureBox> MobileList = new List<PictureBox>();
        private Size TileSize;
        private List<Tile> oldFOV;
        private Bitmap MapImage;
        private Graphics MapDrawer;

        private Control TopLeftControl;
        private Control BottomRightControl;

        public TheWorld()
        {
            InitializeComponent();
            this.Text = "The World";    //Sets the window title.

            oldFOV = new List<Tile>();
            TileSize = new Size(32, 32);
            //InitialiseMobileList(Game.DungeonMap);
            MapImage = new Bitmap(this.WorldMapPanel.Width, this.WorldMapPanel.Height);
            MapDrawer = Graphics.FromImage(MapImage);
            MapDrawer.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            MapDrawer.FillRectangle(Brushes.Black, 0, 0, MapImage.Width, MapImage.Height);

            TopLeftControl = new Control();
            BottomRightControl = new Control();

            TopLeftControl.Size = new Size(1, 1);
            TopLeftControl.Location = new Point(0, 0);
            BottomRightControl.Size = new Size(1, 1);
            BottomRightControl.Location = new Point(this.ClientSize.Width, this.ClientSize.Height);
            this.Controls.Add(TopLeftControl);
            this.Controls.Add(BottomRightControl);

            InitialJumpToPlayer();
        }

        //private void InitialiseMobileList(Map m)
        //{
        //    foreach (Mobile mob in m.MobileArray)
        //    {
        //        PictureBox pb = new PictureBox();
        //        MobileList.Add(pb);
        //        pb.Tag = mob.id;
        //        pb.Size = TileSize;
        //        pb.Image = Image.FromFile(mob.displayGraphic);
        //        pb.BackColor = Color.Transparent;
        //        pb.Location = new Point(-32, -32);  // Just off-screen?
        //    }
        //}

        private void WorldForm_Paint(object sender, PaintEventArgs e)
        {
            //DrawMap(Game.DungeonMap);
            //MessageBox.Show("Paint Event!");
            this.WorldMapPanel.Invalidate();
        }

        private void WorldForm_Load(object sender, EventArgs e)
        {
            // I stole this from Stack Overflow to remove the flickering problem.
            // I have no idea how it works, but it does.
            typeof(Panel).InvokeMember("DoubleBuffered", 
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic, 
                null, 
                WorldMapPanel, 
                new object[] { true });

            int GubbinsHeight = this.Height - this.ClientSize.Height;
            int GubbinsWidth = this.Width - this.ClientSize.Width;
            this.MinimumSize = new Size(96 + GubbinsWidth, 122 + GubbinsHeight);
            
        }

        public void DisplayDeath()
        {
            // A placeholder so that killing mobs can be reflected in the display.
            // Basically just dak the picture box with the matching tag.
        }

        public void DisplayAddMob()
        {
            // Another placeholder. For mid-level summons, spawns, etc.
        }

        public void DrawMap(Map m)
        {
            List<Tile> newFOV = Game.FOVCalculator.ReturnFOVList(
                m.TileArray,
                Game.Player.X(),
                Game.Player.Y(),
                Game.Player.FOVRange);
            List<Tile> fovList = oldFOV.Union(newFOV).ToList();

            // Draw the map.
            // Okay, so, all is black by default.
            // If you can see it, paint it on the map.
            // Then arrange all the mob pictureboxes in the right places.

            Panel p = this.WorldMapPanel;
            Graphics g = p.CreateGraphics();
            //Bitmap bmp;

            foreach (Tile t in oldFOV)
            {
                t.IsInFOV = false;
            }

            foreach (Tile t in newFOV)
            {
                t.IsExplored = true;
                t.IsInFOV = true;
            }

            foreach (Tile t in fovList)
            {
                if (t.IsInFOV)
                {
                    MapDrawer.DrawImage(Image.FromFile(t.TileGraphic), t.X * 32, t.Y * 32);
                }
                else
                {
                    MapDrawer.DrawImage(Image.FromFile(t.DarkTileGraphic), t.X * 32, t.Y * 32);
                }

                if (t.Occupier != null)
                {
                    MapDrawer.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                    MapDrawer.DrawImage(Image.FromFile(t.Occupier.displayGraphic), t.X * 32, t.Y * 32);
                    MapDrawer.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                }
            }
            oldFOV = newFOV;
            this.WorldMapPanel.BackgroundImage = MapImage;
        }

        private void TheWorld_Resize(object sender, EventArgs e)
        {
            // This is going to need adjustment.
        }

        public void ScrollWorld(int origX, int origY, int targX, int targY)
        {
            // Moves the window to centre on targx/targy.
            int dx = targX - origX;
            int dy = targY - origY;

            Point p = new Point(TopLeftControl.Location.X + (dx * 32), TopLeftControl.Location.Y + (dy * 32));
            TopLeftControl.Location = p;
            p.X = BottomRightControl.Location.X + (dx * 32);
            p.Y = BottomRightControl.Location.Y + (dy * 32);
            BottomRightControl.Location = p;

            if (dx < 0)
            {
                // Scroll left.
                this.ScrollControlIntoView(TopLeftControl);
            }
            else
            {
                // Scroll right.
                this.ScrollControlIntoView(BottomRightControl);
            }
            if (dy < 0)
            {
                // Scroll up.
                this.ScrollControlIntoView(TopLeftControl);
            }
            else
            {
                // Scroll down.
                this.ScrollControlIntoView(BottomRightControl);
            }
        }

        public void InitialJumpToPlayer()
        {
            // Get dimensions in tiles.
            int tWidth = this.ClientSize.Width / 32;
            int tHeight = this.ClientSize.Height / 32;

            if (tWidth % 2 == 0)
            {
                tWidth++;
            }
            if (tHeight % 2 == 0)
            {
                tHeight++;
            }

            int tx = tWidth / 2;
            int ty = tHeight / 2;

            //Control c = new Control();
            //c.Location = new Point((Game.Player.X() * 32) + (tx * 32), (Game.Player.Y() * 32) + (ty * 32));
            //// This should work because the level always initialises at 0, 0.
            //this.ScrollControlIntoView(c);

            TopLeftControl.Location = new Point((Game.Player.X() - tx) * 32, (Game.Player.Y() - ty) * 32);
            BottomRightControl.Location = new Point((Game.Player.X() + tx) * 32, (Game.Player.Y() + ty) * 32);

            this.ScrollControlIntoView(TopLeftControl);
            this.ScrollControlIntoView(BottomRightControl);
            //DrawMap(Game.DungeonMap);
        }

        public void EraseMob(int x, int y, Map m)
        {
            // Repaints a single tile with the basic texture, overwriting any mobs (OR ITEMS) there.
            Tile t = m.TileArray[x, y];

            if (t.IsInFOV)
            {
                MapDrawer.DrawImage(Image.FromFile(t.TileGraphic), t.X * 32, t.Y * 32);
            }
            else
            {
                MapDrawer.DrawImage(Image.FromFile(t.DarkTileGraphic), t.X * 32, t.Y * 32);
            }
            WorldMapPanel.Refresh();
        }
    }
}
