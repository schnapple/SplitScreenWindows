﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;

    class Plexiglass : Form
    {
        public Plexiglass(int tX, int tY, int bY, int bX)
        {
       
            this.BackColor = Color.Red;
            CheckForIllegalCrossThreadCalls = false;
            this.Opacity = 0.60;      // Tweak as desired
            this.FormBorderStyle = FormBorderStyle.None;
            this.ControlBox = false;
            this.ShowInTaskbar = false;
            
            this.StartPosition = FormStartPosition.Manual;
            this.AutoScaleMode = AutoScaleMode.None;
            this.Size = new Size(bX, bY);
            this.Location = new Point(tX, tY);
            this.TopMost = false;
            this.ResizeRedraw = true;
            
            this.Show();

            //this.Location = PointToScreen(Point.Empty);
            //this.Location = tocover.PointToScreen(Point.Empty);
            //this.ClientSize = tocover.ClientSize;
            //tocover.LocationChanged += Cover_LocationChanged;
            //tocover.ClientSizeChanged += Cover_ClientSizeChanged;
            //this.Show(tocover);
            //tocover.Focus();
            // Disable Aero transitions, the plexiglass gets too visible
            //if (Environment.OSVersion.Version.Major >= 6)
            //{
            //    int value = 1;
            //    //DwmSetWindowAttribute(tocover.Handle, DWMWA_TRANSITIONS_FORCEDISABLED, ref value, 4);
            //}
        }
        
        public void modifySize(int tx, int ty, int bx, int by)
        {

            this.Size = new System.Drawing.Size(bx, by);
            this.Location = new Point(tx, ty);
            //this.SizeChanged() += 1;
            //this.Size = new Size(200, 200);

            
            //this.Show();
        }

        private void Cover_LocationChanged(object sender, EventArgs e)
        {
            // Ensure the plexiglass follows the owner
            this.Location = this.Owner.PointToScreen(Point.Empty);
        }
        private void Cover_ClientSizeChanged(object sender, EventArgs e)
        {
            // Ensure the plexiglass keeps the owner covered
            this.ClientSize = this.Owner.ClientSize;
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Restore owner

            //this.Owner.LocationChanged -= Cover_LocationChanged;
            //this.Owner.ClientSizeChanged -= Cover_ClientSizeChanged;
            //if (!this.Owner.IsDisposed && Environment.OSVersion.Version.Major >= 6)
            //{
            //    int value = 1;
            //    DwmSetWindowAttribute(this.Owner.Handle, DWMWA_TRANSITIONS_FORCEDISABLED, ref value, 4);
            //}
            //base.OnFormClosing(e);
        }
        protected override void OnActivated(EventArgs e)
        {
            // Always keep the owner activated instead
            //this.BeginInvoke(new Action(() => this.Owner.Activate()));
        }
        private const int DWMWA_TRANSITIONS_FORCEDISABLED = 3;
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hWnd, int attr, ref int value, int attrLen);

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Plexiglass
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "Plexiglass";
            this.Load += new System.EventHandler(this.Plexiglass_Load);
            this.ResumeLayout(false);

        }

        private void Plexiglass_Load(object sender, EventArgs e)
        {

        }
    }
}
