using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI_components
{
    public partial class Ducky_Script_Code_Box : UserControl
    {
        FastColoredTextBoxNS.AutocompleteMenu autocompleteMenu;
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
            fastColoredTextBox1.Text = "REM NHTHEBEST DUCKY IDE";
            fastColoredTextBox1.DescriptionFile = "Ducky.xml";


            string[] keywords =
                { "REPEAT", "REM", "ENTER", "STRING", "DELAY", "GUI", "WINDOWS",
                "APP", "MENU", "SHIFT", "ALT", "CONTROL", "CTRL", "DOWNARROW", "LEFTARROW",
                "RIGHTARROW", "UPARROW", "DOWN", "LEFT", "RIGHT", "UP", "BREAK", "PAUSE",
                "CAPSLOCK", "DELETE", "END", "ESC", "ESCAPE", "HOME", "INSERT", "NUMLOCK",
                "PAGEUP", "PAGEDOWN", "PRINTSCREEN", "SCROLLLOCK", "SPACE", "TAB" };


            autocompleteMenu = new FastColoredTextBoxNS.AutocompleteMenu(fastColoredTextBox1);
            autocompleteMenu.MinFragmentLength = 2;
            autocompleteMenu.Items.SetAutocompleteItems(keywords);
            autocompleteMenu.Items.MaximumSize = new System.Drawing.Size(200, 300);
            autocompleteMenu.Items.Width = 200;
            autocompleteMenu.AllowTabKey = true;

            autocompleteMenu.BackColor = Color.FromArgb(40, 42, 54);
            autocompleteMenu.ForeColor = Color.FromArgb(98, 114, 164);
            autocompleteMenu.SelectedColor = Color.FromArgb(50, 255, 255, 255);
        }
    }
}
