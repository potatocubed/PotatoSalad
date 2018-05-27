namespace PotatoSalad
{
    partial class TheWorld
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.WorldMapPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // WorldMapPanel
            // 
            this.WorldMapPanel.BackColor = System.Drawing.Color.Black;
            this.WorldMapPanel.Location = new System.Drawing.Point(0, 0);
            this.WorldMapPanel.Margin = new System.Windows.Forms.Padding(0);
            this.WorldMapPanel.Name = "WorldMapPanel";
            this.WorldMapPanel.Size = new System.Drawing.Size(480, 480);
            this.WorldMapPanel.TabIndex = 0;
            this.WorldMapPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.WorldForm_Paint);
            // 
            // TheWorld
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(481, 481);
            this.Controls.Add(this.WorldMapPanel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimumSize = new System.Drawing.Size(96, 96);
            this.Name = "TheWorld";
            this.Text = "The World";
            this.Load += new System.EventHandler(this.WorldForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel WorldMapPanel;
    }
}