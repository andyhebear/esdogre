using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Drawing.Drawing2D;

namespace MyOgre
{
    public partial class SetRoadStyleForm : Form
    {
        public SetRoadStyleForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 判断字符串是否是数字
        /// </summary>
        /// <param name="strline"></param>
        /// <returns></returns>
        public bool IsNumber(string strline)
        {
            if (Regex.IsMatch(strline.Trim(), @"^((\d+)|-|.)?([1-9]\d+|\d)((\.\d+)|(\.))?$"))
                return true;
            else
                return false;
        }
        private void DrawRoad()
        {
            //设置路的样式
            Pen p1 = new Pen(road_button.BackColor, int.Parse(road_textBox.Text));
            Pen p2 = new Pen(line_button.BackColor, int.Parse(line_textBox.Text));
            p1.StartCap = LineCap.Round;
            p1.EndCap = LineCap.Round;
            p2.StartCap = LineCap.Round;
            p2.EndCap = LineCap.Round;
            p2.DashStyle = DashStyle.Custom;
            p2.DashPattern = new float[] { 16, 6 };

            Graphics g = panel1.CreateGraphics();

            g.Clear(Color.White);

            g.DrawLine(p1, 30, 50, 250, 50);
       
            g.DrawLine(p2, 30, 50, 250,50);

            g.Dispose();
            p2.Dispose();
            p1.Dispose();
            
        }

        private void SetRoadStyleForm_Load(object sender, EventArgs e)
        {
            DrawRoad();
        }

        private void road_button_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = road_button.BackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                road_button.BackColor = colorDialog1.Color;
                DrawRoad();
            }
        }

        private void line_button_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = line_button.BackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                line_button.BackColor = colorDialog1.Color;
                DrawRoad();
            }
        }

        private void road_textBox_TextChanged(object sender, EventArgs e)
        {
            
            if (!IsNumber(road_textBox.Text))
            {
                road_textBox.Text = "10";
            }
            DrawRoad();
        }

        private void line_textBox_TextChanged(object sender, EventArgs e)
        {
            if (!IsNumber(line_textBox.Text))
            {
                road_textBox.Text = "1";
            }
            DrawRoad();
        }

        private void SetRoadStyleForm_Paint(object sender, PaintEventArgs e)
        {
            DrawRoad();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            road_textBox.Text = "10";
            line_textBox.Text = "1";
            road_button.BackColor = Color.FromArgb(100, 100, 100);
            line_button.BackColor = Color.Yellow;
        }
    }
}
