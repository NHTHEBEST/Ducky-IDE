using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Ducky_IDE
{
    public partial class Ducky_IDE : MetroSet_UI.Forms.MetroSetForm
    {
        Thread thread;
        string ducky = "";
        string cpp = "";
        public Ducky_IDE(string file)
        {
            InitializeComponent();
            ducky_Script_Code_Box1.SendToBack();
            metroSetLabel1.Text = "Status";
            Functions.UpdateProgress += Functions_UpdateProgress;
            Functions.UpdateProgressText += Functions_UpdateProgressText;
            FormClosed += Ducky_IDE_FormClosed;
            
            metroSetLabel1.BringToFront();
            metroSetButton2.BringToFront();
            metroSetButton1.BringToFront();

            if (File.Exists(file))
                ducky_Script_Code_Box1.Text = File.ReadAllText(file);

        }



        private void Ducky_IDE_FormClosed(object sender, FormClosedEventArgs e)
        {
            Functions.Clean();
            ExitBuildFlash();
        }

        void ExitBuildFlash()
        {
            try
            {
                thread.Abort();
            }
            catch { }
        }

        private void Functions_UpdateProgressText(string value)
        {
            MethodInvoker mi = new MethodInvoker(() => metroSetLabel1.Text = value);
            if (metroSetLabel1.InvokeRequired)
            {
                metroSetLabel1.Invoke(mi);
            }
            else
            {
                mi.Invoke();
            }
        }

        private void Functions_UpdateProgress(int value)
        {
            MethodInvoker mi = new MethodInvoker(() => metroSetProgressBar1.Value = value);
            if (metroSetProgressBar1.InvokeRequired)
            {
                metroSetProgressBar1.Invoke(mi);
            }
            else
            {
                mi.Invoke();
            }
        }

        private void MetroSetButton1_Click(object sender, EventArgs e) // build
        {
            string data = ducky_Script_Code_Box1.Text;
            ExitBuildFlash();
            thread = new Thread(() => Functions.Build(data)) ;
            thread.Start();
        }

        private void MetroSetButton2_Click(object sender, EventArgs e) // flash
        {
            ExitBuildFlash();
            thread = new Thread(() => Functions.Flash());
            thread.Start();
        }

        bool DuckyMode = true;

        private void MetroSetLabel1_Click(object sender, EventArgs e)
        {
            if (DuckyMode)
            {
                ducky = ducky_Script_Code_Box1.Text;
                cpp = Functions.GetCpp(ducky);
                ducky_Script_Code_Box1.Ducky = false;
                ducky_Script_Code_Box1.Text = cpp;
                DuckyMode = false;
            }
            else
            {
                ducky_Script_Code_Box1.Text = ducky;
                ducky_Script_Code_Box1.Ducky = true;
                DuckyMode = true;
            }
        }
    }
}
