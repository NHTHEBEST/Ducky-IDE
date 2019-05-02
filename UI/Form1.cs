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
        }


        private void Form1_Load_1(object sender, EventArgs e)
        {
            fl = new List<float>();
        }
        List<float> fl;
        void x(float x, int stage)
        {
            fl.Add(x);
            int y = (int)(x * 100f);
            MessageBox.Show(y.ToString());
            metroSetProgressBar1.Value = y;
        }
        void y(string text)
        {
            MessageBox.Show(text);
        }

        private void MetroSetButton1_Click(object sender, EventArgs e)
        {
            byte[] x = { 0, 0, 0, 0, 0, 0, 0, 0, 00, 0, 0, 0, 0, 0, 0, 0 };
            MessageBox.Show(Micronucleus.Flasher.Flash(x).ToString());
            MessageBox.Show(fl.ToArray().ToString());
        }
    }
}
