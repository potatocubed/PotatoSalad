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
        // Right now Form1 handles a lot on its own...
        // But everything else should go through here.

        public void RenderText(string textToRender)
        {
            ListBox cOutput = Game.ConsoleForm.cOutput;
            cOutput.Items.Insert(0, textToRender);
        }
    }
}
