using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Timers;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
//using UserActivityMonitor.GlobalEventProvider;

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
        private static System.Timers.Timer plexiTimer;
        private List<TemplateParse> tempParseArr = new List<TemplateParse>();
        private static IntPtr currentHandle;
        private static int x; // mouse x position
        private static int y; // mouse y position
        private static bool leftButtonState = false;
        //private static Plexiglass overlayPlex;
        //private int overlayPlexTX;
        //private int overlayPlexTY;
        //private int overlayPlexBX;
        //private int overlayPlexBY;
        //private int overlayPlexCounterX = 150;
        //private int overlayPlexCounterY = 150;
        private IntPtr handle;
        private TemplateParse curTempParse;
        private bool location = false;

        //private static MouseHook mHook = new MouseHook();


        public SnapOn()
        {
            //tempParseArr = curTemp.getList();


            //aTimer = new System.Timers.Timer(100);
            //bTimer = new System.Timers.Timer(1000);
            //cTimer = new System.Timers.Timer(100);
            //plexiTimer = new System.Timers.Timer(.01);
            //aTimer.Elapsed += GrabMousePos;
            //bTimer.Elapsed += WindowInPos;
            //cTimer.Elapsed += hookTimer;
            //plexiTimer.Elapsed += plexiGrow;
            //MouseHook.Start();
            MouseHook.Start();
            MouseHook.MouseUp += new EventHandler(mouseUp);
            MouseHook.MouseDown += new EventHandler(mouseDown);
            MouseHook.MouseMove += new EventHandler(mouseMove);
            //MouseHook.MouseHover += new EventHandler(mouseHover);
            //MouseHookTwo.Start();
            //MouseHookTwo.MouseAction += new EventHandler(mouseUp);
            //Console.WriteLine("started Mouse");

            //MouseEventArgs mouseE = (MouseEventArgs)e;

        }

        //private void mouseHover(object sender, EventArgs e)
        //{
        //    //POINT cusorPoint;
        //    //bool ret = GetCursorPos(out cusorPoint);
        //    //IntPtr winHandle = WindowFromPoint(cusorPoint);
        //    //currentHandle = winHandle;

        //    //Console.WriteLine(currentHandle.ToString());
        //}

        private void mouseMove(object sender, EventArgs e)
        {
            Point mousePoint = Control.MousePosition;
            x = mousePoint.X;
            y = mousePoint.Y;

            

            for (int i = 0; i < tempParseArr.Count; i++)
            {
                if (leftButtonState == true)
                {
                    if (x > tempParseArr[i].getBotX() * 3 - 80 && y > tempParseArr[i].getBotY() * 3 - 80
                        && x < tempParseArr[i].getBotX() * 3 && y < tempParseArr[i].getBotY() * 3)
                    {
                        curTempParse = tempParseArr[i];
                        location = true;
                        Console.WriteLine("location true");
                        break;
                    }
                    else
                    {
                        location = false;
                        Console.WriteLine("location false");
                    }
                }
            }
        }

        private void mouseUp(object sender, EventArgs e)
        {
            const short SWP_NOZORDER = 0x4;
            const int SWP_SHOWWINDOW = 0x0040;
            const int SWP_DRAWFRAME = 0x0020;

            Console.WriteLine("Left mouse up!");

            if (location == true)
            {
                Process[] processes = Process.GetProcesses();
                foreach (var process in processes)
                {
                    Console.WriteLine(process.ProcessName);

                    handle = process.MainWindowHandle;
                        Console.WriteLine("ehrherhe");
                        SetWindowPos(currentHandle, 0, curTempParse.getTopX() * 3, curTempParse.getTopY() * 3, curTempParse.getBotX() * 3
                            - curTempParse.getTopX() * 3, curTempParse.getBotY() * 3 - curTempParse.getTopY() * 3,
                            SWP_DRAWFRAME | SWP_SHOWWINDOW | SWP_NOZORDER);

                    location = false;
                }
            }
            leftButtonState = false;
        }

        private void mouseDown(object sender, EventArgs e)
        {
            POINT cusorPoint;
            bool ret = GetCursorPos(out cusorPoint);
            IntPtr winHandle = WindowFromPoint(cusorPoint);
            currentHandle = winHandle;
            Console.WriteLine("Left mouse click!");
            leftButtonState = true;
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
            //aTimer.AutoReset = true;
            //aTimer.Enabled = true;
            //bTimer.AutoReset = true;
            //bTimer.Enabled = true;
            //cTimer.AutoReset = true;
            //cTimer.Enabled = true;
            this.createPlexiGlass();
        }

        public void Halt()
        {
            //aTimer.AutoReset = false;
            //aTimer.Enabled = false;
            //bTimer.AutoReset = false;
            //bTimer.Enabled = false;
            //cTimer.AutoReset = false;
            //cTimer.Enabled = false;
            MouseHook.stop();
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
            //const short SWP_NOZORDER = 0x4;
            //const int SWP_SHOWWINDOW = 0x0040;

            //Process[] processes = Process.GetProcesses();
            //foreach (var process in processes)
            //{
                //Console.WriteLine( process.ProcessName);

                //handle = process.MainWindowHandle;

            for (int i = 0; i < tempParseArr.Count; i++)
            {
                if (leftButtonState == true)
                {
                    if (x > tempParseArr[i].getBotX() * 3 - 80 && y > tempParseArr[i].getBotY() * 3 - 80
                        && x < tempParseArr[i].getBotX() * 3 && y < tempParseArr[i].getBotY() * 3)
                    {
                        curTempParse = tempParseArr[i];
                        Console.WriteLine("Location is true");
                        //SetWindowPos(handle, 0, tempParseArr[i].getTopX() * 3, tempParseArr[i].getTopY() * 3, tempParseArr[i].getBotX() * 3 
                        //    - tempParseArr[i].getTopX() * 3, tempParseArr[i].getBotY() * 3 - tempParseArr[i].getTopY() * 3,
                        //    SWP_NOZORDER | SWP_SHOWWINDOW);
                        location = true;
                        //makePlexiOverlay(tempParseArr[i], handle);
                    }
                        //else
                        //{
                        //    location = false;
                        //}
                    }
                    //}
            }
        }




        /**
            Hold on with this until you fix the mouse hook issues
        */
        private void makePlexiOverlay(TemplateParse tempParse, IntPtr handle)
        {
            //nsole.WriteLine("HERHERHE");
            //bTimer.AutoReset = false;
            //bTimer.Enabled = false;
            //const short SWP_NOZORDER = 0x4;
            //const int SWP_SHOWWINDOW = 0x0040;



            //overlayPlexBX = tempParse.getBotX() * 3;
            //overlayPlexBY = tempParse.getBotY() * 3;
            //overlayPlexTX = tempParse.getTopX() * 3;
            //overlayPlexTY = tempParse.getTopY() * 3;
            //overlayPlex = new Plexiglass(overlayPlexBX - 300, overlayPlexBY - 300, 0, 0);
            //plexiTimer.AutoReset = true;
            //plexiTimer.Enabled = true;
            //while(i != tempParse.getTopX() && j != tempParse.getTopY())
            //{

            //}



            //for(int i = temp)
            //throw new NotImplementedException();
        }


        private void plexiGrow(object sender, ElapsedEventArgs e)
        {
            //overlayPlex.Close();
            //if (overlayPlexBX - overlayPlexCounterX >= overlayPlexTX && overlayPlexTY <= overlayPlexBY - overlayPlexCounterY)
            //{
            //    overlayPlexCounterX += 8;
            //    overlayPlexCounterY += 8;
            //    overlayPlex.modifySize(overlayPlexBX - overlayPlexCounterX, overlayPlexBY - overlayPlexCounterY,
            //        overlayPlexBX - (overlayPlexBX - overlayPlexCounterX), overlayPlexBY - (overlayPlexBY - overlayPlexCounterY));

            //}
            //else if (overlayPlexBX - overlayPlexCounterX >= overlayPlexTX)
            //{
            //    overlayPlexCounterX += 8;
            //    overlayPlex.modifySize(overlayPlexBX - overlayPlexCounterX, overlayPlexTY,
            //        overlayPlexBX - (overlayPlexBX - overlayPlexCounterX), overlayPlexBY - overlayPlexTY);
            //}
            //else if (overlayPlexBY - overlayPlexCounterY >= overlayPlexTY)
            //{
            //    overlayPlexCounterY += 8;
            //    overlayPlex.modifySize(overlayPlexTX, overlayPlexBY - overlayPlexCounterY,
            //        overlayPlexBX - overlayPlexTY, overlayPlexBY - (overlayPlexBY - overlayPlexCounterY));
            //}
            //else
            //{
            //    plexiTimer.AutoReset = false;
            //    plexiTimer.Enabled = false;
            //}


        }

        //private void GrabMousePos(object sender, ElapsedEventArgs e)
        //{
        //    Point mousePoint = Control.MousePosition;

        //    Debug.Print("{0} and {1}", mousePoint.X, mousePoint.Y);

        //    this.positioningText.Text = x + "  " + y;
        //    throw new NotImplementedException();
        //}

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
                   
                   tempPlexiArr.Add(new Plexiglass(tempParseArr[j].getBotX() * 3 - 80, tempParseArr[j].getBotY() * 3 - 80,80,80));
             }
                    

          
        }
        
    }
}
