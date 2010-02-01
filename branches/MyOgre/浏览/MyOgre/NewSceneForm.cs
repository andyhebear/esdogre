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
    public partial class NewSceneForm : Form
    {
        public NewSceneForm()
        {
            InitializeComponent();
        }

        private void NewSceneForm_Load(object sender, EventArgs e)
        {
            InitListView();
        }
        /// <summary>
        /// 初始化地面材质列表
        /// </summary>
        private void InitListView()
        {
            //得到应用程序启动目录
            string path = Application.StartupPath;
            //得到地面图片所存放的路径
            string[] filenames = Directory.GetFiles(path + "\\Media\\BlackGround");
            int i=0;
            //得到地面纹理文件
            foreach (string filename in filenames)
            {
                string ext = Path.GetExtension(filename).ToLower();
                //如果是图片文件，加入到列表框中
                if (ext.Equals(".jpg") || ext.Equals(".png")||ext.Equals(".jpeg"))
                {
                    string name=Path.GetFileName(filename);
                    Image image = new Bitmap(filename);
                    imageList1.Images.Add(image);
                    blackground_listView.Items.Add(name, i);
                    i++;
                }
            }
        }
    }
}
