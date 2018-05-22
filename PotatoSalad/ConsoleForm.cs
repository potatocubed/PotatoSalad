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
    public partial class ConsoleForm : Form
    {
        public ConsoleForm()
        {
            InitializeComponent();
            this.Text = "Console";    //Sets the window title.
        }

        private void ConsoleForm_Load(object sender, EventArgs e)
        {
            //DrawMap(Game.DungeonMap);
            RenderText("If you can read this, it's worked.");
        }
        
        private void RenderText(string textToRender)
        {
            ListBox cOutput = this.FakeConsole;
            FakeConsole.Items.Insert(0, textToRender);
            FakeConsole.Items.Insert(0, "And this is for debugging purposes.");
        }

    }
}
