using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TinyLang
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            string Code = textBox1.Text;
            TinyLand_Compiler.Start_Compiling(Code);
            PrintTokens();
            PrintErrors();
        }

        void PrintTokens()
        {
            for (int i = 0; i < TinyLand_Compiler.tinyLang_Scanner.Tokens.Count; i++)
            {
                dataGridView1.Rows.Add(TinyLand_Compiler.tinyLang_Scanner.Tokens.ElementAt(i).lex, TinyLand_Compiler.tinyLang_Scanner.Tokens.ElementAt(i).token_type);
            }
        }

        void PrintErrors()
        {
            for (int i = 0; i < Errors.Error_List.Count; i++)
            {
                textBox2.Text += Errors.Error_List[i];
                textBox2.Text += "\r\n";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            dataGridView1.Rows.Clear();
            Errors.Error_List.Clear();
            TinyLand_Compiler.TokenStream.Clear();
        }
    }
}