namespace Samples.Extended
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
            this.SampleListBox = new System.Windows.Forms.ListBox();
            this.LaunchButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SampleListBox
            // 
            this.SampleListBox.FormattingEnabled = true;
            this.SampleListBox.Location = new System.Drawing.Point(12, 11);
            this.SampleListBox.Name = "SampleListBox";
            this.SampleListBox.Size = new System.Drawing.Size(192, 277);
            this.SampleListBox.TabIndex = 3;
            // 
            // LaunchButton
            // 
            this.LaunchButton.Location = new System.Drawing.Point(423, 237);
            this.LaunchButton.Name = "LaunchButton";
            this.LaunchButton.Size = new System.Drawing.Size(133, 51);
            this.LaunchButton.TabIndex = 4;
            this.LaunchButton.Text = "Launch";
            this.LaunchButton.UseVisualStyleBackColor = true;
            this.LaunchButton.Click += new System.EventHandler(this.LaunchButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 304);
            this.Controls.Add(this.LaunchButton);
            this.Controls.Add(this.SampleListBox);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MonoGame.Extended Samples";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox SampleListBox;
        private System.Windows.Forms.Button LaunchButton;
    }
}