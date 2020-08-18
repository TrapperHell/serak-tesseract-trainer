namespace SerakTesseractTrainer
{
    partial class TesseractConfig
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TesseractConfig));
            this.txtTesseractPath = new System.Windows.Forms.TextBox();
            this.lblTesseractPath = new System.Windows.Forms.Label();
            this.btnConfig_BrowseTesseractPath = new System.Windows.Forms.Button();
            this.lblIsoLang = new System.Windows.Forms.Label();
            this.btnConfig_Save = new System.Windows.Forms.Button();
            this.txtIsoLang = new System.Windows.Forms.MaskedTextBox();
            this.epMain = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.epMain)).BeginInit();
            this.SuspendLayout();
            // 
            // txtTesseractPath
            // 
            this.txtTesseractPath.Location = new System.Drawing.Point(103, 44);
            this.txtTesseractPath.MaxLength = 300;
            this.txtTesseractPath.Name = "txtTesseractPath";
            this.txtTesseractPath.Size = new System.Drawing.Size(287, 20);
            this.txtTesseractPath.TabIndex = 0;
            // 
            // lblTesseractPath
            // 
            this.lblTesseractPath.AutoSize = true;
            this.lblTesseractPath.Location = new System.Drawing.Point(12, 47);
            this.lblTesseractPath.Name = "lblTesseractPath";
            this.lblTesseractPath.Size = new System.Drawing.Size(82, 13);
            this.lblTesseractPath.TabIndex = 1;
            this.lblTesseractPath.Text = "Tesseract Path:";
            // 
            // btnConfig_BrowseTesseractPath
            // 
            this.btnConfig_BrowseTesseractPath.Location = new System.Drawing.Point(411, 44);
            this.btnConfig_BrowseTesseractPath.Name = "btnConfig_BrowseTesseractPath";
            this.btnConfig_BrowseTesseractPath.Size = new System.Drawing.Size(39, 20);
            this.btnConfig_BrowseTesseractPath.TabIndex = 1;
            this.btnConfig_BrowseTesseractPath.Text = "...";
            this.btnConfig_BrowseTesseractPath.UseVisualStyleBackColor = true;
            this.btnConfig_BrowseTesseractPath.Click += new System.EventHandler(this.btnConfig_BrowseTesseractPath_Click);
            // 
            // lblIsoLang
            // 
            this.lblIsoLang.AutoSize = true;
            this.lblIsoLang.Location = new System.Drawing.Point(12, 82);
            this.lblIsoLang.Name = "lblIsoLang";
            this.lblIsoLang.Size = new System.Drawing.Size(85, 13);
            this.lblIsoLang.TabIndex = 1;
            this.lblIsoLang.Text = "ISO 639-2 Lang:";
            // 
            // btnConfig_Save
            // 
            this.btnConfig_Save.Location = new System.Drawing.Point(152, 129);
            this.btnConfig_Save.Name = "btnConfig_Save";
            this.btnConfig_Save.Size = new System.Drawing.Size(134, 23);
            this.btnConfig_Save.TabIndex = 3;
            this.btnConfig_Save.Text = "Save";
            this.btnConfig_Save.UseVisualStyleBackColor = true;
            this.btnConfig_Save.Click += new System.EventHandler(this.btnConfig_Save_Click);
            // 
            // txtIsoLang
            // 
            this.txtIsoLang.Location = new System.Drawing.Point(103, 79);
            this.txtIsoLang.Mask = "???";
            this.txtIsoLang.Name = "txtIsoLang";
            this.txtIsoLang.Size = new System.Drawing.Size(287, 20);
            this.txtIsoLang.TabIndex = 2;
            this.txtIsoLang.Text = "eng";
            this.txtIsoLang.Click += new System.EventHandler(this.txtIsoLang_SetCaretPosition);
            this.txtIsoLang.Enter += new System.EventHandler(this.txtIsoLang_SetCaretPosition);
            // 
            // epMain
            // 
            this.epMain.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink;
            this.epMain.ContainerControl = this;
            // 
            // TesseractConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(480, 172);
            this.Controls.Add(this.txtIsoLang);
            this.Controls.Add(this.btnConfig_Save);
            this.Controls.Add(this.btnConfig_BrowseTesseractPath);
            this.Controls.Add(this.lblIsoLang);
            this.Controls.Add(this.lblTesseractPath);
            this.Controls.Add(this.txtTesseractPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = global::SerakTesseractTrainer.Properties.Resources.icon;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TesseractConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.TesseractConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.epMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txtTesseractPath;
        private System.Windows.Forms.Label lblTesseractPath;
        private System.Windows.Forms.Button btnConfig_BrowseTesseractPath;
        private System.Windows.Forms.Label lblIsoLang;
        private System.Windows.Forms.Button btnConfig_Save;
        private System.Windows.Forms.MaskedTextBox txtIsoLang;
        private System.Windows.Forms.ErrorProvider epMain;
    }
}