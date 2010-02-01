using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MyOgre
{
    public partial class AddModelForm : Form
    {
        public string picturename = "";
        public string vidoname = "";
        public AddModelForm()
        {
            InitializeComponent();
        }

        private void picture_button_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                picturename = openFileDialog1.FileName;
                picture_textBox.Text = Path.GetFileName( openFileDialog1.FileName);
            }
        }

        private void vido_button_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                vidoname = openFileDialog1.FileName;
                vido_textBox.Text = Path.GetFileName(openFileDialog1.FileName);
            }
        }

        private void picture_textBox_TextChanged(object sender, EventArgs e)
        {
            if (picture_textBox.Text == "")
                picturename = "";
        }

        private void vido_textBox_TextChanged(object sender, EventArgs e)
        {
            if (vido_textBox.Text == "")
                vidoname = "";
        }
    }
}
