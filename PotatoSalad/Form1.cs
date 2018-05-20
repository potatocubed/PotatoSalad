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
            PictureBox pb1 = new PictureBox();
            pb1.Image = Image.FromFile("../../Graphics/Tiles/return-to-nature.png");
            pb1.Location = new Point(100, 100);
            pb1.Size = new Size(500, 500);
            this.Controls.Add(pb1);
        }
    }
}
