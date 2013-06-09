using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace syntaxHighlighter
{
    public partial class Form1 : Form
    {
        public SaveFileDialog sfd;//instance of savefiledialog to save files       
        public OpenFileDialog ofd;//instance of openfiledialog to open files       
        public int index;
       
        public Form1()//initialization in Constructor       
        {
            InitializeComponent();
            sfd = new SaveFileDialog();
            ofd = new OpenFileDialog();
            this.Text = "Untitled-";
            //richTextBox1.TabIndex = 5;           
            //undoToolStripMenuItem.Enabled = false;           
            richTextBox1.Focus();
        }




        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            string _loopTokens = @"(for|while|do|if|elseif|else|foreach|switch|new|class|case|goto|break|continue|private|void|public|protected|using|namespace|int|void|long|string|char|float)";
            Regex _regex = new Regex(_loopTokens);

            string _braces =  @"({|}|[|]|(|))";       //green
            Regex _bregex = new Regex(_braces);

            string _numbers = @"(0|1|2|3|4|5|6|7|8|9)";    //pink ewww yellow
            Regex _nregex = new Regex(_numbers);

            string _head = @"(.h|conio|stdio|std|out|math|string|iostream|<|>|include|#|br|head|body)";    //pink
            Regex _hregex = new Regex(_head);

            //string _comment = @"(//|//t|//n|//b|/*|*/)";   // grey  
            //Regex _cregex = new Regex(_comment); */

            MatchCollection _matchCollection = _regex.Matches(richTextBox1.Text);
            MatchCollection _braceCollection = _bregex.Matches(richTextBox1.Text);
            MatchCollection _numberCollection = _nregex.Matches(richTextBox1.Text);
            MatchCollection _headCollection = _hregex.Matches(richTextBox1.Text);
            
            int _startPosition= richTextBox1.SelectionStart;


            
            foreach (Match _bmatch in _braceCollection)
            {
                int _startIndex = _bmatch.Index;
                int _stopIndex = _bmatch.Length;
                richTextBox1.Select(_startIndex, _stopIndex);
                richTextBox1.SelectionColor = Color.Green;
                richTextBox1.SelectionStart = _startPosition;
                richTextBox1.SelectionColor = Color.Black;
            }

            foreach (Match _match in _matchCollection)
            {
                int _startIndex = _match.Index;
                int _stopIndex = _match.Length;
                richTextBox1.Select(_startIndex, _stopIndex);
                richTextBox1.SelectionColor = Color.Blue;
                richTextBox1.SelectionStart = _startPosition;
                richTextBox1.SelectionColor = Color.Black;
            }

            foreach (Match _nmatch in _numberCollection)
            {
                int _startIndex = _nmatch.Index;
                int _stopIndex = _nmatch.Length;
                richTextBox1.Select(_startIndex, _stopIndex);
                richTextBox1.SelectionColor = Color.Orange;
                richTextBox1.SelectionStart = _startPosition;
                richTextBox1.SelectionColor = Color.Black;
            }

            foreach (Match _hmatch in _headCollection)   //give a color
            {
                int _startIndex = _hmatch.Index;
                int _stopIndex = _hmatch.Length;
                richTextBox1.Select(_startIndex, _stopIndex);
                richTextBox1.SelectionColor = Color.Red;
                richTextBox1.SelectionStart = _startPosition;
                richTextBox1.SelectionColor = Color.Black;
            }
            statusLabel1.Text = "Line: " + (richTextBox1.GetLineFromCharIndex(Int32.MaxValue) + 1) + "   Cols: " + richTextBox1.Text.Length;

        }


        private void SaveFile()
        {
            //setting title of savefiledialog   
            sfd.Title = "Save As";
            sfd.Filter = "All Files|*.txt|*.cpp|*.html|*.c|*.css|*.java|*.cs";//applied filter       
            sfd.DefaultExt = "txt";//applied default extension    
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                {
                    richTextBox1.SaveFile(sfd.FileName, RichTextBoxStreamType.PlainText);
                    this.Text = sfd.FileName;
                                 
                }
            }
        }


        private void OpenFile()
        {
            //putting title of openfiledialog to Open Document      
            ofd.Title = "Open Document";
            
            ofd.Filter = "All Files|*.txt|*.cpp|*.html|*.c|*.css|*.java|*.cs";//applying filter   
            ofd.FileName = string.Empty;//setting filename box to blank       
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.FileName == String.Empty)
                {
                    return;
                }
                else
                {
                    //reading or loading selected file into richtextbox      
                    string str = ofd.FileName;
                    richTextBox1.LoadFile(str, RichTextBoxStreamType.PlainText);
                    this.Text = ofd.FileName;
                }
            }
        }

        private void Open_Click(object sender, EventArgs e)
        {
             if (richTextBox1.Modified == true)//checking either richtext box have entered value or not     
            {             
                DialogResult dr = MessageBox.Show("Do you want to save changes to the opened file", "unsaved document", MessageBoxButtons.YesNo, MessageBoxIcon.Question);  
                if (dr == DialogResult.No)             
                {                  
                   
                    richTextBox1.Modified = false;               
                    OpenFile();//calling OpenFile user defined function              
                } 
                    else         
                {                  
                    if (this.Text == "Untitled")//checking form Title to Untitled     
                    {                      
                        ///Calling SaveFile and OpenFile user defined functions     
                        SaveFile();                      
                        OpenFile();
                    }                  
                    else               
                    {
                        DialogResult dr1 = MessageBox.Show("the text in the file has been changed.Do you want to save the changes", "Open", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dr1 == DialogResult.Yes)          
                        {
                            richTextBox1.SaveFile(this.Text);    
                            OpenFile();             
                        }              
                        else           
                        {             
                            OpenFile();   
                        }                 
                    }
                }          
            }          
            else        
            {           
                OpenFile();   
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void New_Click(object sender, EventArgs e)
        {
            sfd.Title = "Save";
            DialogResult dr = MessageBox.Show("Do you want to save the file", "save", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr.Equals(DialogResult.Yes))//statement that execute when user click on yes button           
            {
                SaveFile();//calling user defined function SaveFile function               
               
            }
            else if (dr.Equals(DialogResult.No))//statament that execute when user click on no button of dialog           
            {
                richTextBox1.Clear();
                this.Text = "Untitled";
            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Modified == true)
            {
                DialogResult dr = MessageBox.Show("Do you want to save the file before exiting", "unsaved file", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    SaveFile();
                    richTextBox1.Modified = false;
                    Application.Exit();
                }
                else
                {
                    richTextBox1.Modified = false;
                    Application.Exit();
                }
            }       
        }

       

       



       

     

        private void backgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //setting background color of richtextbox  
            ColorDialog cr = new ColorDialog();
            if (cr.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.BackColor = cr.Color;
            }
        }

        private void wrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (wrapToolStripMenuItem.Checked == false)
            {
                wrapToolStripMenuItem.Checked = true;
                richTextBox1.WordWrap = true;
            }
            else
            {
                wrapToolStripMenuItem.Checked = false;
                richTextBox1.WordWrap = false;
            }
        }

        private void centerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            }
            catch (Exception ex1)
            {
                MessageBox.Show(ex1.Message.ToString());
            }      
        }

        private void rightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
            }
            catch (Exception ex1)
            {
                MessageBox.Show(ex1.Message.ToString());
            }      
        }

        private void leftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                richTextBox1.SelectionAlignment = HorizontalAlignment.Left;
            }
            catch (Exception ex1)
            {
                MessageBox.Show(ex1.Message.ToString());
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is a syntax highlighter coded by pcomputo. It helps you highlight the text as and how you enter it also provided enhanced features like wrap and intentation. Copyrights Reserved Pooja Ahuja.","About");
        }
   

    }
}
       
