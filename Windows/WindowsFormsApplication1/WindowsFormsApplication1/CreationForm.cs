using System;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
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

    /*
    This is where the form1 code begins
    */

    public partial class CreationForm : Form
    {

        private static string pathWallpaper;
        //private static int height; // screen height resolution
        //private static int width;  // screen width resolution
        private bool draw = false;
        private bool resizeNW = false;
        private bool resizeW = false;
        private bool resizeSW = false;
        private bool resizeS = false;
        private bool resizeSE = false;
        private bool resizeE = false;
        private bool resizeNE = false;
        private bool resizeN = false;



        private Image[] imageList;
        private int imageListIndex;
        private Image currentImage;
        private static string template;
        //private SnapOn snapper;
        //private SplitScreenTrayApp calledFrom;
        private List<TemplateParse> tempParseArr = new List<TemplateParse>();
        private TemplateParse selectedTemplateParse;
        private List<ConcreteTemplate> templateArr = new List<ConcreteTemplate>();
        private List<ToolStripMenuItem> editList = new List<ToolStripMenuItem>();
        //private List<Plexiglass> tempPlexiArr = new List<Plexiglass>();
        TemplateFactory factTemp = new ConcreteTemplateFactory();

        private static string customizeValOne; // the value for the first point of the square being customized
        private static int customizeValOneX = -20; // the value for the first point's x position
        private static int customizeValOneY = -20; //  the value for the first point's y position
        private static string customizeValTwo; // the value for the first point of the square being customized
        private static int customizeValTwoX = -20; // the value for the first point's x position
        private static int customizeValTwoY; //  the value for the first point's y position
        private static int screenY = Screen.PrimaryScreen.WorkingArea.Height; // Screen Resolution Y value
        private static int screenX = Screen.PrimaryScreen.WorkingArea.Width; // Screen Resolution X value
        private static double resoRatioY = screenY/1080.0;
        private static double resoRatioX = screenX/1920.0;
        private static double formSizeRatioX = 0;
        private static double formSizeRatioY = 0;
        private SplitScreenTrayApp caller;


        private delegate IntPtr LowLevelMouseProc(int nCode,
            IntPtr wParam, IntPtr lParam);

        /**
        
        */
        public CreationForm(SplitScreenTrayApp called)
        {
            caller = called;
            InitializeComponent();
            formSizeRatioX = (this.Width-30) / (double)screenX;
            formSizeRatioY = (this.Height-100) / (double)screenY;
            GetPathOfWallpaper();
            currentImage = Image.FromFile(pathWallpaper);
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



        public void addTemplatesToList(List<ConcreteTemplate> arr)
        {
            for(int i =0; i < arr.Count; i++) {
                templateArr.Add(factTemp.makeTemplate(arr[i].getId(), arr[i].getList()));
                //templateList.Items.Add(arr[i].getId());

                editList.Add(new ToolStripMenuItem());
                editList[i].Text = arr[i].getId();
                editList[i].Click += new EventHandler(OnEditSubItemClick);
                
                this.editToolStripMenuItem.DropDownItems.Add(editList[i]);
                
            }
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




        private void confirmationButton_Click(object sender, EventArgs e)
        {
            tempParseArr.Add(new TemplateParse(customizeValOneX, customizeValOneY, customizeValTwoX, customizeValTwoY));

            //pictureBox1.Dispose();
            customizeValOneX = 0;
            customizeValOneY = 0;
            customizeValTwoX = 0;
            customizeValTwoY = 0;
            pictureBox1.Refresh();
            pictureBox1.Update();

        }



        private void Form1_Load(object sender, EventArgs e)
        {
            this.pictureBox1.ContextMenu = new ContextMenu();
  
            this.pictureBox1.Paint += new PaintEventHandler(this.pictureBox1_Paint);
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

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            draw = false;
             resizeNW = false;
             resizeW = false;
             resizeSW = false;
             resizeS = false;
             resizeSE = false;
             resizeE = false;
             resizeNE = false;
             resizeN = false;
    }


        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            int position = mousePosition(e.X, e.Y);
            if (!draw)
            {
                if (position == 0)
                {
                    Cursor.Current = Cursors.PanNW;
                }
                else if (position == 1)
                {
                    Cursor.Current = Cursors.PanNE;
                }
                else if (position == 2)
                {
                    Cursor.Current = Cursors.PanSE;
                }
                else if (position == 3)
                {
                    Cursor.Current = Cursors.PanSW;
                }
                else if (position == 4)
                {
                    Cursor.Current = Cursors.PanWest;
                }
                else if (position == 5)
                {
                    Cursor.Current = Cursors.PanEast;
                }
                else if (position == 6)
                {
                    Cursor.Current = Cursors.PanNorth;
                }
                else if (position == 7)
                {
                    Cursor.Current = Cursors.PanSouth;
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                }
            }
            else
            {
                if (resizeNW)
                {
                    Cursor.Current = Cursors.PanNW;
                    if (e.X * (1 / formSizeRatioX) < 0) { customizeValOneX = 0; }
                    else if (e.X * (1 / formSizeRatioX) > customizeValTwoX) { customizeValOneX = customizeValTwoX - 10; }
                    else { customizeValOneX = Convert.ToInt32(e.X * (1 / formSizeRatioX)); }

                    if ((e.Y * (1 / formSizeRatioY) < 0)) { customizeValOneY = 0; }
                    else if (e.Y * (1 / formSizeRatioY) > customizeValTwoY) { customizeValOneY = customizeValTwoY - 10; }
                    else { customizeValOneY = Convert.ToInt32(e.Y * (1 / formSizeRatioY)); }
                    pictureBox1.Refresh();
                }
                else if (resizeSW)
                {
                    Cursor.Current = Cursors.PanSW;
                    if (e.X * (1 / formSizeRatioX) < 0) { customizeValOneX = 0; }
                    else if (e.X * (1 / formSizeRatioX) > customizeValTwoX) { customizeValOneX = customizeValTwoX - 10; }
                    else { customizeValOneX = Convert.ToInt32(e.X * (1 / formSizeRatioX)); }

                    if ((e.Y * (1 / formSizeRatioY) > screenY)) { customizeValTwoY = screenY; }
                    else if (e.Y * (1 / formSizeRatioY) < customizeValOneY) { customizeValTwoY = customizeValOneY + 10; }
                    else { customizeValTwoY = Convert.ToInt32(e.Y * (1 / formSizeRatioY)); }
                    pictureBox1.Refresh();
                }
                else if (resizeSE)
                {
                    Cursor.Current = Cursors.PanSE;
                    if (e.X * (1 / formSizeRatioX) > screenX) { customizeValTwoX = screenX; }
                    else if (e.X * (1 / formSizeRatioX) < customizeValOneX) { customizeValTwoX = customizeValOneX + 10; }
                    else { customizeValTwoX = Convert.ToInt32(e.X * (1 / formSizeRatioX)); }

                    if ((e.Y * (1 / formSizeRatioY) > screenY)) { customizeValTwoY = screenY; }
                    else if (e.Y * (1 / formSizeRatioY) < customizeValOneY) { customizeValTwoY = customizeValOneY + 10; }
                    else { customizeValTwoY = Convert.ToInt32(e.Y * (1 / formSizeRatioY)); }
                    pictureBox1.Refresh();
                }
                else if (resizeNE)
                {
                    Cursor.Current = Cursors.PanNE;
                    if (e.X * (1 / formSizeRatioX) > screenX) { customizeValTwoX = screenX; }
                    else if (e.X * (1 / formSizeRatioX) < customizeValOneX) { customizeValTwoX = customizeValOneX + 10; }
                    else { customizeValTwoX = Convert.ToInt32(e.X * (1 / formSizeRatioX)); }

                    if ((e.Y * (1 / formSizeRatioY) < 0)) { customizeValOneY = 0; }
                    else if (e.Y * (1 / formSizeRatioY) > customizeValTwoY) { customizeValOneY = customizeValTwoY - 10; }
                    else { customizeValOneY = Convert.ToInt32(e.Y * (1 / formSizeRatioY)); }
                    pictureBox1.Refresh();
                }
                else if (resizeW)
                {
                    Cursor.Current = Cursors.PanWest;
                    if (e.X * (1 / formSizeRatioX) < 0) { customizeValOneX = 0; }
                    else if (e.X * (1 / formSizeRatioX) > customizeValTwoX) { customizeValOneX = customizeValTwoX - 10; }
                    else { customizeValOneX = Convert.ToInt32(e.X * (1 / formSizeRatioX)); }
                    
                    //customizeValOneY = Convert.ToInt32(e.Y * (1 / formSizeRatioY));
                    pictureBox1.Refresh();
                }
                else if (resizeE)
                {
                    Cursor.Current = Cursors.PanEast;
                    if (e.X * (1 / formSizeRatioX) > screenX) { customizeValTwoX = screenX; }
                    else if (e.X * (1 / formSizeRatioX) < customizeValOneX) { customizeValTwoX = customizeValOneX + 10; }
                    else { customizeValTwoX = Convert.ToInt32(e.X * (1 / formSizeRatioX)); }
                    //customizeValOneY = Convert.ToInt32(e.Y * (1 / formSizeRatioY));
                    pictureBox1.Refresh();
                }
                else if (resizeN)
                {
                    Cursor.Current = Cursors.PanNorth;
                    //customizeValOneX = Convert.ToInt32(e.X * (1 / formSizeRatioX));
                    if ((e.Y * (1 / formSizeRatioY) < 0)) { customizeValOneY = 0; }
                    else if(e.Y * (1 / formSizeRatioY) > customizeValTwoY) { customizeValOneY = customizeValTwoY - 10; }
                    else { customizeValOneY = Convert.ToInt32(e.Y * (1 / formSizeRatioY)); }
                    pictureBox1.Refresh();

                }
                else if (resizeS)
                {
                    Cursor.Current = Cursors.PanSouth;
                    if ((e.Y * (1 / formSizeRatioY) > screenY)) { customizeValTwoY = screenY; }
                    else if (e.Y * (1 / formSizeRatioY) < customizeValOneY) { customizeValTwoY = customizeValOneY + 10; }
                    else { customizeValTwoY = Convert.ToInt32(e.Y * (1 / formSizeRatioY)); }
                    pictureBox1.Refresh();

                }
                else
                {
                    if (e.X > screenX * formSizeRatioX)
                        customizeValTwoX = Convert.ToInt32(currentImage.Width * (1 / formSizeRatioX));
                    else
                        customizeValTwoX = Convert.ToInt32(e.X * (1 / formSizeRatioX));

                    if (e.Y > screenY * formSizeRatioY)
                        customizeValTwoY = Convert.ToInt32(currentImage.Height * (1 / formSizeRatioY));
                    else
                        customizeValTwoY = Convert.ToInt32(e.Y * (1 / formSizeRatioY));
                    pictureBox1.Refresh();
                }
            }


        }

        private void pictureBox1_DoubleClick(object sender, MouseEventArgs e)
        {
            if (tempParseArr.Count != 0)
            {
                if (tempParseArr != null)
                {
                    for (int i = 0; i < tempParseArr.Count; i++)
                    {
                        if ((e.X* (1 / formSizeRatioX)) - (tempParseArr[i]).getTopX() < 20 && (e.X * (1 / formSizeRatioX)) - (tempParseArr[i]).getTopX() > 0) 
                        {
                            customizeValOneX = tempParseArr[i].getTopX();
                            customizeValOneY = tempParseArr[i].getTopY();
                            customizeValTwoX = tempParseArr[i].getBotX();
                            customizeValTwoY = tempParseArr[i].getBotY();
                            tempParseArr.Remove(tempParseArr[i]);
                            
                            pictureBox1.Refresh();
                        }

                    }
                }
            }
        }


        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int position = mousePosition(e.X, e.Y);
            if (position == 0)
            {
                Cursor.Current = Cursors.PanNW;
                resizeNW = true;
            }
            else if(position == 1)
            {
                Cursor.Current = Cursors.PanNE;
                resizeNE = true;
            }
            else if (position == 2)
            {
                Cursor.Current = Cursors.PanSE;
                resizeSE = true;
            }
            else if (position == 3)
            {
                Cursor.Current = Cursors.PanSW;
                resizeSW = true;
            }
            else if(position == 4)
            {
                Cursor.Current = Cursors.PanWest;
                resizeW = true;
            }
            else if (position == 5)
            {
                Cursor.Current = Cursors.PanEast;
                resizeE = true;
            }
            else if (position == 6)
            {
                Cursor.Current = Cursors.PanNorth;
                resizeN = true;
            }
            else if (position == 7)
            {
                Cursor.Current = Cursors.PanSouth;
                resizeS = true;
            }
            else
            {
                Cursor.Current = Cursors.Default;
                customizeValOneX = Convert.ToInt32(e.X * (1 / formSizeRatioX));
                customizeValOneY = Convert.ToInt32(e.Y * (1 / formSizeRatioY));
                if (customizeValOneX < 20)
                    customizeValOneX = 0;
                if (customizeValOneY < 20)
                    customizeValOneY = 0;
            }


            draw = true;
        }

        private int mousePosition(int x, int y)
        {
            if (((x * (1 / formSizeRatioX)) - (customizeValOneX) < 10)
                && ((x * (1 / formSizeRatioX)) - (customizeValOneX) > -10)
                && ((y * (1 / formSizeRatioY)) - (customizeValOneY) < 10)
                && ((y * (1 / formSizeRatioY)) - (customizeValOneY) > -10))
            {
                return 0;
            }
            else if (((x * (1 / formSizeRatioX)) - (customizeValTwoX) < 10)
               && ((x * (1 / formSizeRatioX)) - (customizeValTwoX) > -10)
               && ((y * (1 / formSizeRatioY)) - (customizeValOneY) < 10)
               && ((y * (1 / formSizeRatioY)) - (customizeValOneY) > -10))
            {
                return 1;
            }
            else if (((x * (1 / formSizeRatioX)) - (customizeValTwoX) < 10)
               && ((x * (1 / formSizeRatioX)) - (customizeValTwoX) > -10)
               && ((y * (1 / formSizeRatioY)) - (customizeValTwoY) < 10)
               && ((y * (1 / formSizeRatioY)) - (customizeValTwoY) > -10))
            {
                return 2;
            }
            else if (((x * (1 / formSizeRatioX)) - (customizeValOneX) < 10)
               && ((x * (1 / formSizeRatioX)) - (customizeValOneX) > -10)
               && ((y * (1 / formSizeRatioY)) - (customizeValTwoY) < 10)
               && ((y * (1 / formSizeRatioY)) - (customizeValTwoY) > -10))
            {
                return 3;
            }
            else if (((x * (1 / formSizeRatioX)) - (customizeValOneX) < 10)
                && ((x * (1 / formSizeRatioX)) - (customizeValOneX) > -10)
                && (y * (1/formSizeRatioY) > customizeValOneY)
                && (y* (1/formSizeRatioY) < customizeValTwoY))
            {
                return 4;
            }
            else if (((x * (1 / formSizeRatioX)) - (customizeValTwoX) < 10)
                && ((x * (1 / formSizeRatioX)) - (customizeValTwoX) > -10)
                && (y * (1 / formSizeRatioY) > customizeValOneY)
                && (y * (1 / formSizeRatioY) < customizeValTwoY))
            {
                return 5;
            }
            else if (((y * (1 / formSizeRatioY)) - (customizeValOneY) < 10)
               && ((y * (1 / formSizeRatioY)) - (customizeValOneY) > -10)
               && (x * (1 / formSizeRatioX) > customizeValOneX)
                && (x * (1 / formSizeRatioX) < customizeValTwoX))
            {
                return 6;
            }
            else if (((y * (1 / formSizeRatioY)) - (customizeValTwoY) < 10)
                && ((y * (1 / formSizeRatioY)) - (customizeValTwoY) > -10)
                && (x * (1 / formSizeRatioX) > customizeValOneX)
                && (x * (1 / formSizeRatioX) < customizeValTwoX))
            {
                return 7;
            }
            return -1;
        }

        
        private void OnEditSubItemClick(object sender, EventArgs e)
        {
            tempParseArr.Clear();
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            string args = menuItem.Text.ToString();
           // Debug.Print(args);

            tempParseArr.Clear();
            currentImage = ResizeImage(currentImage, Convert.ToInt32(screenX * formSizeRatioX), Convert.ToInt32(screenY * formSizeRatioY));
            this.pictureBox1.Size = new System.Drawing.Size(Convert.ToInt32(screenX * formSizeRatioX), Convert.ToInt32(screenY * formSizeRatioY));
            this.pictureBox1.Image = currentImage;
            this.pictureBox1.Visible = true;

            try
            {
                this.positioningText.Text = args;
                template = args;
                for (int i = 0; i < templateArr.Count; i++)
                {
                    if (templateArr[i].getId().Equals(args))
                    {

                        List<TemplateParse> selected = templateArr[i].getList();
                        for (int j = 0; j < selected.Count; j++)
                        {
                            tempParseArr.Add(selected[j]);
                        }
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





        private void pictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (tempParseArr.Count == 0)
            {
                Graphics gCur = e.Graphics;
                Pen pen1 = new Pen(Color.WhiteSmoke, 1);
                gCur.DrawRectangle(pen1, Convert.ToInt32(customizeValOneX*formSizeRatioX), Convert.ToInt32(customizeValOneY*formSizeRatioY), 
                    Convert.ToInt32(customizeValTwoX*formSizeRatioX) - Convert.ToInt32(customizeValOneX*formSizeRatioX),
                    Convert.ToInt32(customizeValTwoY*formSizeRatioY) - Convert.ToInt32(customizeValOneY*formSizeRatioY));
                
            }
            else
            {
                Graphics gCur;
                Pen pen1;
                SolidBrush brush1;
                if(tempParseArr != null) {
                    for (int i = 0; i < tempParseArr.Count; i++ )
                    {
                        gCur = e.Graphics;
                        pen1 = new Pen(Color.Red, 5);
                        Debug.Print("{0} and {1}", (tempParseArr[i]).getTopX(), (tempParseArr[i]).getTopY());
                        gCur.DrawRectangle(pen1, Convert.ToInt32((tempParseArr[i]).getTopX()*formSizeRatioX), Convert.ToInt32((tempParseArr[i]).getTopY()*formSizeRatioY),
                            (Convert.ToInt32((tempParseArr[i]).getBotX()*formSizeRatioX - (tempParseArr[i]).getTopX()*formSizeRatioX)), 
                            Convert.ToInt32((tempParseArr[i]).getBotY()*formSizeRatioY - (tempParseArr[i]).getTopY()*formSizeRatioY));
                        pen1 = new Pen(Color.Green, 5);
                        brush1 = new SolidBrush(Color.Green);
                        gCur.FillRectangle(brush1, (Convert.ToInt32((tempParseArr[i]).getBotX()*formSizeRatioX) - 20), Convert.ToInt32((tempParseArr[i]).getBotY()*formSizeRatioY - 20), 20, 20);
                    }
                }
                gCur = e.Graphics;
                pen1 = new Pen(Color.WhiteSmoke, 1);
                gCur.DrawRectangle(pen1, Convert.ToInt32(customizeValOneX*formSizeRatioX), Convert.ToInt32(customizeValOneY*formSizeRatioY), 
                    Convert.ToInt32(customizeValTwoX*formSizeRatioX) - Convert.ToInt32(customizeValOneX*formSizeRatioX),
                    Convert.ToInt32(customizeValTwoY*formSizeRatioY) - Convert.ToInt32(customizeValOneY*formSizeRatioY));
            }

        }




        private void templateConfirmationButton_Click(object sender, EventArgs e)
        {
            //String savedTemplateString;
            var result = MessageBox.Show("Do you want to save this template", "Save Template", MessageBoxButtons.YesNoCancel);
            //ConcreteTemplate savedTemplate;
            int randomNuber = new Random().Next(int.MinValue, int.MaxValue);
            String newLine = "helloworld" + randomNuber;
            if (result == DialogResult.Yes)
            {
                templateArr.Add(factTemp.makeTemplate(newLine, tempParseArr));
                try
                {
                    string path = Directory.GetCurrentDirectory() + "\\templates.txt";
                    TextWriter sw = new StreamWriter(path, true);
                    for(int i = 0; i <tempParseArr.Count; i++)
                    {
                        newLine = newLine + "|" + tempParseArr[i].getTopX() + "|" + tempParseArr[i].getTopY() + "|" + tempParseArr[i].getBotX() + "|" + tempParseArr[i].getBotY();
                    }
                    newLine = newLine + "|end";
                    
                    
                    sw.WriteLine(newLine);
                    sw.Close();
                    sw.Dispose();
                }
                catch (Exception exe)
                {
                    Console.WriteLine("Exception::::: " + exe.Message);
                }

            }

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tempParseArr.Clear();
            GetPathOfWallpaper();
           
            currentImage = ResizeImage(currentImage, Convert.ToInt32(screenX * formSizeRatioX), Convert.ToInt32(screenY * formSizeRatioY));
            this.pictureBox1.Size = new System.Drawing.Size(Convert.ToInt32(screenX * formSizeRatioX), Convert.ToInt32(screenY * formSizeRatioY));
            //Debug.Print((formSizeRatioX.ToString());
            
            this.pictureBox1.Image = currentImage;
            this.pictureBox1.Visible = true;
            draw = false;
        }

    }

}
