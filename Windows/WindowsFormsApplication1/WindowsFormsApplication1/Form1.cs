using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Forms;


namespace WindowsFormsApplication1
{

    public partial class Form1 : Form
    {
        //[DllImport("user32.dll", EntryPoint = "SetWindowPos")]

        private static int x;
        private static int y;
        private static System.Timers.Timer aTimer;
        private static System.Timers.Timer bTimer;
        //public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
       // private int _mouseChange;


        public Form1()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            aTimer = new System.Timers.Timer(100);
            aTimer.Elapsed += GrabMousePos;

            //bTimer = new System.Timers.Timer(100);
            //bTimer.Elapsed += WindowInPos;



            //this.Cursor = new Cursor(Cursor.Current.Handle);
            if (this.button1.Text == "Run")
            {
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
                button1.Text = "Stop";
                //while (true)
                //{
                //Point mousePoint = MousePosition;
                //int x = mousePoint.X;
                //int y = mousePoint.Y;
                ////    Console.WriteLine(x + "   " + y);
                ////    Console.Out.WriteLine(x + "   " + y);
                //this.button1.Text = x + "   " + y;
                // }
            }
            else
            {
                aTimer.AutoReset = false;
                aTimer.Enabled = false;
                button1.Text = "Run";
                

            }
            //const short swp_nosize = 1;
            //const short swp_nomove = 0x2;
            //const short swp_nozorder = 0x4;
            //const int swp_showwindow = 0x0040;

            //process[] processes = process.getprocesses();

            //foreach (var process in processes)
            //{
            //    console.writeline("process name: ", process.processname);

            //    intptr handle = process.mainwindowhandle;
            //    if (handle != intptr.zero)
            //    {
            //        setwindowpos(handle, 0, 0, 0, 0, 0, swp_nozorder | swp_nosize | swp_showwindow);
            //    }

            //}

        }

        private void WindowInPos(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }



        private void GrabMousePos(object sender, ElapsedEventArgs e)
        {
        Point mousePoint = MousePosition;
            Debug.Print("{0} and {1}" , mousePoint.X, mousePoint.Y);
            
            x = mousePoint.X;
            y = mousePoint.Y;
            //this.textBox1.Text = x + "  " + y;
            //throw new NotImplementedException();
        }
    }
}
