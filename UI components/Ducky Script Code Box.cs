using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace UI_components
{
    public partial class Ducky_Script_Code_Box : UserControl
    {

        FastColoredTextBoxNS.AutocompleteMenu autocompleteMenu;
        string defaulttext = "REM NHTHEBEST DUCKY IDE";
        bool saved = true;
        public Ducky_Script_Code_Box()
        {
            InitializeComponent();
            fastColoredTextBox1.Dock = DockStyle.Fill;
            fastColoredTextBox1.BackColor = Color.FromArgb(40, 42, 54);
            fastColoredTextBox1.ForeColor = Color.FromArgb(98, 114, 164);
            fastColoredTextBox1.LineNumberColor = Color.FromArgb(98, 114, 164);
            fastColoredTextBox1.IndentBackColor = Color.FromArgb(40, 42, 54);
            fastColoredTextBox1.PaddingBackColor = Color.FromArgb(40, 42, 54);
            fastColoredTextBox1.ServiceLinesColor = Color.FromArgb(40, 42, 54);

            fastColoredTextBox1.Language = FastColoredTextBoxNS.Language.Custom;
            fastColoredTextBox1.AutoIndent = false;
            fastColoredTextBox1.AutoIndentChars = false;
            fastColoredTextBox1.Text = defaulttext;
            fastColoredTextBox1.DescriptionFile = "Ducky.xml";

            fastColoredTextBox1.KeyDown += FastColoredTextBox1_KeyDown;
            fastColoredTextBox1.TextChanged += FastColoredTextBox1_TextChanged;


            string[] keywords =
                { "REPEAT", "REM", "ENTER", "STRING", "DELAY", "GUI", "WINDOWS",
                "APP", "MENU", "SHIFT", "ALT", "CONTROL", "CTRL", "DOWNARROW", "LEFTARROW",
                "RIGHTARROW", "UPARROW", "DOWN", "LEFT", "RIGHT", "UP", "BREAK", "PAUSE",
                "CAPSLOCK", "DELETE", "END", "ESC", "ESCAPE", "HOME", "INSERT", "NUMLOCK",
                "PAGEUP", "PAGEDOWN", "PRINTSCREEN", "SCROLLLOCK", "SPACE", "TAB" };


            autocompleteMenu = new FastColoredTextBoxNS.AutocompleteMenu(fastColoredTextBox1);
            autocompleteMenu.MinFragmentLength = 2;
            autocompleteMenu.Items.SetAutocompleteItems(keywords);
            autocompleteMenu.Items.MaximumSize = new Size(200, 300);
            autocompleteMenu.Items.Width = 200;
            autocompleteMenu.AllowTabKey = true;

            autocompleteMenu.BackColor = Color.FromArgb(40, 42, 54);
            autocompleteMenu.ForeColor = Color.FromArgb(98, 114, 164);
            autocompleteMenu.SelectedColor = Color.FromArgb(50, 255, 255, 255);


            fastColoredTextBox1.Update();
            fastColoredTextBox1.OnKeyPressed('\n');
        }

        private void FastColoredTextBox1_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            saved = false;
        }
        Stream FileLocation;
        private void FastColoredTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)       // Ctrl-S Save
            {
                try
                {
                    // Do what you want here
                    if(FileLocation == null)
                    {
                        SaveFileDialog file = new SaveFileDialog();
                        file.Filter = "Text Files | *.txt";
                        file.ShowDialog();
                        FileLocation = file.OpenFile();
                        
                    }
                    StreamWriter streamWriter = new StreamWriter(FileLocation);
                    streamWriter.Write(fastColoredTextBox1.Text);
                    streamWriter.Flush();
                    saved = true;
                }
                catch { }
            }
            else if (e.Control && e.KeyCode == Keys.O)
            {
                try
                {
                    if(!saved)
                    {
                        DialogResult res = MessageBox.Show("File Not Saved","Continue",MessageBoxButtons.OKCancel);
                        if (res != DialogResult.OK)
                            throw new Exception("NO");
                    }
                    OpenFileDialog file = new OpenFileDialog();
                    file.Filter = "Text Files | *.txt";
                    file.ShowDialog();
                    FileLocation = file.OpenFile();
                    var x = new StreamReader(FileLocation);
                    fastColoredTextBox1.Text = x.ReadToEnd();
                    saved = true;
                }
                catch { }
            }
        }

        public string Text
        {
            get
            {
                return fastColoredTextBox1.Text;
            }
            set
            {
                fastColoredTextBox1.Text = value;
            }
        }

        public bool Ducky
        {
            get
            {
                return FastColoredTextBoxNS.Language.Custom==fastColoredTextBox1.Language;
            }
            set
            {
                if (value)
                    fastColoredTextBox1.Language = FastColoredTextBoxNS.Language.Custom;
                else
                    fastColoredTextBox1.Language = FastColoredTextBoxNS.Language.CSharp;
            }
        }
    }
}
