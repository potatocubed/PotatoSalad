using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PotatoSalad
{
    class GraphicIntermediary
    {
        // This class exists to ease any future swapping out of display options.

        public void RenderText(string textToRender)
        {
            ListBox cOutput = Game.ConsoleForm.cOutput;
            cOutput.Items.Insert(0, textToRender);
            Game.ConsoleForm.Refresh();
        }

        public void ScrollBox(int origX, int origY, int destX, int destY)
        {
            // Move the screen with the PC.
            Game.WorldForm.ScrollWorld(origX, origY, destX, destY);
        }
    }
}
