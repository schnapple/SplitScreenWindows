using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    

    static class Program
    {

        //private static System.Timers.Timer aTimer;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]


        [DllImport(@"C:\GitProjects\SplitScreenWindows\HookDll\HookDLL.dll")]
        private static extern bool installHook(); 
        static void Main(string[] args)
        {
            if (installHook()) Debug.Write("success");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Point mousepoint = Control.MousePosition;
            Application.Run(new Form1());
            //aTimer = new System.Timers.Timer(1000);
            //aTimer.Elapsed += GrabMousePos;
            //aTimer.AutoReset = true;
            //aTimer.Enabled = true;
            Console.ReadLine();
            //MousePos mp = new MousePos();
        }


        private static void GrabMousePos(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}", e.SignalTime);
            //Debug.WriteLine("run");
            //Console.ReadLine();
            //Point mousepoint = Control.MousePosition;
            //Form1.button1.Text = mousepoint.Y + "   " + mousepoint.X;
            
        }


    }

}
