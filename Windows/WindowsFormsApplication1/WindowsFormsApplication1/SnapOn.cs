using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Timers;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class SnapOn
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


        //[DllImport(@"C:\Users\plaga\Documents\GitHub\SplitScreenWindows\HookDll")]
        //private static extern bool installHook();


        private delegate IntPtr LowLevelMouseProc(int nCode,
            IntPtr wParam, IntPtr lParam);

//        private static LowLevelMouseProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        static IntPtr hHook = IntPtr.Zero;
        private List<Plexiglass> tempPlexiArr = new List<Plexiglass>();
        private static System.Timers.Timer aTimer;
        private static System.Timers.Timer bTimer;
        private static System.Timers.Timer cTimer;
        private List<TemplateParse> tempParseArr = new List<TemplateParse>();
        private static IntPtr currentHandle;
        private static int x; // mouse x position
        private static int y; // mouse y position


        public SnapOn()
        {
            //tempParseArr = curTemp.getList();


            aTimer = new System.Timers.Timer(100);
            bTimer = new System.Timers.Timer(1000);
            cTimer = new System.Timers.Timer(100);
            aTimer.Elapsed += GrabMousePos;
            bTimer.Elapsed += WindowInPos;
            cTimer.Elapsed += hookTimer;



        }


        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
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

        public void Run(List<TemplateParse> arr)
        {
            tempParseArr = arr;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
            bTimer.AutoReset = true;
            bTimer.Enabled = true;
            cTimer.AutoReset = true;
            cTimer.Enabled = true;
            this.createPlexiGlass();
        }

        public void Halt()
        {
            aTimer.AutoReset = false;
            aTimer.Enabled = false;
            bTimer.AutoReset = false;
            bTimer.Enabled = false;
            cTimer.AutoReset = false;
            cTimer.Enabled = false;
        }

        public void RemovePlexi()
        {
            for (int i = 0; i < tempPlexiArr.Count; i++)
            {
                tempPlexiArr[i].Close();
            }
            tempPlexiArr.Clear();
        }

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

                for (int i = 0; i < tempParseArr.Count; i++)
                {
                    if (x > tempParseArr[i].getBotX() * 3 - 100 && y > tempParseArr[i].getBotY() * 3 - 100 && x < tempParseArr[i].getBotX() * 3 && y < tempParseArr[i].getBotY() * 3 && handle == currentHandle)
                    {
                        SetWindowPos(handle, 0, tempParseArr[i].getTopX() * 3, tempParseArr[i].getTopY() * 3, tempParseArr[i].getBotX() * 3 - tempParseArr[i].getTopX() * 3, tempParseArr[i].getBotY() * 3 - tempParseArr[i].getTopY() * 3,
                            SWP_NOZORDER | SWP_SHOWWINDOW);
                    }
                }
            }
        }

        private void GrabMousePos(object sender, ElapsedEventArgs e)
        {
            Point mousePoint = Control.MousePosition;
            //Debug.Print("{0} and {1}", mousePoint.X, mousePoint.Y);
            x = mousePoint.X;
            y = mousePoint.Y;
            //this.positioningText.Text = x + "  " + y;
            //throw new NotImplementedException();
        }

        private void createPlexiGlass()
        {
            for (int i = 0; i < tempPlexiArr.Count; i++)
            {
                tempPlexiArr[i].Close();
            }
            tempPlexiArr.Clear();
            //Debug.Print(selected[0].toString());
            for (int j = 0; j < tempParseArr.Count; j++)
            {
                            //Debug.Print(selected[j].toString());
                   
                   tempPlexiArr.Add(new Plexiglass(tempParseArr[j].getBotX() * 3 - 100, tempParseArr[j].getBotY() * 3 - 100));
             }
                    

          
        }
        
    }
}
