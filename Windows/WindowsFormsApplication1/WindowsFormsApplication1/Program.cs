﻿using System;
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
        public static int x = 0, y = 0;
        public static bool moving = false;
        internal static bool mouseDown = false;

        [STAThread]


        static void Main(string[] args)
        {
           // if (installHook()) Debug.Write("success");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Point mousepoint = Control.MousePosition;
            Application.Run(new CreationForm());
            Console.ReadLine();
            //MousePos mp = new MousePos();
        }
    }

}
