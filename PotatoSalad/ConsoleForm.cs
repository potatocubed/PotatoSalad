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
        public ListBox cOutput;

        public ConsoleForm()
        {
            InitializeComponent();
            this.Text = "Console";    //Sets the window title.
            cOutput = this.FakeConsole;
        }

        private void ConsoleForm_Load(object sender, EventArgs e)
        {
            if (Game.Globals.DEBUG_ERROR_LIST.Count > 0)
            {
                foreach (string s in Game.Globals.DEBUG_ERROR_LIST)
                {
                    Game.GI.RenderText(s);
                }
            }
            Game.Globals.DEBUG_ERROR_LIST.Clear();
        }
    }
}
