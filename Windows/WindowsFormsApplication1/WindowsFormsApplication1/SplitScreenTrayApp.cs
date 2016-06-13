using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class SplitScreenTrayApp : Form
    {
        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        private CreationForm createForm;
        private List<ConcreteTemplate> templateArr = new List<ConcreteTemplate>();
        private List<TemplateParse> tempParseArr = new List<TemplateParse>();
        TemplateFactory factTemp = new ConcreteTemplateFactory();
        private SnapOn snapper;
        string path;

        public SplitScreenTrayApp()
        {
            path = Directory.GetCurrentDirectory() + "\\templates.txt";
            //createForm = new CreationForm();

            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", OnExit);
            trayMenu.MenuItems.Add("Open Creation Form", openCreationForm);
            
            //trayMenu.MenuItems.Add("Load Templates", reloadTemps);
            loadTemplates(path);
            snapper = new SnapOn();

            trayIcon = new NotifyIcon();
            trayIcon.Text = "MyTrayApp";
            trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);

            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;


            // FOR TESTING
            //createForm.addTemplatesToList(templateArr);
            //createForm.Visible = true;
        }

        public void reload()
        {

        }

        //private void reloadTemps(object sender, EventArgs e)
        //{
        //    loadTemplates(path);
        //}

        private void openCreationForm(object sender, EventArgs e)
        {
            
            snapper.RemovePlexi();
            snapper.Halt();
            //Console.WriteLine("Help!!");
            createForm = new CreationForm();// this);
            createForm.addTemplatesToList(templateArr);
            createForm.Visible = true;
            //trayMenu.Dispose();
            this.Hide();

        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.

            base.OnLoad(e);
        }

        private void OnExit(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            Application.Exit();
        }


        private void RunTemp(object sender, EventArgs e)
        {
            //Debug.Print("herre");
            //Debug.Print(sender.ToString());
            String id = sender.ToString();
            id = id.Substring(id.IndexOf("Text:") + 6);

            for(int i = 0; i< templateArr.Count; i++)
            {
                if (templateArr[i].getId().Equals(id))
                {
                    snapper.Run(templateArr[i].getList());
                    //break;
                }
            }

            //Debug.Print(id);
        }




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
                        //templateList.Items.Add(name);
                        trayMenu.MenuItems.Add(name, RunTemp);
                    }
                    else
                    {
                        if (pointCounter % 4 == 0)
                        {
                            String val = line.Substring(0, lineAt);
                            //Debug.Print(val + " 1");

                            tX = Int32.Parse(val);
                        }
                        else if (pointCounter % 4 == 1)
                        {
                            String val = line.Substring(0, lineAt);
                            //Debug.Print(val + "2");

                            tY = Int32.Parse(val);
                        }
                        else if (pointCounter % 4 == 2)
                        {
                            String val = line.Substring(0, lineAt);
                            //Debug.Print(val + "3");

                            bX = Int32.Parse(val);
                        }
                        else
                        {
                            String val = line.Substring(0, lineAt);
                            //Debug.Print(val + "4");
                            bY = Int32.Parse(val);
                            tempParseArr.Add(new TemplateParse(tX, tY, bX, bY));
                            //Debug.Print("FUCK");
                        }
                        pointCounter++;

                    }

                    line = line.Substring(lineAt + 1);
                }

                List<TemplateParse> newTempArr = new List<TemplateParse>();
                //Debug.Print(name + tempParseArr.Count);
                for (int i = 0; i < tempParseArr.Count; i++)
                {
                    //Debug.Print(tempParseArr[i].toString());
                    newTempArr.Add(tempParseArr[i]);
                }

                templateArr.Add(factTemp.makeTemplate(name, newTempArr));
                tempParseArr.Clear();
                line = sr.ReadLine();
                firstVal = true;
            }
            sr.Close();
        }
    }
}
