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
        public Form1()
        {
            InitializeComponent();
            this.Text = "The World";    //Sets the window title.
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
            foreach (Tile t in m.TileArray)
            {
                PictureBox pb = new PictureBox();
                pb.Image = Image.FromFile(t.TileGraphic);
                pb.Location = new Point(32 * t.X, 32 * t.Y);
                pb.Size = new Size(32, 32);
                this.Controls.Add(pb);  // TODO: Is this necessary?

                if(t.Occupier != null)
                {
                    PictureBox mpb = new PictureBox();
                    pb.Controls.Add(mpb);
                    mpb.Image = Image.FromFile(t.Occupier.displayGraphic);
                    mpb.Location = new Point(0, 0); // This is RELATIVE to the container.
                    mpb.Size = pb.Size;
                    mpb.BackColor = Color.Transparent;
                    mpb.BringToFront();
                }
            }

        }
    }
}
