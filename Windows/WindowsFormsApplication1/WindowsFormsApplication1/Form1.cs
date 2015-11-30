using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Forms;
using Microsoft.Win32;


namespace WindowsFormsApplication1
{

    public partial class Form1 : Form
    {
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd,
            int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelMouseProc Ipfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        static extern IntPtr WindowFromPoint(POINT Point);

        //NOTE: DllImport attributes must be applied for each function, one for each function.
        // Sam's file path [DllImport(@"C:\GitProjects\SplitScreenWindows\HookDll\HookDLL.dll")]


        [DllImport(@"C:\Users\plaga\Documents\GitHub\SplitScreenWindows\HookDll")]
        private static extern bool installHook();

        /**
        [DllImport(@"C:\GitProjects\SplitScreenWindows\HookDll\HookDLL.dll")]
            private static extern bool installHook();
        [DllImport(@"C:\GitProjects\SplitScreenWindows\HookDll\HookDLL.dll")]
            private static extern bool getMoving();
        // getX and getY  not functional at the moment. Will always return 0.
        [DllImport(@"C:\GitProjects\SplitScreenWindows\HookDll\HookDLL.dll")]
            private static extern int getX();
        [DllImport(@"C:\GitProjects\SplitScreenWindows\HookDll\HookDLL.dll")]
            private static extern int getY()
        */

        private static string pathWallpaper;
        private static int x; // mouse x position
        private static int y; // mouse y position
        private static double height; // screen height resolution
        private static double width;  // screen width resolution
        private static System.Timers.Timer aTimer;
        private static System.Timers.Timer bTimer;
        private static System.Timers.Timer cTimer;
        // private MouseButtons mouseButton;
        //private static bool isMousePress = false;
        //private static bool hookCreated = false;
        private static LowLevelMouseProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        static IntPtr hHook = IntPtr.Zero;
        private static IntPtr currentHandle;
     

        private delegate IntPtr LowLevelMouseProc(int nCode,
            IntPtr wParam, IntPtr lParam);

       // private int _mouseChange;


        public Form1()
        {
            InitializeComponent();
            Rectangle rect = Screen.PrimaryScreen.WorkingArea;
            height = rect.Height;
            width = rect.Width;
            Debug.Print("{0} y {1}", height, width);
            aTimer = new System.Timers.Timer(100);
            bTimer = new System.Timers.Timer(1000);
            cTimer = new System.Timers.Timer(100);
            GetPathOfWallpaper();
            if(pathWallpaper != null)
                this.pictureBox1.ImageLocation = pathWallpaper;
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            //Debug.Print("here {0}", pathWallpaper);
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {


            aTimer.Elapsed += GrabMousePos;
            bTimer.Elapsed += WindowInPos;
            cTimer.Elapsed += hookTimer;


            /**if (!hookCreated)
            {
                if (installHook())
                {
                    hookCreated = true;
                    Console.WriteLine("Hooks installed successfully\n");
                }
            }
        */
            //if (IntPtr.Zero == hHook)
            //{
            //    using (Process curProcess = Process.GetCurrentProcess())
            //    using (ProcessModule curModule = curProcess.MainModule)
            //    {
            //        hHook = SetWindowsHookEx(WH_Mouse_LL, _proc,
            //            GetModuleHandle(curModule.ModuleName), 0);
            //    }
            //}



            // Cursor = new Cursor(Cursor.Current.Handle);
            if (this.button1.Text == "Run")
            {

                aTimer.AutoReset = true;
                aTimer.Enabled = true;

                bTimer.AutoReset = true;
                bTimer.Enabled = true;

                cTimer.AutoReset = true;
                cTimer.Enabled = true;

                button1.Text = "Stop";
            }
            else
            {
                Debug.Print("Stopped");
                aTimer.AutoReset = false;
                aTimer.Enabled = false;

                bTimer.AutoReset = false;
                bTimer.Enabled = false;

                cTimer.AutoReset = false;
                cTimer.Enabled = false;
                button1.Text = "Run";
            }

        }

        private void hookTimer(object sender, ElapsedEventArgs e)
        {
           // if(nCode >= 0 && MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam)
           // {
                POINT cusorPoint;
                bool ret = GetCursorPos(out cusorPoint);

                IntPtr winHandle = WindowFromPoint(cusorPoint);

                currentHandle = winHandle;

                //UnhookWindowsHookEx(hHook);
                //hHook = IntPtr.Zero;
           // }

        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if(nCode >= 0 && MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam)
            {
                POINT cusorPoint;
                bool ret = GetCursorPos(out cusorPoint);

                IntPtr winHandle = WindowFromPoint(cusorPoint);

                currentHandle = winHandle;

                UnhookWindowsHookEx(hHook);
                hHook = IntPtr.Zero;
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }


        private const int WH_Mouse_LL = 14;

        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        //private struct MSLLHOOKSTRUCT
        //{
        //    public POINT pt;
        //    public uint mouseData;
        //    public uint flags;
        //    public uint time;
        //    public IntPtr dwExtraInfo;
        //}


        private void WindowInPos(object sender, ElapsedEventArgs e)
        {
                        
           // const short SWP_NOSIZE = 1;
           // const short SWP_NOMOVE = 0x2;
            const short SWP_NOZORDER = 0x4;
            const int SWP_SHOWWINDOW = 0x0040;
            
            Process[] processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                //Console.WriteLine( process.ProcessName);

                IntPtr handle = process.MainWindowHandle;


                if (x == 0 && y == 0 && handle == currentHandle)
                {
                    SetWindowPos(handle, 0, 0, 0, 960, 540,
                        SWP_NOZORDER | SWP_SHOWWINDOW);
                }
                else if (x >= 1919 && y == 0 && handle == currentHandle)
                {
                    SetWindowPos(handle, 0, 960, 0, 960, 540,
                        SWP_NOZORDER | SWP_SHOWWINDOW);
                }
                else if (x >= 1919 && y >= 1036 && handle == currentHandle)
                {
                    SetWindowPos(handle, 0, 960, 540, 960, 540,
                        SWP_NOZORDER | SWP_SHOWWINDOW);
                }
                else if (x <= 2 && y >= 1036 && handle == currentHandle)
                {
                    SetWindowPos(handle, 0, 0, 540, 960, 540,
                        SWP_NOZORDER | SWP_SHOWWINDOW);
                }
                else if (x <= 2 && handle == currentHandle)
                {
                    SetWindowPos(handle, 0, 0, 0, 960, 1030,
                        SWP_NOZORDER | SWP_SHOWWINDOW);
                }
                else if (x >= 1919 && handle == currentHandle)
                {
                    SetWindowPos(handle, 0, 960, 0, 960, 1030,
                        SWP_NOZORDER | SWP_SHOWWINDOW);
                }

            }
        }



        private void GrabMousePos(object sender, ElapsedEventArgs e)
        {
            Point mousePoint = MousePosition;
            Debug.Print("{0} and {1}" , mousePoint.X, mousePoint.Y);
            //console.writeline("process name: ", process.processname);
            x = mousePoint.X;
            y = mousePoint.Y;
            //this.textBox1.Text = x + "  " + y;
            //throw new NotImplementedException();
        }

        private void GetPathOfWallpaper()
        {
            pathWallpaper = "";
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", false);
            
            if(regKey != null)
            {
                pathWallpaper = regKey.GetValue("WallPaper").ToString();
                regKey.Close();
            }
        }

    }
}
