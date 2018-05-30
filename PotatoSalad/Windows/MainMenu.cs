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
                // Do nothing.
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
                    // So do nothing.
                }

                // We'll generate a picturebox for each loadgame, and display it in
                // the side panel. Click on it to load your game!

                Bitmap bmp = new Bitmap(this.LoadListPanel.Width - 30, 64);  // -4 on the width to allow for a 2px border on the picturebox.
                Graphics drawer = Graphics.FromImage(bmp);
                drawer.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                drawer.FillRectangle(Brushes.Black, 0, 0, bmp.Width, bmp.Height);

                // Create a rectangle for the top line of text.
                RectangleF rectTop = new RectangleF(8, 8, bmp.Width - 8, 24);
                RectangleF rectBottom = new RectangleF(8, bmp.Height - 32, bmp.Width - 8, 24);

                // Make it look fancy.
                drawer.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                drawer.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                drawer.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                drawer.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

                StringFormat sf = new StringFormat()
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center,
                };

                Font drawFont = new Font("Consolas", 14);
                Font drawFontBold = new Font("Consolas", 14, FontStyle.Bold);

                for(int i = 0; i <= loadList.GetUpperBound(1); i++)
                {
                    // Draw the text onto the image
                    drawer.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                    drawer.DrawString(loadList[0, i], drawFontBold, Brushes.White, rectTop, sf);
                    drawer.DrawString(loadList[3, i], drawFont, Brushes.White, rectBottom, sf);
                    drawer.Flush();

                    // Display the picturebox.
                    PictureBox pb = new PictureBox();
                    
                    pb.Tag = loadList[1, i];    // Click on it, get this tag, that's your working directory.
                    pb.Width = bmp.Width + 4;   // This clever trick gives me a white border.
                    pb.Height = bmp.Height + 4;
                    pb.BackColor = Color.White;
                    pb.BackgroundImage = (Image)bmp.Clone();
                    pb.BackgroundImageLayout = ImageLayout.Center;
                    this.LoadListPanel.Controls.Add(pb);
                    pb.Location = new Point(0, (i * 76));
                    pb.Visible = true;

                    // There needs to be a whole thing here where we set up the picturebox for being clicked on.
                    pb.MouseEnter += (sender, e) =>
                    {
                        pb.BackColor = Color.Yellow;
                    };
                    pb.MouseLeave += (sender, e) =>
                    {
                        pb.BackColor = Color.White;
                    };
                    pb.Click += (sender, e) =>
                    {
                        // Here is where we jump out to the loadgame method.
                        //MessageBox.Show((String)pb.Tag);

                        // We need to load the map first.
                        // Into DungeonMap and also LevelXML.
                        // (Could do LevelXML first then have DungeonMap draw from that?)

                        // Then we load the player.
                        // Then we close this form and on with the show.
                    };

                    // Reset bmp for future use.
                    drawer.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                    drawer.FillRectangle(Brushes.Black, 0, 0, bmp.Width, bmp.Height);
                }
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
