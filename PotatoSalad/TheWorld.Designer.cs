﻿namespace PotatoSalad
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
            this.WorldMapPanel.Name = "WorldMapPanel";
            this.WorldMapPanel.Size = new System.Drawing.Size(2560, 800);
            this.WorldMapPanel.TabIndex = 0;
            this.WorldMapPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.WorldForm_Paint);
            // 
            // TheWorld
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.WorldMapPanel);
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(96, 96);
            this.Name = "TheWorld";
            this.Text = "TheWorld";
            this.Load += new System.EventHandler(this.WorldForm_Load);
            this.Resize += new System.EventHandler(this.TheWorld_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel WorldMapPanel;
    }
}