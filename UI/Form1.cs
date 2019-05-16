using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI_Components;
using MetroSet_UI.Forms;
using System.Threading;

namespace UI
{
    public partial class Form1 : MetroSetForm
    {
        public Form1()
        {
            InitializeComponent();
            Status_lbl.Text = "";
            Functions.UpdateProgress += Functions_UpdateProgress;
            Functions.UpdateProgressText += Functions_UpdateProgressText;
        }

        private void Functions_UpdateProgressText(string value)
        {
            MethodInvoker mi = new MethodInvoker(() => Status_lbl.Text = value);
            if (Status_lbl.InvokeRequired)
            {
                Status_lbl.Invoke(mi);
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

        private void Form1_Load_1(object sender, EventArgs e)
        {
            
        }


        private void MetroSetButton1_Click(object sender, EventArgs e)
        {
            string data = ducky_editor1.Text;
            Thread thread = new Thread(() => Functions.Build(data));
            thread.Start();
        }

        private void Flash_btn_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => Functions.Flash());
            thread.Start();
        }

        private void BuildFlash_btn_Click(object sender, EventArgs e)
        {
            string data = ducky_editor1.Text;
            Thread thread = new Thread(() => Functions.Build(data));
            thread.Start();
            Thread thread2 = new Thread(() => Functions.Flash());
            thread2.Start();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Functions.Clean();

        }
    }
}
