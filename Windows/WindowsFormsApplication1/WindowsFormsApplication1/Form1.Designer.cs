namespace WindowsFormsApplication1
{
    partial class Form1
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
            this.snappingButton = new System.Windows.Forms.Button();
            this.positioningText = new System.Windows.Forms.TextBox();
            this.wallpaperImage = new System.Windows.Forms.PictureBox();
            this.customizationButton = new System.Windows.Forms.Button();
            this.templateList = new System.Windows.Forms.ListBox();
            this.positionText = new System.Windows.Forms.TextBox();
            this.firstXCoorScroller = new System.Windows.Forms.NumericUpDown();
            this.firstYCoorScroller = new System.Windows.Forms.NumericUpDown();
            this.secondXCoorScroller = new System.Windows.Forms.NumericUpDown();
            this.secondYCoorScroller = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.wallpaperImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.firstXCoorScroller)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.firstYCoorScroller)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.secondXCoorScroller)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.secondYCoorScroller)).BeginInit();
            this.SuspendLayout();
            // 
            // snappingButton
            // 
            this.snappingButton.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.snappingButton.Font = new System.Drawing.Font("Arial", 26F);
            this.snappingButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.snappingButton.Location = new System.Drawing.Point(12, 87);
            this.snappingButton.Name = "snappingButton";
            this.snappingButton.Size = new System.Drawing.Size(243, 59);
            this.snappingButton.TabIndex = 0;
            this.snappingButton.Text = "Run";
            this.snappingButton.UseVisualStyleBackColor = false;
            this.snappingButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // positioningText
            // 
            this.positioningText.BackColor = System.Drawing.Color.Fuchsia;
            this.positioningText.Font = new System.Drawing.Font("PT Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.positioningText.Location = new System.Drawing.Point(12, 26);
            this.positioningText.Name = "positioningText";
            this.positioningText.Size = new System.Drawing.Size(154, 43);
            this.positioningText.TabIndex = 1;
            this.positioningText.Text = "Positioning";
            // 
            // wallpaperImage
            // 
            this.wallpaperImage.Location = new System.Drawing.Point(381, 164);
            this.wallpaperImage.Name = "wallpaperImage";
            this.wallpaperImage.Size = new System.Drawing.Size(737, 341);
            this.wallpaperImage.TabIndex = 2;
            this.wallpaperImage.TabStop = false;
            this.wallpaperImage.Visible = false;
            // 
            // customizationButton
            // 
            this.customizationButton.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.customizationButton.Font = new System.Drawing.Font("Arial", 26F);
            this.customizationButton.Location = new System.Drawing.Point(281, 10);
            this.customizationButton.Name = "customizationButton";
            this.customizationButton.Size = new System.Drawing.Size(243, 59);
            this.customizationButton.TabIndex = 3;
            this.customizationButton.Text = "Customize";
            this.customizationButton.UseVisualStyleBackColor = false;
            this.customizationButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // templateList
            // 
            this.templateList.BackColor = System.Drawing.Color.GhostWhite;
            this.templateList.Font = new System.Drawing.Font("Segoe Print", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.templateList.FormattingEnabled = true;
            this.templateList.ItemHeight = 61;
            this.templateList.Location = new System.Drawing.Point(12, 200);
            this.templateList.Name = "templateList";
            this.templateList.Size = new System.Drawing.Size(298, 309);
            this.templateList.TabIndex = 4;
            this.templateList.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // positionText
            // 
            this.positionText.Location = new System.Drawing.Point(12, 164);
            this.positionText.Name = "positionText";
            this.positionText.Size = new System.Drawing.Size(220, 20);
            this.positionText.TabIndex = 5;
            // 
            // firstXCoorScroller
            // 
            this.firstXCoorScroller.Location = new System.Drawing.Point(381, 87);
            this.firstXCoorScroller.Name = "firstXCoorScroller";
            this.firstXCoorScroller.Size = new System.Drawing.Size(120, 20);
            this.firstXCoorScroller.TabIndex = 6;
            this.firstYCoorScroller.Maximum = 2000;
            // 
            // firstYCoorScroller
            // 
            this.firstYCoorScroller.Location = new System.Drawing.Point(381, 115);
            this.firstYCoorScroller.Name = "firstYCoorScroller";
            this.firstYCoorScroller.Size = new System.Drawing.Size(120, 20);
            this.firstYCoorScroller.TabIndex = 7;
            this.firstXCoorScroller.Maximum = 2000;
            // 
            // numericUpDown1
            // 
            this.secondXCoorScroller.Location = new System.Drawing.Point(557, 87);
            this.secondXCoorScroller.Name = "secondXCoorScroller";
            this.secondXCoorScroller.Size = new System.Drawing.Size(120, 20);
            this.secondXCoorScroller.TabIndex = 8;
            this.secondXCoorScroller.Maximum = 2000;
            // 
            // numericUpDown2
            // 
            this.secondYCoorScroller.Location = new System.Drawing.Point(557, 114);
            this.secondYCoorScroller.Name = "secondYCoorScroller";
            this.secondYCoorScroller.Size = new System.Drawing.Size(120, 20);
            this.secondYCoorScroller.TabIndex = 9;
            this.secondYCoorScroller.Maximum = 2000;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 521);
            this.Controls.Add(this.secondYCoorScroller);
            this.Controls.Add(this.secondXCoorScroller);
            this.Controls.Add(this.firstYCoorScroller);
            this.Controls.Add(this.firstXCoorScroller);
            this.Controls.Add(this.positionText);
            this.Controls.Add(this.templateList);
            this.Controls.Add(this.customizationButton);
            this.Controls.Add(this.wallpaperImage);
            this.Controls.Add(this.positioningText);
            this.Controls.Add(this.snappingButton);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "SplitScreen";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.wallpaperImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.firstXCoorScroller)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.firstYCoorScroller)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.secondXCoorScroller)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.secondYCoorScroller)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button snappingButton;
        private System.Windows.Forms.TextBox positioningText;
        private System.Windows.Forms.PictureBox wallpaperImage;
        private System.Windows.Forms.Button customizationButton;
        private System.Windows.Forms.ListBox templateList;
        private System.Windows.Forms.TextBox positionText;
        private System.Windows.Forms.NumericUpDown firstXCoorScroller;
        private System.Windows.Forms.NumericUpDown firstYCoorScroller;
        private System.Windows.Forms.NumericUpDown secondXCoorScroller;
        private System.Windows.Forms.NumericUpDown secondYCoorScroller;
    }
}

