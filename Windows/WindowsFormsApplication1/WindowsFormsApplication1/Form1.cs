using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Input;


namespace WindowsFormsApplication1
{

    public partial class Form1 : Form
    {
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        //NOTE: DllImport attributes must be applied for each function, one for each function.
        // Sam's file path [DllImport(@"C:\GitProjects\SplitScreenWindows\HookDll\HookDLL.dll")]
        //[DllImport(@"C:\Users\plaga\Documents\GitHub\SplitScreenWindows\HookDll")]

        [DllImport(@"C:\GitProjects\SplitScreenWindows\HookDll\HookDLL.dll")]
            private static extern bool installHook();
        [DllImport(@"C:\GitProjects\SplitScreenWindows\HookDll\HookDLL.dll")]
            private static extern bool getMoving();
        // getX and getY  not functional at the moment. Will always return 0.
        [DllImport(@"C:\GitProjects\SplitScreenWindows\HookDll\HookDLL.dll")]
            private static extern int getX();
        [DllImport(@"C:\GitProjects\SplitScreenWindows\HookDll\HookDLL.dll")]
            private static extern int getY();

        private static int x;
        private static int y;
        private static System.Timers.Timer aTimer;
        private static System.Timers.Timer bTimer;
        private static bool hookCreated = false;
        
       // private int _mouseChange;


        public Form1()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            aTimer = new System.Timers.Timer(100);
            aTimer.Elapsed += GrabMousePos;

            bTimer = new System.Timers.Timer(1000);
            bTimer.Elapsed += WindowInPos;

            if (!hookCreated)
            {
                if (installHook())
                {
                    hookCreated = true;
                    Console.WriteLine("Hook installed successfully\n");
                }
            }

            //this.Cursor = new Cursor(Cursor.Current.Handle);
            if (this.button1.Text == "Run")
            {
                aTimer.AutoReset = true;
                aTimer.Enabled = true;

                bTimer.AutoReset = true;
                bTimer.Enabled = true;

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
                aTimer.AutoReset = false;
                aTimer.Enabled = false;
                button1.Text = "Run";
                

            }

            //



        }

        private void WindowInPos(object sender, ElapsedEventArgs e)
        {
                        
            const short SWP_NOSIZE = 1;
           // const short SWP_NOMOVE = 0x2;
            const short SWP_NOZORDER = 0x4;
            const int SWP_SHOWWINDOW = 0x0040;
            
            Process[] processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                //Console.WriteLine( process.ProcessName);

                IntPtr handle = process.MainWindowHandle;

                if (handle != IntPtr.Zero && x == 0)
                {
                    SetWindowPos(handle, 0, 0, 0, 0, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);
                }

            }



        }



        private void GrabMousePos(object sender, ElapsedEventArgs e)
        {
            Point mousePoint = MousePosition;
            Debug.Print("{0} and {1}" , mousePoint.X, mousePoint.Y);
            console.writeline("process name: ", process.processname);
            x = mousePoint.X;
            y = mousePoint.Y;
            //this.textBox1.Text = x + "  " + y;
            //throw new NotImplementedException();
        }
    }
}
