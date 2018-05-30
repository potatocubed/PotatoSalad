using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace PotatoSalad
{
    public partial class MainMenu : Form
    {
        string[,] loadList;
        // Dimension 1:
        // 0: CharName
        // 1: Directory (for loading from)
        // 2: CharDetails (e.g. Daughter of Blorf, commanding Spoons, Love, and Heroism)
        // 3: CurrentLocation (e.g. 'Dungeon, Level 1')

        public MainMenu()
        {
            InitializeComponent();

            // Grey out the load game button if there are none to load.
            string here = Directory.GetCurrentDirectory();
            string dataDir = here + "/data";
            if (!Directory.Exists(dataDir) || Directory.GetDirectories(dataDir).Count() == 0)
            {
                this.buttonLoadGame.Enabled = false;
            }
            else
            {
                // Prep the load-list.
                loadList = new string[4, Directory.GetDirectories(dataDir).Count()];
                int targRow = 0;
                XmlDocument f = new XmlDocument();
                XmlNode xNode;
                foreach (string d in Directory.GetDirectories(dataDir))
                {
                    if (File.Exists(d + "/character.xml"))
                    {
                        try
                        {
                            f.Load(d + "/character.xml");
                            xNode = f.SelectSingleNode("/CharData/Name");
                            loadList[0, targRow] = xNode.InnerText;
                            loadList[1, targRow] = d;
                            //xNode = f.SelectSingleNode("/CharData/Name");
                            loadList[2, targRow] = "";  // Got no details to put here yet.
                            xNode = f.SelectSingleNode("/CharData/Location");
                            loadList[3, targRow] = xNode.InnerText;
                            targRow++;
                        }
                        catch
                        {
                            // There's something wrong with the XML.
                            // Skip it.
                        }
                    }
                }
                if (loadList[1, 0] == "")
                {
                    // We've got a situation where nothing has passed validation.
                    this.buttonLoadGame.Enabled = false;
                }

                // Okay, fuck the load button.
                // We'll generate a picturebox for each loadgame, and display it in
                // the side panel. Click on it to load your game!

                Bitmap bmp = new Bitmap(this.LoadListPanel.Width, 32);
                Graphics drawer = Graphics.FromImage(bmp);
                drawer.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                drawer.FillRectangle(Brushes.Black, 0, 0, bmp.Width, bmp.Height);

                // Create a rectangle for the top line of text.
                RectangleF rectf = new RectangleF(4, 4, bmp.Width - 8, (bmp.Height - 8) / 2);

                // Make it look fancy.
                drawer.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                drawer.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                drawer.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                drawer.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

                StringFormat sf = new StringFormat()
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Near,
                };

                Font drawFont = new Font("Consolas", 11);
                Font drawFontBold = new Font("Consolas", 11, FontStyle.Bold);

                //// Draw the text onto the image
                //g.DrawString("yourText", new Font("Tahoma", 8), Brushes.Black, rectf, format);

                //// Flush all graphics changes to the bitmap
                //g.Flush();

                //// Now save or use the bitmap
                //image.Image = bmp;
            }
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
