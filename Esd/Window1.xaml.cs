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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Mogre;
using System.ComponentModel;
using System.Windows.Media.Animation;
using Esd.Tool;
using EsdCommon;
using EsdCommon.Tool;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace Esd
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        /// <summary>
        /// 场景视图变换工具
        /// </summary>
        ViewportTool viewporttool = new ViewportTool();
        public Window1()
        {
            InitializeComponent();
            /// <summary>
            /// 工具管理类对象
            /// </summary>
            new ToolManage();
        }

        private void _ogre_OnInitialised(object sender, RoutedEventArgs e)
        {
            var sceneMgr = _ogreImage.SceneManager;
            // Set ambient light
            sceneMgr.AmbientLight = new ColourValue(0.5f, 0.5f, 0.5f);

            // Create a point light
            Light l = sceneMgr.CreateLight("MainLight");
            // Accept default settings: point light, white diffuse, just set position
            // NB I could attach the light to a SceneNode if I wanted it to move automatically with
            //  other objects, but I don't
            l.Position = new Vector3(20, 80, 50);


            // Set up a material for the skydome
            MaterialPtr skyMat = MaterialManager.Singleton.Create("SkyMat",
                ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
            // Perform no dynamic lighting on the sky
            skyMat.SetLightingEnabled(false);
            // Use a cloudy sky
            TextureUnitState t = skyMat.GetTechnique(0).GetPass(0).CreateTextureUnitState("clouds.jpg");
            // Scroll the clouds
            t.SetScrollAnimation(0.15f, 0f);

            // System will automatically set no depth write

            // Create a skydome
            sceneMgr.SetSkyDome(true, "SkyMat", -5, 2);
            EsdSceneManager.CreateSceneManager(_ogreImage);
            inittool();
        }
        private void RenterTargetControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_ogreImage == null) return;

            _ogreImage.ViewportSize = e.NewSize;
        }
        void _image_PreRender(object sender, System.EventArgs e)
        {
            //更新场景视图
            viewporttool.UpdateViewport();
        }
        /// <summary>
        /// 初始或啊工具
        /// </summary>
        private void inittool()
        {
            //漫游工具
            ToolManage.Singleton.AddTool(new PanToolClass());
            //设置当前工具为漫游
            ToolManage.Singleton.ToolType = typeof(PanToolClass);
        }
        private void Window1_OnLoaded(object sender, RoutedEventArgs e)
        {
            _ogreImage.InitOgreAsync();
            LoadModelFiles();
            //向工具管理器增加工具

            /*  panel1.MouseDown += new MouseEventHandler(ToolManageObject.MouseDown);
              panel1.MouseMove += new MouseEventHandler(ToolManageObject.MouseMove);
              panel1.MouseUp += new MouseEventHandler(ToolManageObject.MouseUp);
              this.KeyDown += new KeyEventHandler(ToolManageObject.KeyDown);
              this.KeyUp += new KeyEventHandler(ToolManageObject.KeyUp);*/


        }
        private void Window1_OnClosing(object sender, CancelEventArgs e)
        {
            RenterTargetControl.Source = null;
            EsdSceneManager.Singleton.MaterialPtr = null;
            _ogreImage.Dispose();
        }

        private void NewScene_Click(object sender, RoutedEventArgs e)
        {
            NewSceneTool tool = new NewSceneTool();
            tool.Click();

        }
        #region 工具栏透视俯视旋转工具事件
        private void Viewport_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
            //img.CaptureMouse();
            switch (img.Name)
            {
                case "img_down":
                    viewporttool.MouseDown(ViewportToolEnum.ViewportDown);
                    break;
                case "img_up":
                    viewporttool.MouseDown(ViewportToolEnum.ViewportUp);
                    break;
                case "img_left":
                    viewporttool.MouseDown(ViewportToolEnum.ViewportLeft);
                    break;
                case "img_right":
                    viewporttool.MouseDown(ViewportToolEnum.ViewportRight);
                    break;
            }
            e.Handled = true;
        }

        private void Viewport_MouseLeave(object sender, MouseEventArgs e)
        {
            //viewporttool.MouseLeave(ViewportToolEnum.None);
        }

        private void Viewport_MouseUp(object sender, MouseButtonEventArgs e)
        {
            viewporttool.MouseLeave(ViewportToolEnum.None);
        }
        #endregion

        private void OgreMouseUp(object sender, MouseButtonEventArgs e)
        {
            ToolManage.Singleton.MouseUp(sender, e);
        }
        private void OgreMouseDown(object sender, MouseButtonEventArgs e)
        {
            ToolManage.Singleton.MouseDown(sender, e);
            /* Point position = e.GetPosition(RenterTargetControl);

             MessageBox.Show(position.ToString());*/
        }

        private void OgreMouseMove(object sender, MouseEventArgs e)
        {
            ToolManage.Singleton.MouseMove(sender, e);
        }

        private void OgreKeyDown(object sender, KeyEventArgs e)
        {
            ToolManage.Singleton.KeyDown(sender, e);
            e.Handled = true;
        }

        private void OgreKeyUp(object sender, KeyEventArgs e)
        {
            ToolManage.Singleton.KeyUp(sender, e);
        }
        //复位
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _ogreImage.RestCamer();
        }
        #region 模型操作
        /// <summary>
        /// 模型文件夹中的列表，用来在导入模型时进行判断
        /// </summary>
        List<string> ModelFolderFiles = new List<string>();
        /// <summary>
        /// 载入模型文件夹中的文件名，用来做导入模型时用。
        /// </summary>
        private void LoadModelFiles()
        {         
            String appStartPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);//得到应用程序启动目录
            string path = appStartPath + "\\Media\\Model";
            string[] filenames = Directory.GetFiles(path);

            foreach (string filename in filenames)
            {
                string name = System.IO.Path.GetFileName(filename);
                ModelFolderFiles.Add(name);
            }
        }
        /// <summary>
        /// 模型分组对象
        /// </summary>
        public ModelManage ModelGroup = new ModelManage();
        //打开模型库分组文件
        private void OpenModelGroup()
        {

            String appStartPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);//得到应用程序启动目录
            if (File.Exists(appStartPath + "\\ModelGroup.XML"))
            {
                //打开场景文件， 通过XML的返序列化读取文件，
                XmlSerializer serializer = new XmlSerializer(typeof(ModelManage));
                FileStream file = File.OpenRead(appStartPath + "\\ModelGroup.XML");
                ModelGroup = (ModelManage)serializer.Deserialize(file);
                file.Close();
            }
            else
            {
                ModelGroup = new ModelManage();
            }
        }

        /// <summary>
        /// 载入模型到面板上的
        /// </summary>
        public void LoadAllModel()
        {
            /*
            listView1.Items.Clear();
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
            }*/
        }
        #endregion

    }
}
