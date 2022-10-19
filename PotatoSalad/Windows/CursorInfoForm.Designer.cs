namespace PotatoSalad.Windows
{
    partial class CursorInfoForm
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
            this.cursorInfoBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cursorInfoBox
            // 
            this.cursorInfoBox.Location = new System.Drawing.Point(-3, 0);
            this.cursorInfoBox.Multiline = true;
            this.cursorInfoBox.Name = "cursorInfoBox";
            this.cursorInfoBox.ReadOnly = true;
            this.cursorInfoBox.Size = new System.Drawing.Size(582, 232);
            this.cursorInfoBox.TabIndex = 0;
            // 
            // CursorInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 230);
            this.Controls.Add(this.cursorInfoBox);
            this.Name = "CursorInfoForm";
            this.Text = "Information";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox cursorInfoBox;
    }
}