using System;
using System.Drawing;
using System.Windows.Forms;

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
            this.removeButton = new System.Windows.Forms.Button();
            this.positioningText = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.positionText = new System.Windows.Forms.TextBox();
            this.confirmationButton = new System.Windows.Forms.Button();
            this.templateConfirmationButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.basicOneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // removeButton
            // 
            this.removeButton.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.removeButton.Font = new System.Drawing.Font("Arial", 26F);
            this.removeButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.removeButton.Location = new System.Drawing.Point(83, 1);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(50, 43);
            this.removeButton.TabIndex = 0;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = false;
            // 
            // positioningText
            // 
            this.positioningText.BackColor = System.Drawing.Color.Fuchsia;
            this.positioningText.Font = new System.Drawing.Font("PT Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.positioningText.Location = new System.Drawing.Point(152, 0);
            this.positioningText.Name = "positioningText";
            this.positioningText.Size = new System.Drawing.Size(52, 43);
            this.positioningText.TabIndex = 1;
            this.positioningText.Text = "Positioning";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 49);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1184, 425);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            this.pictureBox1.MouseDoubleClick += new MouseEventHandler(this.pictureBox1_DoubleClick);
            // 
            // positionText
            // 
            this.positionText.Location = new System.Drawing.Point(976, 23);
            this.positionText.Name = "positionText";
            this.positionText.Size = new System.Drawing.Size(220, 20);
            this.positionText.TabIndex = 5;
            // 
            // confirmationButton
            // 
            this.confirmationButton.Location = new System.Drawing.Point(237, 19);
            this.confirmationButton.Name = "confirmationButton";
            this.confirmationButton.Size = new System.Drawing.Size(107, 20);
            this.confirmationButton.TabIndex = 10;
            this.confirmationButton.Text = "Confirmation Button";
            this.confirmationButton.UseVisualStyleBackColor = true;
            this.confirmationButton.Click += new System.EventHandler(this.confirmationButton_Click);
            // 
            // templateConfirmationButton
            // 
            this.templateConfirmationButton.Location = new System.Drawing.Point(371, 19);
            this.templateConfirmationButton.Name = "templateConfirmationButton";
            this.templateConfirmationButton.Size = new System.Drawing.Size(119, 20);
            this.templateConfirmationButton.TabIndex = 11;
            this.templateConfirmationButton.Text = "templateConfirmationButton";
            this.templateConfirmationButton.UseVisualStyleBackColor = true;
            this.templateConfirmationButton.Click += new System.EventHandler(this.templateConfirmationButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1234, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.toolStripSeparator1,
            this.editToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(100, 6);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.basicOneToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // basicOneToolStripMenuItem
            // 
            this.basicOneToolStripMenuItem.Name = "basicOneToolStripMenuItem";
            this.basicOneToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.basicOneToolStripMenuItem.Text = "Basic One";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.closeToolStripMenuItem.Text = "Close";
            // 
            // CreationForm
            // 
            this.ClientSize = new System.Drawing.Size(1234, 529);
            this.Controls.Add(this.templateConfirmationButton);
            this.Controls.Add(this.confirmationButton);
            this.Controls.Add(this.positionText);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.positioningText);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "CreationForm";
            this.ShowIcon = false;
            this.Text = "SplitScreen";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        private void Form1_Resize(object sender, EventArgs e)
        {
            //pictureBox1.Dispose();
            formSizeRatioX = (this.Width - 30) / (double)screenX;
            formSizeRatioY = (this.Height - 100) / (double)screenY;

            //GetPathOfWallpaper();
            currentImage = Image.FromFile(pathWallpaper);
            currentImage = ResizeImage(currentImage, Convert.ToInt32(screenX * formSizeRatioX), Convert.ToInt32(screenY * formSizeRatioY));
            this.pictureBox1.Size = new System.Drawing.Size(Convert.ToInt32(screenX * formSizeRatioX), Convert.ToInt32(screenY * formSizeRatioY));
            //Debug.Print((formSizeRatioX.ToString());
            
            
            this.pictureBox1.Image = currentImage;
            this.pictureBox1.Visible = true;
            pictureBox1.Update();
            pictureBox1.Refresh();
        }

        private void PictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.TextBox positioningText;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox positionText;
        private System.Windows.Forms.Button confirmationButton;
        private System.Windows.Forms.Button templateConfirmationButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem basicOneToolStripMenuItem;
    }
}

