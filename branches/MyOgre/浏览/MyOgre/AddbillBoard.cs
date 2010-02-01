using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MyOgre
{
    public partial class AddbillBoard : Form
    {
        public Font selectfont;
        public Color selectcolor;
        public string text = "";

        public AddbillBoard()
        {
            InitializeComponent();
            textBox1.Font = selectfont;
            textBox1.ForeColor = selectcolor;
        }

        private void setfont_button_Click(object sender, EventArgs e)
        {
            FontDialog dlg = new FontDialog();
            dlg.Font = selectfont;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                selectfont = dlg.Font;
                textBox1.Font = selectfont;
            }
        }

        private void setcolor_button_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = selectcolor;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                selectcolor = dlg.Color;
                textBox1.ForeColor = selectcolor;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            text = textBox1.Text;
        }
    }
}
