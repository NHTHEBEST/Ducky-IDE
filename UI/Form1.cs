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
        }


        private void Form1_Load_1(object sender, EventArgs e)
        {
            //Micronucleus.Flasher.
        }

        void x(float x)
        {
            MessageBox.Show(x.ToString());
        }

    }
}
