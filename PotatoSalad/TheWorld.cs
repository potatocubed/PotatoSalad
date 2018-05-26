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
        public List<PictureBox> MobileList = new List<PictureBox>();
        private Size TileSize;
        private List<Tile> oldFOV;
        private Bitmap MapImage;
        private Graphics MapDrawer;

        public TheWorld()
        {
            InitializeComponent();
            this.Text = "The World";    //Sets the window title.

            oldFOV = new List<Tile>();
            TileSize = new Size(32, 32);
            InitialiseMobileList(Game.DungeonMap);
            MapImage = new Bitmap(this.WorldMapPanel.Width, this.WorldMapPanel.Height);
            MapDrawer = Graphics.FromImage(MapImage);
            MapDrawer.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            MapDrawer.FillRectangle(Brushes.Black, 0, 0, MapImage.Width, MapImage.Height);
        }

        private void InitialiseMobileList(Map m)
        {
            foreach (Mobile mob in m.MobileArray)
            {
                PictureBox pb = new PictureBox();
                MobileList.Add(pb);
                pb.Tag = mob.id;
                pb.Size = TileSize;
                pb.Image = Image.FromFile(mob.displayGraphic);
                pb.BackColor = Color.Transparent;
                pb.Location = new Point(-32, -32);  // Just off-screen?
                this.Controls.Add(pb);
            }
        }

        private void WorldForm_Paint(object sender, PaintEventArgs e)
        {
            DrawMap(Game.DungeonMap);
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
                    PictureBox mpb = FindMobBox(t.Occupier.id);
                    mpb.Location = new Point(t.X * 32, t.Y * 32);
                    mpb.BringToFront();
                }
            }
            oldFOV = newFOV;
            this.WorldMapPanel.BackgroundImage = MapImage;
        }

        private PictureBox FindMobBox(string tagToFind)
        {
            foreach (PictureBox pb in MobileList)
            {
                if (pb.Tag.ToString() == tagToFind)
                {
                    return pb;
                }
            }
            return null;
        }
    }
}
