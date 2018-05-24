using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.IO;
using System.Windows.Forms;

namespace PotatoSalad
{
    public partial class Form1 : Form
    {
        private PictureBox[,] MapDisplayArray;
        public List<PictureBox> MobileList = new List<PictureBox>();
        private Size TileSize;
        // x, y

        public Form1()
        {
            InitializeComponent();
            this.Text = "The World";    //Sets the window title.

            TileSize = new Size(32, 32);
            MapDisplayArray = new PictureBox[80, 25];
            // It's going to run 0-79, 0-24.
            for (int i = 0; i < 80; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    MapDisplayArray[i, j] = new PictureBox();
                    MapDisplayArray[i, j].Size = TileSize;
                    MapDisplayArray[i, j].Location = new Point(32 * i, 32 * j);
                    this.Controls.Add(MapDisplayArray[i, j]);
                }
            }
            InitialiseMobileList(Game.DungeonMap);
        }

        private void InitialiseMobileList(Map m)
        {
            foreach (Mobile mob in Game.DungeonMap.MobileArray)
            {
                PictureBox pb = new PictureBox();
                MobileList.Add(pb);
                pb.Tag = mob.id;
                pb.Size = TileSize;
                pb.Image = Image.FromFile(mob.displayGraphic);
                pb.BackColor = Color.Transparent;
                pb.Location = new Point(0, 0);  // Will be relative to its container.
            }
        }

        public void DisplayDeath()
        {
            // A placeholder so that killing mobs can be reflected in the display.
            // Basically just dak the picture box with the matching tag.
        }

        //public void DisplayMove(string id, int destX, int destY)
        //{
        //    // So when a thing moves, its picturebox can be reassigned to the appropriate place.
        //    PictureBox mpb = FindMobBox(id);
        //    mpb.Parent = MapDisplayArray[destX, destY];
        //}

        public void DisplayAddMob()
        {
            // Another placeholder. For mid-level summons, spawns, etc.
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawMap(Game.DungeonMap);
        }

        public void DrawMap(Map m)
        {
            List<Tile> fovList = Game.FOVCalculator.ReturnFOVList(
                m.TileArray,
                Game.Player.X(),
                Game.Player.Y(),
                Game.Player.FOVRange);

            foreach (Tile t in m.TileArray)
            {
                t.IsInFOV = false;
            }

            foreach (Tile t in fovList)
            {
                t.IsExplored = true;
                t.IsInFOV = true;
            }

            foreach (Tile t in m.TileArray)
            {
                // Don't draw anything further out than 80x25.
                PictureBox displayBox = MapDisplayArray[t.X, t.Y];

                if (t.X >= 0 && t.X <= 79 && t.Y >= 0 && t.Y <= 24)
                {
                    if (t.IsExplored)
                    {
                        if (t.IsInFOV)
                        {
                            displayBox.Image = Image.FromFile(t.TileGraphic);
                        }
                        else
                        {
                            displayBox.Image = Image.FromFile(t.DarkTileGraphic);
                        }
                    }
                    else
                    {
                        displayBox.Image = Image.FromFile(t.Blackout);
                    }
                    
                }

                if (t.Occupier != null)
                {
                    PictureBox mpb = FindMobBox(t.Occupier.id);
                    mpb.Parent = displayBox;
                }
            }
        }

        private void FindTag(Control.ControlCollection controls)
            // Not used for anything yet. But might be handy?
        {
            foreach (Control c in controls)
            {
                if (c.Tag != null)
                    //logic

                    if (c.HasChildren)
                        FindTag(c.Controls); //Recursively check all children controls as well; ie groupboxes or tabpages
            }
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
