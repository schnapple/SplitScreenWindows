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
    /*
    The Templatr Parse Object
    */
    public struct TemplateParse
    {

        private int topX;
        private int topY;
        private int botX;
        private int botY;
        public TemplateParse(int tX, int tY, int bX, int bY)
        {
            topX = tX;
            topY = tY;
            botX = bX;
            botY = bY;
           // Graphics g;
        }

        public void resize(int tX, int tY, int bX, int bY)
        {
            topX = tX;
            topY = tY;
            botX = bX;
            botY = bY;
        }

        public int getTopX()
        {
            return topX;
        }

        public int getTopY()
        {
            return topY;
        }

        public int getBotX()
        {
            return botX;
        }

        public int getBotY()
        {
            return botY;
        }

        public String toString()
        {
            String val = topX.ToString() + ',' + topY.ToString();
            return val;
        }
    }

    public struct Template
    {
        private List<TemplateParse> tempParseArr;
        private String templateID;
        public Template(String ident, List<TemplateParse> templateList)
        {
            templateID = ident;
            tempParseArr = templateList;
        }

        public String getId()
        {
            return templateID;
        }

        public List<TemplateParse> getList()
        {
            return tempParseArr;
        }

    }




    /*
    This is where the form1 code begins
    */

    public partial class CreationForm : Form
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

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder text, int count);

        private static string pathWallpaper;
        private static int height; // screen height resolution
        private static int width;  // screen width resolution
        private bool running = false;
        private Image[] imageList;
        private int imageListIndex;
        private Image currentImage;
        //private Image origImage;
        //private Bitmap currBit;
        //private Image drawnImage;
        private static string template;
        // private MouseButtons mouseButton;
        //private static bool isMousePress = false;
        //private static bool hookCreated = false;
        //private static LowLevelMouseProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        static IntPtr hHook = IntPtr.Zero;
        private static IntPtr currentHandle;
        private List<TemplateParse> tempParseArr = new List<TemplateParse>();
        private List<Template> templateArr = new List<Template>();
        private List<Plexiglass> tempPlexiArr = new List<Plexiglass>();
        private static string customizeValOne; // the value for the first point of the square being customized
        private static int customizeValOneX; // the value for the first point's x position
        private static int customizeValOneY; //  the value for the first point's y position
        private static string customizeValTwo; // the value for the first point of the square being customized
        private static int customizeValTwoX; // the value for the first point's x position
        private static int customizeValTwoY; //  the value for the first point's y position
        private int tempParseId; // the current template parse ID
                                 // private Graphics gNew; // the newest graphics
        private List<Graphics> listGraphics;


        private delegate IntPtr LowLevelMouseProc(int nCode,
            IntPtr wParam, IntPtr lParam);

        // private int _mouseChange;

        public CreationForm()
        {
            InitializeComponent();
            string path = Directory.GetCurrentDirectory() + "\\templates.txt";
            //string path = Directory.GetCurrentDirectory() + "\\templatesInfo.txt";
            loadTemplates(path);

            //new Plexiglass();
            imageList = new Image[20];
            imageListIndex = 0;
            Rectangle rect = Screen.PrimaryScreen.WorkingArea;
            height = rect.Height;
            width = rect.Width;
            Debug.Print("{0} y {1}", width, height);
            this.pictureBox1.Height = height / 3;
            this.pictureBox1.Width = width / 3;
            GetPathOfWallpaper();
            //if(pathWallpaper != null)
            //    this.pictureBox1.ImageLocation = pathWallpaper;
            //this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            currentImage = Image.FromFile(pathWallpaper);
            currentImage = ResizeImage(currentImage, width / 3, height / 3);
            // pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);

            // origImage = Image.FromFile(pathWallpaper);
            // origImage = ResizeImage(currentImage, width / 3, height / 3);
            this.pictureBox1.Image = currentImage;
            imageList[imageListIndex] = currentImage;

            this.DoubleBuffered = true;
        }

        //WindowHook
        private const int WM_MOVING = 0x0216;
        private const int WM_EXITSIZEMOVE = 0x0232;
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            // Listen for operating system messages.
            switch (m.Msg)
            {
                case WM_MOVING:
                    Program.moving = true;
                    Debug.WriteLine("moving");
                    break;
                case WM_EXITSIZEMOVE:
                    Program.moving = false;
                    break;
            }
            base.WndProc(ref m);
        }

        //Mouse event handler
        private void MouseEvent(object sender, EventArgs e)
        {
            //get cursor pos, get handle, window in pos
            POINT cursorPoint;
            cursorPoint.x = Program.x;
            cursorPoint.y = Program.y;
            currentHandle = WindowFromPoint(cursorPoint);

            //old "WindowInPos" code
            if (Program.moving)
            {
                const short SWP_NOZORDER = 0x4;
                const int SWP_SHOWWINDOW = 0x0040;

                Process[] processes = Process.GetProcesses();
                foreach (var process in processes)
                {
                    //Console.WriteLine( process.ProcessName);

                    IntPtr handle = process.MainWindowHandle;

                    for (int i = 0; i < tempParseArr.Count; i++)
                    {
                        if (Program.x > tempParseArr[i].getBotX() * 3 - 100 && Program.y > tempParseArr[i].getBotY() * 3 - 100 && Program.x < tempParseArr[i].getBotX() * 3 && Program.y < tempParseArr[i].getBotY() * 3 && handle == currentHandle)
                        {
                            SetWindowPos(handle, 0, tempParseArr[i].getTopX() * 3, tempParseArr[i].getTopY() * 3, tempParseArr[i].getBotX() * 3 - tempParseArr[i].getTopX() * 3, tempParseArr[i].getBotY() * 3 - tempParseArr[i].getTopY() * 3,
                                SWP_NOZORDER | SWP_SHOWWINDOW);
                        }
                    }
                }
            }

            //Debug.WriteLine(currentHandle);
            //Debug.WriteLine("(" + Program.x.ToString() + "," + Program.y.ToString() + ")");
            System.Text.StringBuilder Buff = new System.Text.StringBuilder(256);
            if (GetWindowText(currentHandle, Buff, 256) > 0)
            {
                label2.Text = "Handle Title: " + Buff.ToString();
            }
            else
            {
                label2.Text = "Handle Title: ";
            }
            label1.Text = "CursorPos: (" + Program.x.ToString() + "," + Program.y.ToString() + ")";
            label3.Text = "Window Moving: " + Program.moving.ToString();
        }

        /**
        When the application starts up load all saved templates from templates.txt
        */
        private void loadTemplates(String path)
        {
            StreamReader sr = new StreamReader(path);
            String line = sr.ReadLine();
            String name = "";
            int lineAt;
            bool firstVal = true;
            int pointCounter;

            while (line != null)
            {
                pointCounter = 0;
                int tX = 0, tY = 0, bX = 0, bY = 0;
                while (!line.Equals("end"))
                {

                    lineAt = line.IndexOf('|');
                    if (firstVal)
                    {
                        firstVal = false;
                        name = line.Substring(0, lineAt);
                        templateList.Items.Add(name);
                    }
                    else
                    {
                        if (pointCounter % 4 == 0)
                        {
                            String val = line.Substring(0, lineAt);
                            Debug.Print(val + " 1");

                            tX = Int32.Parse(val);
                        }
                        else if (pointCounter % 4 == 1)
                        {
                            String val = line.Substring(0, lineAt);
                            Debug.Print(val + "2");

                            tY = Int32.Parse(val);
                        }
                        else if (pointCounter % 4 == 2)
                        {
                            String val = line.Substring(0, lineAt);
                            Debug.Print(val + "3");

                            bX = Int32.Parse(val);
                        }
                        else
                        {
                            String val = line.Substring(0, lineAt);
                            Debug.Print(val + "4");
                            bY = Int32.Parse(val);
                            tempParseArr.Add(new TemplateParse(tX, tY, bX, bY));
                            Debug.Print("FUCK");
                        }
                        pointCounter++;

                    }

                    line = line.Substring(lineAt + 1);
                }

                List<TemplateParse> newTempArr = new List<TemplateParse>();
                Debug.Print(name + tempParseArr.Count);
                for (int i = 0; i < tempParseArr.Count; i++)
                {
                    //Debug.Print(tempParseArr[i].toString());
                    newTempArr.Add(tempParseArr[i]);
                }
                templateArr.Add(new Template(name, newTempArr));
                tempParseArr.Clear();
                line = sr.ReadLine();
                firstVal = true;
            }
            sr.Close();
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
            if (this.snappingButton.Text == "Run")
            {
                running = true;
                MouseHook.Start();
                MouseHook.MouseAction += new EventHandler(MouseEvent);
                snappingButton.Text = "Stop";
            }
            else
            {
                Debug.Print("Stopped");
                running = false;
                MouseHook.stop();
                MouseHook.MouseAction -= new EventHandler(MouseEvent);
                snappingButton.Text = "Run";
            }
        }


        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        private void GetPathOfWallpaper()
        {
            pathWallpaper = "";
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", false);

            if (regKey != null)
            {
                pathWallpaper = regKey.GetValue("WallPaper").ToString();
                regKey.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.pictureBox1.ContextMenu = new ContextMenu();
        }

        Rectangle rec;

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.Red, 2))
            {
                e.Graphics.DrawRectangle(pen, rec);
            }

            if (tempParseArr.Count == 0)
            {
                e.Graphics.Save();
            }
            else
            {
                Pen pen1;
                SolidBrush brush1;
                if (tempParseArr != null)
                {
                    //foreach (TemplateParse template in tempParseArr)
                    for (int i = 0; i < tempParseArr.Count; i++)
                    {
                        //tempParseArr.Add(new TemplateParse(tempParseId, customizeValOneX, customizeValOneY, customizeValTwoX, customizeValTwoY));
                        //gNew = Graphics.FromImage(currentImage);
                        pen1 = new Pen(Color.Red, 5);
                        Debug.Print("{0} and {1}", (tempParseArr[i]).getTopX(), (tempParseArr[i]).getTopY());
                        e.Graphics.DrawRectangle(pen1, (tempParseArr[i]).getTopX(), (tempParseArr[i]).getTopY(), (tempParseArr[i]).getBotX() - (tempParseArr[i]).getTopX(), (tempParseArr[i]).getBotY() - (tempParseArr[i]).getTopY());
                        e.Graphics.Save();
                        pen1 = new Pen(Color.Green, 5);
                        brush1 = new SolidBrush(Color.Green);
                        //gCur.DrawRectangle(pen1, (tempParseArr[i]).getBotX() - 50, (tempParseArr[i]).getBotY() - 50, 50, 50);
                        e.Graphics.FillRectangle(brush1, (tempParseArr[i]).getBotX() - 20, (tempParseArr[i]).getBotY() - 20, 20, 20);
                    }
                }
                //tempParseArr.Add(new TemplateParse(tempParseId, customizeValOneX, customizeValOneY, customizeValTwoX, customizeValTwoY));
                //gNew = Graphics.FromImage(currentImage);
                pen1 = new Pen(Color.Black, 2);
                //Debug.Print("{0} and {1}", e.X * 3, e.Y * 3);
                e.Graphics.DrawRectangle(pen1, rec);// customizeValOneX, customizeValOneY, customizeValTwoX - customizeValOneX, customizeValTwoY - customizeValOneY);
                e.Graphics.Save();
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                rec.Width = e.X - rec.X;
                rec.Height = e.Y - rec.Y;
                pictureBox1.Invalidate();
                //Debug.WriteLine(rec.Height + "," + rec.Width);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                rec = new Rectangle(e.X, e.Y, 0, 0);
                pictureBox1.Invalidate();
                // Debug.WriteLine("New Rectangle");
            }
        }

        /**
        ______________________________________________________________________________________________________
            This is the event handler for the list of templates that are currently made. 
            When an index is change the picture box will change to represent the template.
            __________________________________________________________________________________________________
        */
        private void templateListSelectedIndexChanged(object sender, EventArgs e)
        {
            tempParseArr.Clear();
            for (int i = 0; i < tempPlexiArr.Count; i++)
            {
                tempPlexiArr[i].Close();
            }
            tempPlexiArr.Clear();
            try
            {
                this.positioningText.Text = templateList.SelectedItem.ToString();
                template = templateList.SelectedItem.ToString();
                this.pictureBox1.Visible = true;
                String templateId = templateList.SelectedItem.ToString();

                for (int i = 0; i < templateArr.Count; i++)
                {

                    if (templateArr[i].getId().Equals(templateId))
                    {
                        //Debug.Print("In");
                        List<TemplateParse> selected = templateArr[i].getList();
                        Debug.Print(templateArr[i].getId());
                        Debug.Print(templateArr[i].getList().Count.ToString());
                        //Debug.Print(selected[0].toString());
                        for (int j = 0; j < selected.Count; j++)
                        {
                            Debug.Print(selected[j].toString());
                            tempParseArr.Add(selected[j]);
                            tempPlexiArr.Add(new Plexiglass(selected[j].getBotX() * 3 - 100, selected[j].getBotY() * 3 - 100));
                        }
                        //tempParseArr = templateArr[i].getList();
                        //Debug.Print(tempParseArr[0].toString());
                        pictureBox1.Update();
                        pictureBox1.Refresh();
                    }

                }
            }
            catch (NullReferenceException exe)
            {
                Console.WriteLine("Exception " + exe.Message);
            }
        }



        /**
        This will move the left side of the current templateBox when the numeric up down is changed
        */
        private void firstXCoorScroller_ValueChanged(object sender, EventArgs e)
        {
            customizeValOneX = Decimal.ToInt32(firstXCoorScroller.Value) / 3;
            pictureBox1.Update();
            pictureBox1.Refresh();

        }

        /**
        This will move the top side of the current templateBox when the numeric up down is changed
        */
        private void firstYCoorScroller_ValueChanged(object sender, EventArgs e)
        {
            customizeValOneY = Decimal.ToInt32(firstYCoorScroller.Value) / 3;
            pictureBox1.Update();
            pictureBox1.Refresh();
        }

        /**
        This will move the right side of the current templateBox when the numeric up down is changed
        */
        private void secondXCoorScroller_ValueChanged(object sender, EventArgs e)
        {
            customizeValTwoX = Decimal.ToInt32(secondXCoorScroller.Value) / 3;
            pictureBox1.Update();
            pictureBox1.Refresh();
        }

        /**
        This will move the bottom side of the current templateBox when the numeric up down is changed
        */
        private void secondYCoorScroller_ValueChanged(object sender, EventArgs e)
        {
            customizeValTwoY = Decimal.ToInt32(secondYCoorScroller.Value) / 3;
            pictureBox1.Update();
            pictureBox1.Refresh();
        }

        private void confirmationButton_Click(object sender, EventArgs e)
        {
            tempParseArr.Add(new TemplateParse(customizeValOneX, customizeValOneY, customizeValTwoX, customizeValTwoY));

            pictureBox1.Refresh();
            pictureBox1.Update();

        }

        private void templateConfirmationButton_Click(object sender, EventArgs e)
        {
            //String savedTemplateString;
            var result = MessageBox.Show("Do you want to save this template", "Save Template", MessageBoxButtons.YesNoCancel);
            Template savedTemplate;
            int randomNuber = new Random().Next(int.MinValue, int.MaxValue);
            String newLine = "helloworld" + randomNuber;
            if (result == DialogResult.Yes)
            {
                templateList.Items.Add(newLine);
                Debug.Print("HHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH");
                savedTemplate = new Template(newLine, tempParseArr);
                templateArr.Add(savedTemplate);
                try
                {
                    string path = Directory.GetCurrentDirectory() + "\\templates.txt";
                    StreamWriter sw = new StreamWriter(path);
                    for (int i = 0; i < tempParseArr.Count; i++)
                    {
                        newLine = newLine + "|" + tempParseArr[i].getTopX() + "|" + tempParseArr[i].getTopY() + "|" + tempParseArr[i].getBotX() + "|" + tempParseArr[i].getBotY();
                    }
                    newLine = newLine + "|end";

                    sw.WriteLine(newLine);
                    sw.Close();
                }
                catch (Exception exe)
                {
                    Console.WriteLine("Exception::::: " + exe.Message);
                }
            }
        }
    }

    //Global Mouse hook handler
    //mouse event function can be found in class CreationForm
    public static class MouseHook
    {
        public static event EventHandler MouseAction = delegate { };

        public static void Start()
        {
            _hookID = SetHook(_proc);


        }
        public static void stop()
        {
            UnhookWindowsHookEx(_hookID);
        }

        private static LowLevelMouseProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        private static IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc,
                  GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && MouseMessages.WM_MOUSEMOVE == (MouseMessages)wParam)
            {
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                MouseAction(null, new EventArgs());
                Program.x = hookStruct.pt.x;
                Program.y = hookStruct.pt.y;
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private const int WH_MOUSE_LL = 14;

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

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
          LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
          IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);


    }

}
