using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace My_Project
{
    public partial class Chitalka : Form
    {
        public Chitalka() => InitializeComponent();
        public class Reader : Chitalka
        {
            private string _Text_of_File;
            public List<string> Pages { get; set; }
            public void _Reader_Page(int _Pages) => Text = Pages[_Pages];
            public string _File_Text
            {
                get => _Text_of_File;
                set => _Initialize(value);
            }
            private void _Initialize(string _File)
            {
                if (string.IsNullOrEmpty(_File))
                {
                    _Text_of_File = string.Empty;
                    return;
                }
                _Text_of_File = _File;
                Pages = new List<string>();
                TextFormatFlags _Flags = TextFormatFlags.Top | TextFormatFlags.Left | TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl | TextFormatFlags.NoPadding;
                Size _Text_Size = TextRenderer.MeasureText(_Text_of_File, Font, ClientSize, _Flags);
                int _Number_Pages = _Text_Size.Height / ClientSize.Height;
                if (_Text_Size.Height > Height)
                {
                    Main_Rich_Text_Box.Text = _File;
                    Main_Rich_Text_Box.Update();
                    int _First_Char_Last_Line = Main_Rich_Text_Box.GetCharIndexFromPosition(new Point(0, ClientSize.Height));
                    int _Lines = Main_Rich_Text_Box.GetLineFromCharIndex(_First_Char_Last_Line);
                    int _Total_Lines = Main_Rich_Text_Box.GetLineFromCharIndex(Text.Length - 1);
                    for (int _Pages = 0; _Pages < _Number_Pages; _Pages++)
                    {
                        int _First_Line_Page = (_Pages * _Lines);
                        int _First_Char_Page = Main_Rich_Text_Box.GetFirstCharIndexFromLine(_First_Line_Page);
                        if (_First_Char_Page < 0) _First_Char_Page = 0;
                        int _First_Line_Next_Page = (_Pages + 1) * _Lines;
                        _First_Line_Next_Page = (_First_Line_Next_Page > _Total_Lines) ? _Total_Lines : _First_Line_Next_Page;
                        int _Last_Char_Page = (_First_Line_Next_Page < _Total_Lines) ? Main_Rich_Text_Box.GetFirstCharIndexFromLine(_First_Line_Next_Page) - 1 : Text.Length;
                        Pages.Add(Text.Substring(_First_Char_Page = 0, _Last_Char_Page - _First_Char_Page));
                    }
                }
                else { Pages.Add(_Text_of_File); }
                Text = Pages.First();
            }
        }
        Reader _Document_Viewer = null;      
        public int _Current_Page = 0;
        public int _First_Page = 0;
        public int _Last_Page = 0;
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var _File = new OpenFileDialog();
            _File.ShowDialog();
            _Document_Viewer = new Reader();
            string _File_Document = File.ReadAllText(_File.FileName, Encoding.UTF8);
            Main_Rich_Text_Box.Text = _Document_Viewer._File_Text = _File_Document;
            _Last_Page = _Document_Viewer.Pages.Count - 1;
        }
        private void Next_Button_Click(object sender, EventArgs e)
        {
            if (_Current_Page < _Last_Page)
            {
                _Current_Page += 1;            
               _Document_Viewer._Reader_Page(_Current_Page);
            }
        }
        private void Exit_Button_Click(object sender, EventArgs e) => Application.Exit();
    }
}