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
    public partial class BackgroupForm : Form
    {
        string[] files = null;
        public BackgroupForm()
        {
            InitializeComponent();
        }

        private void BackgroupForm_Load(object sender, EventArgs e)
        {
            InitListbox();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                pictureBox1.Image = new Bitmap(files[listBox1.SelectedIndex]);
            }
        }
        //导入
        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string sourcestring=Application.StartupPath + "\\Media\\BlackGround\\"+Path.GetFileName(openFileDialog1.FileName);
                
                File.Copy(openFileDialog1.FileName, sourcestring,true);
                listBox1.Items.Add(Path.GetFileName(openFileDialog1.FileName));
                try
                {
                    pictureBox1.Image = new Bitmap(sourcestring);
                }
                catch
                {
                    MessageBox.Show("选择的图片有误！","提示",MessageBoxButtons.OK);
                }
                InitListbox();
            }
        }
        //删除
        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                pictureBox1.Image.Dispose();
                pictureBox1.Image = null;
                File.Delete(files[listBox1.SelectedIndex]);
                InitListbox();
            }
        }
        private void InitListbox()
        {
            files = Directory.GetFiles(Application.StartupPath + "\\Media\\BlackGround");
            listBox1.Items.Clear();
            foreach (string filename in files)
            {
                listBox1.Items.Add(Path.GetFileName(filename));
            }
            if (files.Length > 0)
            {
                pictureBox1.Image = new Bitmap(files[0]);
            }
        }
    }
}
