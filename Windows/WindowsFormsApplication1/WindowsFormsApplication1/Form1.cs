using System;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections.Generic;

/**
    Philip LaGambino
    SplitScreen
*/


namespace WindowsFormsApplication1
{

    public struct TemplateParse
    {

        private int id;
        private int topX;
        private int topY;
        private int botX;
        private int botY;
        public TemplateParse(int iD, int tX, int tY, int bX, int bY)
        {
            id = iD;
            topX = tX;
            topY = tY;
            botX = bX;
            botY = bY;
        }

        public void move()
        {

        }

        public void resize()
        {

        }
    }

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
        private static int height; // screen height resolution
        private static int width;  // screen width resolution
        private bool draw;
        private Image origImage;
        private Image drawnImage;
        private static string template;
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
        private List<TemplateParse> tempParseArr = new List<TemplateParse>();
        private int tempParseId;
     

        private delegate IntPtr LowLevelMouseProc(int nCode,
            IntPtr wParam, IntPtr lParam);

       // private int _mouseChange;


        
        /**
        
        */
        public Form1()
        {
            InitializeComponent();
            string path = Directory.GetCurrentDirectory() + "\\templates.txt";
            Debug.Print("{0}", Directory.GetCurrentDirectory());
            StreamReader sr = new StreamReader(path);
            String line = sr.ReadLine();
            String name;
            int lineAt;
            tempParseId = 0; // *************************** THIS LINE IS TEMPORARY  *************************
            while (line != null)
            {
                Debug.Print("{0}", line);
                lineAt = line.IndexOf('|');
                name = line.Substring(0, lineAt);
                if(line != null)
                    this.listBox1.Items.Add(name);
                line = sr.ReadLine();
                
            }

            Rectangle rect = Screen.PrimaryScreen.WorkingArea;
            height = rect.Height;
            width = rect.Width;
            Debug.Print("{0} y {1}", width, height);
            this.pictureBox1.Height = height/3;
            this.pictureBox1.Width = width/3;
            aTimer = new System.Timers.Timer(100);
            bTimer = new System.Timers.Timer(1000);
            cTimer = new System.Timers.Timer(100);
            GetPathOfWallpaper();
            //if(pathWallpaper != null)
            //    this.pictureBox1.ImageLocation = pathWallpaper;
            //this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            origImage = Image.FromFile(pathWallpaper);
            origImage = ResizeImage(origImage, width/3, height/3);
            this.pictureBox1.Image = origImage;
            //this.pictureBox1.
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            aTimer.Elapsed += GrabMousePos;
            bTimer.Elapsed += WindowInPos;
            cTimer.Elapsed += hookTimer;
            /*
            if (!hookCreated)
            {
                if (installHook())
                {
                    hookCreated = true;
                    Console.WriteLine("Hooks installed successfully\n");
                }
            }
            if (IntPtr.Zero == hHook)
            {
                using (Process curProcess = Process.GetCurrentProcess())
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    hHook = SetWindowsHookEx(WH_Mouse_LL, _proc,
                        GetModuleHandle(curModule.ModuleName), 0);
                }
            }

            Cursor = new Cursor(Cursor.Current.Handle);
            */
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

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.pictureBox1.ContextMenu = new ContextMenu();
            this.pictureBox1.MouseDown += new MouseEventHandler(pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new MouseEventHandler(pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new MouseEventHandler(pictureBox1_MouseUp);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Debug.Print("mouse is down on picture box");
           
            Graphics g;
           
                //Debug.Print("on picture box");
            draw = true;
            tempParseArr.Add(new TemplateParse(tempParseId, e.X*3, e.Y*3, e.X*3 + 120, e.Y*3 + 120));
            tempParseId++;
            g = Graphics.FromImage(pictureBox1.Image);
            Pen pen1 = new Pen(Color.Red, 5);
            Debug.Print("{0} and {1}", e.X*3, e.Y*3);
            g.DrawRectangle(pen1, e.X, e.Y, 40, 40);
            g.Save();
            pictureBox1.Image = origImage;
            
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            draw = false;
        }



        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            
            foreach (TemplateParse currentTemp in tempParseArr)
            {

            }
            
            //if (draw)
            //{
            //    Debug.Print("mouse is down and moving on picture box");
            //    Graphics g = Graphics.FromImage(origImage);
            //    SolidBrush brush = new SolidBrush(Color.Red);
            //    g.FillRectangle(brush, e.X, e.Y, 20, 20);
            //    g.Save();
            //    pictureBox1.Image = origImage;
            //}
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textBox1.Text = listBox1.SelectedItem.ToString();
            template = listBox1.SelectedItem.ToString();
        }
    }
}
