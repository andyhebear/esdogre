using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace MyOgre
{
    public partial class ImportModelForm : Form
    {
        public ImportModelForm()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (!IsNumber(textBox2.Text))
            {
                textBox2.Text = "1";
            }
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
    }
}
