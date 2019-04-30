using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroSet_UI.Controls;
using System.Text.RegularExpressions;

namespace UI_Components
{
    public partial class ducky_editor : RichTextBox //MetroSetRichTextBox
    {
        public ducky_editor()
        {
            InitializeComponent();
            regex = new List<string>();
            BorderStyle = BorderStyle.None;
            BackColor = Color.FromArgb(34, 34, 34);
            ForeColor = Color.FromArgb(204,204,204);
            KeyUp += keyPress;
            foreach (string tok in IntalTokens)
                regex.Add(tok);
        }
        private string[] IntalTokens = { "ENTER", "STRING", "GUI" };

        public AutoScaleMode AutoScaleMode;

        public List<string> regex { get; private set; }
        public string Tokens
        {
            get
            {
                string buff = string.Join("|",regex.ToArray());
                return "("+buff+ @"|DELAY\s*[0-9]*|REM( \w*)*(?!(\r\n)))";
            }
        }



        private void keyPress(object sender, EventArgs e)
        {
            Regex rex = new Regex(Tokens);
            MatchCollection mc = rex.Matches(Text);
            int StartCursorPosition = SelectionStart;
            foreach (Match m in mc)
            {
                int startIndex = m.Index;
                int StopIndex = m.Length;
                Select(startIndex, StopIndex);
                if (m.Value.StartsWith("REM"))
                {
                    SelectionColor = Color.LimeGreen;
                }
                else if (m.Value == "STRING")
                {
                    SelectionColor = Color.Yellow;
                }
                else if (m.Value.StartsWith("DELAY"))
                {
                    SelectionColor = Color.Purple;
                }
                else if (m.Value == "ENTER")
                {
                    SelectionColor = Color.HotPink;
                }
                else
                {
                    SelectionColor = Color.Red;
                }
                SelectionStart = StartCursorPosition;
                SelectionColor = Color.FromArgb(204, 204, 204);
            }
        }
    }
}
