using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Dialog : Form
    {
        public bool radio1 { get; set; }
        public bool radio2 { get; set; }
        public bool radio3 { get; set; }
        public bool radio4 { get; set; }
        public bool radio5 { get; set; }
        public Color color;

        public Dialog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dlg_box = colorDialog1.ShowDialog();

            if (dlg_box == DialogResult.OK)
            {
                color = colorDialog1.Color;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            radio1 = true;
            radio2 = false;
            radio3 = false;
            radio4 = false;
            radio5 = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            radio1 = false;
            radio2 = true;
            radio3 = false;
            radio4 = false;
            radio5 = false;

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            radio1 = false;
            radio2 = false;
            radio3 = true;
            radio4 = false;
            radio5 = false;

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            radio1 = false;
            radio2 = false;
            radio3 = false;
            radio4 = true;
            radio5 = false;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            radio1 = false;
            radio2 = false;
            radio3 = false;
            radio4 = false;
            radio5 = true;

        }
    }
}
