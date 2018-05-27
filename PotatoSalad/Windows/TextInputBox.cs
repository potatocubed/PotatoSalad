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
    public partial class TextInputBox : Form
    {
        public TextInputBox()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {

        }

        void tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //enter key is down
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
