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
            Status_lbl.Text = value;
        }

        private void Functions_UpdateProgress(int value)
        {
            metroSetProgressBar1.Value = value;
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            
        }


        private void MetroSetButton1_Click(object sender, EventArgs e)
        {
            Functions.Build(ducky_editor1.Text);
        }

        private void Flash_btn_Click(object sender, EventArgs e)
        {
            Functions.Flash();
        }

        private void BuildFlash_btn_Click(object sender, EventArgs e)
        {
            Functions.Build(ducky_editor1.Text);
            Functions.Flash();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Functions.Clean();
        }
    }
}
