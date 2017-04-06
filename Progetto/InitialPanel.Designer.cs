namespace Progetto
{
    partial class InitialPanel
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
            this.freqeuncies = new System.Windows.Forms.ComboBox();
            this.info_label = new System.Windows.Forms.Label();
            this.bntOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // freqeuncies
            // 
            this.freqeuncies.FormattingEnabled = true;
            this.freqeuncies.Items.AddRange(new object[] {
            "50 Hz",
            "100 Hz",
            "200 Hz"});
            this.freqeuncies.Location = new System.Drawing.Point(76, 78);
            this.freqeuncies.Name = "freqeuncies";
            this.freqeuncies.Size = new System.Drawing.Size(121, 21);
            this.freqeuncies.TabIndex = 0;
            this.freqeuncies.SelectedIndexChanged += new System.EventHandler(this.freqeuncies_SelectedIndexChanged);
            // 
            // info_label
            // 
            this.info_label.AutoSize = true;
            this.info_label.Location = new System.Drawing.Point(55, 47);
            this.info_label.Name = "info_label";
            this.info_label.Size = new System.Drawing.Size(185, 13);
            this.info_label.TabIndex = 1;
            this.info_label.Text = "Please, select the packets frequency:";
            this.info_label.Click += new System.EventHandler(this.info_Click);
            // 
            // bntOk
            // 
            this.bntOk.Location = new System.Drawing.Point(101, 121);
            this.bntOk.Name = "bntOk";
            this.bntOk.Size = new System.Drawing.Size(75, 23);
            this.bntOk.TabIndex = 2;
            this.bntOk.Text = "OK";
            this.bntOk.UseVisualStyleBackColor = true;
            this.bntOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // InitialPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 177);
            this.Controls.Add(this.bntOk);
            this.Controls.Add(this.info_label);
            this.Controls.Add(this.freqeuncies);
            this.Name = "InitialPanel";
            this.Text = "Welcome to Xbus Master 2.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InitPanel_Closing);
            this.Load += new System.EventHandler(this.InitPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox freqeuncies;
        private System.Windows.Forms.Label info_label;
        private System.Windows.Forms.Button bntOk;
    }
}