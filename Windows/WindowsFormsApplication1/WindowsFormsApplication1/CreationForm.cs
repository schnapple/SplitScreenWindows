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
        private Image[] imageList;
        private int imageListIndex;
        private Image currentImage;
        //private Image origImage;
        //private Bitmap currBit;
        //private Image drawnImage;
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


        
        /**
        
        */
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
            this.pictureBox1.Height = height/3;
            this.pictureBox1.Width = width/3;
            aTimer = new System.Timers.Timer(100);
            bTimer = new System.Timers.Timer(1000);
            cTimer = new System.Timers.Timer(100);
            GetPathOfWallpaper();
            //if(pathWallpaper != null)
            //    this.pictureBox1.ImageLocation = pathWallpaper;
            //this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            currentImage = Image.FromFile(pathWallpaper);
            currentImage = ResizeImage(currentImage, width/3, height/3);
           // pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            
           // origImage = Image.FromFile(pathWallpaper);
           // origImage = ResizeImage(currentImage, width / 3, height / 3);
            this.pictureBox1.Image = currentImage;
            imageList[imageListIndex] = currentImage;
            //this.pictureBox1.
            this.DoubleBuffered = true;
        }
        Rectangle rec = new Rectangle(0, 0, 0, 0);

        


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
                    
                    //bool snapBox = false;
                    
                    lineAt = line.IndexOf('|');
                    if (firstVal)
                    {
                        firstVal = false;
                        name = line.Substring(0, lineAt);
                        templateList.Items.Add(name);
                    }
                    else
                    {
                        if(pointCounter % 4 == 0)
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
                        pointCounter ++;

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
            aTimer.Elapsed += GrabMousePos;
            bTimer.Elapsed += WindowInPos;
            cTimer.Elapsed += hookTimer;
           

            if (this.snappingButton.Text == "Run")
            {

                aTimer.AutoReset = true;
                aTimer.Enabled = true;

                bTimer.AutoReset = true;
                bTimer.Enabled = true;

                cTimer.AutoReset = true;
                cTimer.Enabled = true;

                snappingButton.Text = "Stop";
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
                snappingButton.Text = "Run";
            }

        }



        private void customizationButtonClick(object sender, EventArgs e)
        {
            this.pictureBox1.Visible = true;
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
                    if(x > tempParseArr[i].getBotX() * 3 - 100 && y > tempParseArr[i].getBotY() * 3 - 100 && x < tempParseArr[i].getBotX()*3 && y < tempParseArr[i].getBotY()*3 && handle == currentHandle)
                    {
                        SetWindowPos(handle, 0, tempParseArr[i].getTopX() * 3, tempParseArr[i].getTopY() * 3, tempParseArr[i].getBotX() * 3 - tempParseArr[i].getTopX() * 3, tempParseArr[i].getBotY() * 3 - tempParseArr[i].getTopY() * 3,
                            SWP_NOZORDER | SWP_SHOWWINDOW);
                    }
                }
            }
        }



        /**
        This will grab the position of the mouse when the run button is clicked
        */
        private void GrabMousePos(object sender, ElapsedEventArgs e)
        {
            Point mousePoint = MousePosition;
            Debug.Print("{0} and {1}" , mousePoint.X, mousePoint.Y);
            x = mousePoint.X;
            y = mousePoint.Y;
            //this.positioningText.Text = x + "  " + y;
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







        /*
        ***********************************************************************
        *           __           ___  ____                                    *
        *   |\  /| /   \ |   |  /    |                                        *
        *   | \/ | |   | |   |  |__  |____                                    *
        *   |    | |   | |   |     \ |                                        *
        *   |    | \___/  \__/  ___/ |____                                    *
        *                                                                     *
        ***********************************************************************
        */
        //private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        //{
        //    Debug.Print("mouse is down on picture box");


        //    // for the first click
        //    if (customizeValOne == null)
        //    {
        //        customizeValOne = e.X * 3 + "," + e.Y * 3;
        //        customizeValOneX = e.X;
        //        customizeValOneY = e.Y;
        //        positionText.Text = customizeValOne;
        //        firstXCoorScroller.Value = customizeValOneX * 3;
        //        firstYCoorScroller.Value = customizeValOneY * 3;

        //    }
        //    // for the second click
        //    else if(customizeValTwo == null)
        //    {
        //        customizeValTwo = e.X * 3 + "," + e.Y * 3;
        //        customizeValTwoX = e.X;
        //        customizeValTwoY = e.Y;
        //        positionText.Text = customizeValOne + " and " + customizeValTwo;
        //        secondXCoorScroller.Value = customizeValTwoX * 3;
        //        secondYCoorScroller.Value = customizeValTwoY * 3;
        //        pictureBox1.Refresh();
        //        // draw the box on the current

        //    }
        //    else
        //    {
        //        customizeValOne = e.X * 3 + "," + e.Y * 3;
        //        customizeValOneX = e.X;
        //        customizeValOneY = e.Y;
        //        customizeValTwo = null;
        //        firstXCoorScroller.Value = customizeValOneX * 3;
        //        firstYCoorScroller.Value = customizeValOneY * 3;
        //        secondXCoorScroller.Value = 0;
        //        secondYCoorScroller.Value = 0;
        //        positionText.Text = customizeValOne;
        //    }
        //}



        //private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        //{
        //    draw = false;
        //}


        /**
        This method is currently not being used
        */
        private void Form1_Load(object sender, EventArgs e)
        {
            this.pictureBox1.ContextMenu = new ContextMenu();
            this.pictureBox1.MouseDown += new MouseEventHandler(OnMouseDown);
            this.pictureBox1.MouseMove += new MouseEventHandler(OnMouseMove);
            // this.pictureBox1.MouseUp += new MouseEventHandler(pictureBox1_MouseUp);
            this.pictureBox1.Paint += new PaintEventHandler(OnPaint);
        }
        protected void OnPaint(object sender, PaintEventArgs e)
        {
            Pen pen1 = new Pen(Color.Red, 1);
            e.Graphics.DrawRectangle(pen1, rec);
            Debug.WriteLine("drawing");
        }
        protected void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                rec = new Rectangle(e.X, e.Y, 0, 0);
                Invalidate();
                Debug.WriteLine("mouse down");
            }
        }
        protected void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                rec.Width = e.X - rec.X;
                rec.Height = e.Y - rec.Y;
                Invalidate();
                Debug.WriteLine("moving");
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
            for(int i = 0; i < tempPlexiArr.Count; i++)
            {
                tempPlexiArr[i].Close();
            }
            tempPlexiArr.Clear();
            try {
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
                        for(int j = 0; j < selected.Count; j++)
                        {
                            Debug.Print(selected[j].toString());
                            tempParseArr.Add(selected[j]);
                            tempPlexiArr.Add(new Plexiglass(selected[j].getBotX()*3-100, selected[j].getBotY()*3-100));
                        }
                        //tempParseArr = templateArr[i].getList();
                        //Debug.Print(tempParseArr[0].toString());
                        pictureBox1.Update();
                        pictureBox1.Refresh();
                    }

                }
            }
            catch(NullReferenceException exe)
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




        private void pictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (tempParseArr.Count == 0)
            {
                Graphics gCur = e.Graphics;
                draw = true;
                //tempParseArr.Add(new TemplateParse(tempParseId, customizeValOneX, customizeValOneY, customizeValTwoX, customizeValTwoY));
                //gNew = Graphics.FromImage(currentImage);
                Pen pen1 = new Pen(Color.Red, 1);
                //Debug.Print("{0} and {1}", e.X * 3, e.Y * 3);
                gCur.DrawRectangle(pen1, customizeValOneX, customizeValOneY, customizeValTwoX - customizeValOneX, customizeValTwoY - customizeValOneY);
                gCur.Save();
            }
            else
            {
                Graphics gCur;
                Pen pen1;
                SolidBrush brush1;
                if(tempParseArr != null) {
                    //foreach (TemplateParse template in tempParseArr)
                    for (int i = 0; i < tempParseArr.Count; i++ )
                    {
                        gCur = e.Graphics;
                        draw = true;
                        //tempParseArr.Add(new TemplateParse(tempParseId, customizeValOneX, customizeValOneY, customizeValTwoX, customizeValTwoY));
                        //gNew = Graphics.FromImage(currentImage);
                        pen1 = new Pen(Color.Red, 5);
                        Debug.Print("{0} and {1}", (tempParseArr[i]).getTopX(), (tempParseArr[i]).getTopY());
                        gCur.DrawRectangle(pen1, (tempParseArr[i]).getTopX(), (tempParseArr[i]).getTopY(), (tempParseArr[i]).getBotX() - (tempParseArr[i]).getTopX(), (tempParseArr[i]).getBotY() - (tempParseArr[i]).getTopY());
                        gCur.Save();
                        pen1 = new Pen(Color.Green, 5);
                        brush1 = new SolidBrush(Color.Green);
                        //gCur.DrawRectangle(pen1, (tempParseArr[i]).getBotX() - 50, (tempParseArr[i]).getBotY() - 50, 50, 50);
                        gCur.FillRectangle(brush1, (tempParseArr[i]).getBotX() - 20, (tempParseArr[i]).getBotY() - 20, 20, 20);
                    }
                }
                gCur = e.Graphics;
                draw = true;
                //tempParseArr.Add(new TemplateParse(tempParseId, customizeValOneX, customizeValOneY, customizeValTwoX, customizeValTwoY));
                //gNew = Graphics.FromImage(currentImage);
                pen1 = new Pen(Color.Red, 1);
                //Debug.Print("{0} and {1}", e.X * 3, e.Y * 3);
                gCur.DrawRectangle(pen1, customizeValOneX, customizeValOneY, customizeValTwoX - customizeValOneX, customizeValTwoY - customizeValOneY);
                gCur.Save();
            }
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
                    for(int i = 0; i <tempParseArr.Count; i++)
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
}
