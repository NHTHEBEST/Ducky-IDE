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
            Micronucleus.Flasher.FlashUpdate += new Micronucleus.Flasher.FlashUpdateEventHandler(x);
            Micronucleus.Flasher.Text += new Micronucleus.Flasher.TextEventHandler(y);
            metroSetLabel1.Text = "";
        }


        private void Form1_Load_1(object sender, EventArgs e)
        {
            //System.IO.File.WriteAllBytes(@"C:\Users\NHTHEBEST\Downloads\hex.bin", Micronucleus.Flasher.getFromHex(System.IO.File.ReadAllBytes(@"C:\Users\NHTHEBEST\Downloads\Wifi_pass.ino.hex")));
        }
        void x(float x, int stage)
        {
            if (stage == 0)
                BeginInvoke((MethodInvoker)delegate {
                    // Running on the UI thread
                    metroSetLabel1.Text = "Conecting";
                });
            else if (stage == 1)
                BeginInvoke((MethodInvoker)delegate {
                    // Running on the UI thread
                    metroSetLabel1.Text = "Erasing";
                });
            else if (stage == 2)
                BeginInvoke((MethodInvoker)delegate {
                    // Running on the UI thread
                    metroSetLabel1.Text = "Flashing";
                });
            else
                metroSetLabel1.Text = "";
            int y = (int)(x * 100f);
            //MessageBox.Show(y.ToString());
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

        void set (object x , string text)
        { 
}

        private void MetroSetButton1_Click(object sender, EventArgs e)
        { 
            System.Threading.Thread t = new System.Threading.Thread(() => Micronucleus.Flasher.Flash(@"C:\Users\NHTHEBEST\Downloads\Wifi_pass.inos.hex").ToString());
            t.Start();
        }
    }
}
