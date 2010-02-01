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
    public partial class DeleteModelForm : Form
    {
        public DeleteModelForm()
        {
            InitializeComponent();
        }

        private void DeleteModelForm_Load(object sender, EventArgs e)
        {
            InitModelList();
        }
        //删除
        private void button1_Click(object sender, EventArgs e)
        {
            //查看当前模型列表中是否有选中的项，没有则返回，当单模型列表是会响应该事伯
            if (listView1.SelectedItems.Count < 0)
                return;
            //得到当前选择模型的名称
            string ModelName = listView1.SelectedItems[0].Text;
            string group = listView1.SelectedItems[0].Group.Header;
            MainOgreForm.Singleton.ModelGroup.DeleteMesh(ModelName, group);

            MainOgreForm.Singleton.LoadAllModel();
            InitModelList();
            MessageBox.Show("删除模型成功,重启程序后生效！", "删除模型库模型");
        }
        private void InitModelList()
        {
            listView1.Items.Clear();

            ModelManage ModelGroup = MainOgreForm.Singleton.ModelGroup;
            string path = Application.StartupPath;
            //载入“无预览”图
            System.Drawing.Image image = new Bitmap(path + "\\yulan.JPG");
            imageList1.Images.Add(image);
            int imagecount = 1;
            ListViewGroup group = new ListViewGroup("建筑物");
            listView1.Groups.Add(group);
            foreach (ModelStruct temp in ModelGroup.建筑物)
            {
                string imagename = Application.StartupPath + "\\Media\\Model\\" + temp.ModelName.Substring(0, temp.ModelName.Length - 5) + ".jpg";
                //为个地方是用来判断是否有和模型名同名的图片，如果有，则为模型的缩略图，将其载入到模型列表的显示中。
                if (File.Exists(imagename))
                {
                    image = new Bitmap(imagename);
                    imageList1.Images.Add(image);
                    listView1.Items.Add(temp.Name, imagecount).Group = listView1.Groups[0];
                    imagecount++;
                }
                else//如果模型没有缩略图，则设置为“无预览”图
                {
                    listView1.Items.Add(temp.Name, 0).Group = listView1.Groups[0];
                }
            }
            group = new ListViewGroup("植物");

            listView1.Groups.Add(group);
            foreach (ModelStruct temp in ModelGroup.植物)
            {
                string imagename = Application.StartupPath + "\\Media\\Model\\" + temp.ModelName.Substring(0, temp.ModelName.Length - 5) + ".jpg";
                //为个地方是用来判断是否有和模型名同名的图片，如果有，则为模型的缩略图，将其载入到模型列表的显示中。
                if (File.Exists(imagename))
                {
                    image = new Bitmap(imagename);
                    imageList1.Images.Add(image);
                    listView1.Items.Add(temp.Name, imagecount).Group = listView1.Groups[1];
                    imagecount++;
                }
                else//如果模型没有缩略图，则设置为“无预览”图
                {
                    listView1.Items.Add(temp.Name, 0).Group = listView1.Groups[1];
                }
            }
            group = new ListViewGroup("室内元素");

            listView1.Groups.Add(group);
            foreach (ModelStruct temp in ModelGroup.室内元素)
            {
                string imagename = Application.StartupPath + "\\Media\\Model\\" + temp.ModelName.Substring(0, temp.ModelName.Length - 5) + ".jpg";
                //为个地方是用来判断是否有和模型名同名的图片，如果有，则为模型的缩略图，将其载入到模型列表的显示中。
                if (File.Exists(imagename))
                {
                    image = new Bitmap(imagename);
                    imageList1.Images.Add(image);
                    listView1.Items.Add(temp.Name, imagecount).Group = listView1.Groups[2];
                    imagecount++;
                }
                else//如果模型没有缩略图，则设置为“无预览”图
                {
                    listView1.Items.Add(temp.Name, 0).Group = listView1.Groups[2];
                }
            }
            group = new ListViewGroup("杂项");

            listView1.Groups.Add(group);
            foreach (ModelStruct temp in ModelGroup.杂项)
            {
                string imagename = Application.StartupPath + "\\Media\\Model\\" + temp.ModelName.Substring(0, temp.ModelName.Length - 5) + ".jpg";
                //为个地方是用来判断是否有和模型名同名的图片，如果有，则为模型的缩略图，将其载入到模型列表的显示中。
                if (File.Exists(imagename))
                {
                    image = new Bitmap(imagename);
                    imageList1.Images.Add(image);
                    listView1.Items.Add(temp.Name, imagecount).Group = listView1.Groups[3];
                    imagecount++;
                }
                else//如果模型没有缩略图，则设置为“无预览”图
                {
                    listView1.Items.Add(temp.Name, 0).Group = listView1.Groups[3];
                }
            }
        }
    }
}
