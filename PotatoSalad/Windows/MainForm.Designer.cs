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
            this.worldMapPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.worldMapPanel.Name = "worldMapPanel";
            this.worldMapPanel.Size = new System.Drawing.Size(720, 738);
            this.worldMapPanel.TabIndex = 0;
            this.worldMapPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.worldMapPanel_Paint);
            // 
            // consoleListBox
            // 
            this.consoleListBox.FormattingEnabled = true;
            this.consoleListBox.ItemHeight = 20;
            this.consoleListBox.Location = new System.Drawing.Point(18, 748);
            this.consoleListBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.consoleListBox.Name = "consoleListBox";
            this.consoleListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.consoleListBox.Size = new System.Drawing.Size(679, 164);
            this.consoleListBox.TabIndex = 1;
            // 
            // infoBox
            // 
            this.infoBox.Location = new System.Drawing.Point(735, 18);
            this.infoBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.infoBox.Multiline = true;
            this.infoBox.Name = "infoBox";
            this.infoBox.ReadOnly = true;
            this.infoBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.infoBox.Size = new System.Drawing.Size(446, 89);
            this.infoBox.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1202, 925);
            this.Controls.Add(this.infoBox);
            this.Controls.Add(this.consoleListBox);
            this.Controls.Add(this.worldMapPanel);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainForm";
            this.Text = "PotatoSalad";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
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