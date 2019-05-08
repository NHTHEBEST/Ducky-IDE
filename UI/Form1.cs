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
using Core;

namespace UI
{
    public partial class Form1 : MetroSetForm
    {
        private BuildProsses EncodeCompile;
        public Form1()
        {
            InitializeComponent();
            Micronucleus.Flasher.FlashUpdate += new Micronucleus.Flasher.FlashUpdateEventHandler(x);
            Micronucleus.Flasher.Text += new Micronucleus.Flasher.TextEventHandler(y);
            Status_lbl.Text = "";
        }


        private void Form1_Load_1(object sender, EventArgs e)
        {
            
        }


        void x(float p, int stage)
        {
            if (stage == 0)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    // Running on the UI thread
                    Status_lbl.Text = "Conecting";
                });
            }
            else if (stage == 1)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    // Running on the UI thread
                    Status_lbl.Text = "Erasing";
                });
            }
            else if (stage == 2)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    // Running on the UI thread
                    Status_lbl.Text = "Flashing";
                });
            }
            else
            {
                Status_lbl.Text = "";
            }
            int y = (int)(p * 100f);
            metroSetProgressBar1.Value = y;
        }
        void y(string text)
        {
            //MessageBox.Show(text);
            BeginInvoke((MethodInvoker)delegate {
                // Running on the UI thread
                ducky_editor1.Text = ducky_editor1.Text + text;
            });
            
        }

        private void MetroSetButton1_Click(object sender, EventArgs e)
        {
            BuildProsses prosses = new BuildProsses(ducky_editor1.Text);
            Micronucleus.Flasher.Flash(prosses.RawBinary);
        }


        private void Build()
        {
            EncodeCompile = new BuildProsses(ducky_editor1.Text);
        }
        private void Flash()
        {

        }
    }
}
