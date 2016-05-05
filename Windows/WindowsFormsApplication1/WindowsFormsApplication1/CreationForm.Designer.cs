namespace WindowsFormsApplication1
{
    partial class CreationForm
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.customizationButton = new System.Windows.Forms.Button();
            this.templateList = new System.Windows.Forms.ListBox();
            this.positionText = new System.Windows.Forms.TextBox();
            this.firstXCoorScroller = new System.Windows.Forms.NumericUpDown();
            this.firstYCoorScroller = new System.Windows.Forms.NumericUpDown();
            this.secondXCoorScroller = new System.Windows.Forms.NumericUpDown();
            this.secondYCoorScroller = new System.Windows.Forms.NumericUpDown();
            this.confirmationButton = new System.Windows.Forms.Button();
            this.templateConfirmationButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.firstXCoorScroller)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.firstYCoorScroller)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.secondXCoorScroller)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.secondYCoorScroller)).BeginInit();
            this.SuspendLayout();
            // 
            // snappingButton
            // 
            this.snappingButton.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.snappingButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
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
            this.positioningText.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.positioningText.Location = new System.Drawing.Point(12, 151);
            this.positioningText.Name = "positioningText";
            this.positioningText.Size = new System.Drawing.Size(154, 38);
            this.positioningText.TabIndex = 1;
            this.positioningText.Text = "Positioning";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Cross;
            this.pictureBox1.Location = new System.Drawing.Point(381, 168);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(737, 341);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // customizationButton
            // 
            this.customizationButton.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.customizationButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.customizationButton.Font = new System.Drawing.Font("Arial", 26F);
            this.customizationButton.Location = new System.Drawing.Point(12, 12);
            this.customizationButton.Name = "customizationButton";
            this.customizationButton.Size = new System.Drawing.Size(243, 59);
            this.customizationButton.TabIndex = 3;
            this.customizationButton.Text = "Customize";
            this.customizationButton.UseVisualStyleBackColor = false;
            //this.customizationButton.Click += new System.EventHandler(this.customizationButtonClick); //pictureBox now visible by default
            // 
            // templateList
            // 
            this.templateList.BackColor = System.Drawing.Color.GhostWhite;
            this.templateList.Font = new System.Drawing.Font("Arial Narrow", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.templateList.FormattingEnabled = true;
            this.templateList.ItemHeight = 42;
            this.templateList.Location = new System.Drawing.Point(12, 200);
            this.templateList.Name = "templateList";
            this.templateList.Size = new System.Drawing.Size(298, 298);
            this.templateList.TabIndex = 4;
            this.templateList.SelectedIndexChanged += new System.EventHandler(this.templateListSelectedIndexChanged);
            // 
            // positionText
            // 
            this.positionText.Location = new System.Drawing.Point(913, 12);
            this.positionText.Name = "positionText";
            this.positionText.Size = new System.Drawing.Size(220, 20);
            this.positionText.TabIndex = 5;
            // 
            // firstXCoorScroller
            // 
            this.firstXCoorScroller.Location = new System.Drawing.Point(381, 87);
            this.firstXCoorScroller.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.firstXCoorScroller.Name = "firstXCoorScroller";
            this.firstXCoorScroller.Size = new System.Drawing.Size(120, 20);
            this.firstXCoorScroller.TabIndex = 6;
            this.firstXCoorScroller.ValueChanged += new System.EventHandler(this.firstXCoorScroller_ValueChanged);
            // 
            // firstYCoorScroller
            // 
            this.firstYCoorScroller.Location = new System.Drawing.Point(381, 115);
            this.firstYCoorScroller.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.firstYCoorScroller.Name = "firstYCoorScroller";
            this.firstYCoorScroller.Size = new System.Drawing.Size(120, 20);
            this.firstYCoorScroller.TabIndex = 7;
            this.firstYCoorScroller.ValueChanged += new System.EventHandler(this.firstYCoorScroller_ValueChanged);
            // 
            // secondXCoorScroller
            // 
            this.secondXCoorScroller.Location = new System.Drawing.Point(557, 87);
            this.secondXCoorScroller.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.secondXCoorScroller.Name = "secondXCoorScroller";
            this.secondXCoorScroller.Size = new System.Drawing.Size(120, 20);
            this.secondXCoorScroller.TabIndex = 8;
            this.secondXCoorScroller.ValueChanged += new System.EventHandler(this.secondXCoorScroller_ValueChanged);
            // 
            // secondYCoorScroller
            // 
            this.secondYCoorScroller.Location = new System.Drawing.Point(557, 114);
            this.secondYCoorScroller.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.secondYCoorScroller.Name = "secondYCoorScroller";
            this.secondYCoorScroller.Size = new System.Drawing.Size(120, 20);
            this.secondYCoorScroller.TabIndex = 9;
            this.secondYCoorScroller.ValueChanged += new System.EventHandler(this.secondYCoorScroller_ValueChanged);
            // 
            // confirmationButton
            // 
            this.confirmationButton.Location = new System.Drawing.Point(711, 84);
            this.confirmationButton.Name = "confirmationButton";
            this.confirmationButton.Size = new System.Drawing.Size(147, 50);
            this.confirmationButton.TabIndex = 10;
            this.confirmationButton.Text = "Confirmation Button";
            this.confirmationButton.UseVisualStyleBackColor = true;
            this.confirmationButton.Click += new System.EventHandler(this.confirmationButton_Click);
            // 
            // templateConfirmationButton
            // 
            this.templateConfirmationButton.Location = new System.Drawing.Point(896, 84);
            this.templateConfirmationButton.Name = "templateConfirmationButton";
            this.templateConfirmationButton.Size = new System.Drawing.Size(160, 50);
            this.templateConfirmationButton.TabIndex = 11;
            this.templateConfirmationButton.Text = "templateConfirmationButton";
            this.templateConfirmationButton.UseVisualStyleBackColor = true;
            this.templateConfirmationButton.Click += new System.EventHandler(this.templateConfirmationButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(378, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "CursorPos:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(378, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Handle Title:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(378, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Window Moving:";
            // 
            // CreationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 521);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.templateConfirmationButton);
            this.Controls.Add(this.confirmationButton);
            this.Controls.Add(this.secondYCoorScroller);
            this.Controls.Add(this.secondXCoorScroller);
            this.Controls.Add(this.firstYCoorScroller);
            this.Controls.Add(this.firstXCoorScroller);
            this.Controls.Add(this.positionText);
            this.Controls.Add(this.templateList);
            this.Controls.Add(this.customizationButton);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.positioningText);
            this.Controls.Add(this.snappingButton);
            this.Name = "CreationForm";
            this.ShowIcon = false;
            this.Text = "SplitScreen";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.firstXCoorScroller)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.firstYCoorScroller)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.secondXCoorScroller)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.secondYCoorScroller)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void PictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.Button snappingButton;
        private System.Windows.Forms.TextBox positioningText;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button customizationButton;
        private System.Windows.Forms.ListBox templateList;
        private System.Windows.Forms.TextBox positionText;
        private System.Windows.Forms.NumericUpDown firstXCoorScroller;
        private System.Windows.Forms.NumericUpDown firstYCoorScroller;
        private System.Windows.Forms.NumericUpDown secondXCoorScroller;
        private System.Windows.Forms.NumericUpDown secondYCoorScroller;
        private System.Windows.Forms.Button confirmationButton;
        private System.Windows.Forms.Button templateConfirmationButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

