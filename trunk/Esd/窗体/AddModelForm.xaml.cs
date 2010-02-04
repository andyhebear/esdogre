using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Esd
{
    /// <summary>
    /// AddModelForm.xaml 的交互逻辑
    /// </summary>
    public partial class AddModelForm : Window
    {
        public string picturename = "";
        public string vidoname = "";

        public AddModelForm()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void textBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (picture_textBox.Text == "")
                picturename = "";
        }

        private void vido_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (vido_textBox.Text == "")
                vidoname = "";
        }
    }
}
