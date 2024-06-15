namespace DesktopSetupV2
{
    partial class DialogBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogBox));
            this.listBoxLiveLogging = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBoxLiveLogging
            // 
            this.listBoxLiveLogging.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxLiveLogging.FormattingEnabled = true;
            this.listBoxLiveLogging.Location = new System.Drawing.Point(0, 0);
            this.listBoxLiveLogging.Name = "listBoxLiveLogging";
            this.listBoxLiveLogging.Size = new System.Drawing.Size(484, 461);
            this.listBoxLiveLogging.TabIndex = 0;
            // 
            // DialogBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 461);
            this.Controls.Add(this.listBoxLiveLogging);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DialogBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Tag = "";
            this.Text = "Live Logging";
            this.Load += new System.EventHandler(this.DialogBox_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxLiveLogging;
    }
}