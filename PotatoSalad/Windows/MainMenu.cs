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
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NameChar();
        }

        private void NameChar()
        {
            TextInputBox nf = new TextInputBox();

            if (nf.ShowDialog(this) == DialogResult.OK)
            {
                // We're starting a new game.
                Game.NewGame(nf.typeTextHere.Text);
                this.Close();
            }
            else
            {
                //this.txtResult.Text = "Cancelled";
            }
            nf.Dispose();
        }
    }
}
