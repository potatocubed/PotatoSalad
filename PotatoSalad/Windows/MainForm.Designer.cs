namespace PotatoSalad.Windows
{
    partial class MainForm
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
            this.worldMapPanel = new System.Windows.Forms.Panel();
            this.consoleListBox = new System.Windows.Forms.ListBox();
            this.infoBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // worldMapPanel
            // 
            this.worldMapPanel.BackColor = System.Drawing.Color.Black;
            this.worldMapPanel.Location = new System.Drawing.Point(0, 0);
            this.worldMapPanel.Name = "worldMapPanel";
            this.worldMapPanel.Size = new System.Drawing.Size(480, 480);
            this.worldMapPanel.TabIndex = 0;
            this.worldMapPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.worldMapPanel_Paint);
            // 
            // consoleListBox
            // 
            this.consoleListBox.FormattingEnabled = true;
            this.consoleListBox.Location = new System.Drawing.Point(12, 486);
            this.consoleListBox.Name = "consoleListBox";
            this.consoleListBox.Size = new System.Drawing.Size(454, 108);
            this.consoleListBox.TabIndex = 1;
            // 
            // infoBox
            // 
            this.infoBox.Location = new System.Drawing.Point(490, 12);
            this.infoBox.Multiline = true;
            this.infoBox.Name = "infoBox";
            this.infoBox.ReadOnly = true;
            this.infoBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.infoBox.Size = new System.Drawing.Size(299, 59);
            this.infoBox.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(801, 601);
            this.Controls.Add(this.infoBox);
            this.Controls.Add(this.consoleListBox);
            this.Controls.Add(this.worldMapPanel);
            this.Name = "MainForm";
            this.Text = "PotatoSalad";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel worldMapPanel;
        private System.Windows.Forms.ListBox consoleListBox;
        private System.Windows.Forms.TextBox infoBox;
    }
}