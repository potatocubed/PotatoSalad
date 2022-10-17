using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PotatoSalad
{
    class SaladGraphics
    {
        // This class exists to ease any future swapping out of display options.

        public void RenderText(string textToRender)
        {
            ListBox cOutput = Game.ConsoleForm.cOutput;
            cOutput.Items.Insert(0, textToRender);
            Game.ConsoleForm.Refresh();
        }

        public void DrawMap(Map m)
        {
            Game.WorldForm.DrawMap(m);
        }
    }
}
