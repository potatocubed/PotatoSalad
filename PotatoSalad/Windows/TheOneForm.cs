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
    public partial class TheOneForm : Form
    {
        private Form WorldForm;
        private Form ConsoleForm;
        public bool MinimiseOkay = false;
        public bool AllFormsMinimised = false;

        public TheOneForm(Form wForm, Form cForm)
        {
            // This form is just a controller for the behaviour of all the other forms.
            InitializeComponent();
            this.Visible = false;
            this.Text = "Potato Salad";
            WorldForm = wForm;
            ConsoleForm = cForm;
        }

        private void TheOneForm_Load(object sender, EventArgs e)
        {
            this.Size = new System.Drawing.Size(0, 0);
        }

        private void TheOneForm_Resize(Object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                // It was minimised, then I clicked the icon on the taskbar.
                WorldForm.WindowState = FormWindowState.Normal;
                ConsoleForm.WindowState = FormWindowState.Normal;
            }
        }

        private void TheOneForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // This is where the data dump for the auto-savegame happens.
        }
    }
}
