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
using System.IO;
using System.Diagnostics;

namespace Esd
{
    /// <summary>
    /// Window2.xaml 的交互逻辑
    /// </summary>
    public partial class NewSceneWindow : Window
    {
        public NewSceneWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        //窗体载入事件
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitListView();
            listView1.SelectedIndex = 0;
        }
        /// <summary>
        /// 初始化地面材质列表
        /// </summary>
        private void InitListView()
        {
            //得到应用程序启动目录
            String appStartPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            //得到地面图片所存放的路径
            string[] filenames = Directory.GetFiles(appStartPath + "\\Media\\BlackGround");
            int i = 0;
            //得到地面纹理文件
            foreach (string filename in filenames)
            {
                string ext = System.IO.Path.GetExtension(filename).ToLower();
                //如果是图片文件，加入到列表框中
                if (ext.Equals(".jpg") || ext.Equals(".png") || ext.Equals(".jpeg"))
                {
                    string name = System.IO.Path.GetFileName(filename);
                    listView1.Items.Add(new ListViewItem() { Content = filename });
                }
            }
        }

        private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(((ListViewItem)listView1.SelectedItem).Content.ToString());
            bi.EndInit();
            image1.Source = bi;
        }


    }
}
