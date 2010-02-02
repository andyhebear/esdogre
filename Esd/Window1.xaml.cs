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
            //当窗口改变大小时。会重表创建渲染窗口，这时，场景管理器中的所有结点会被销毁，所以这需重新创建。
            EsdSceneManager.Singleton.MainNode = sceneMgr.RootSceneNode.CreateChildSceneNode("ogreNode");
            EsdSceneManager.Singleton.AddmodelNode = sceneMgr.RootSceneNode.CreateChildSceneNode();
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
            if (ToolManage.Singleton.ToolType != typeof(AddModelTool))
            {

                if (EsdSceneManager.Singleton.AddmodelNode == null)
                    return;
                EsdSceneManager.Singleton.AddmodelNode.Position = new Vector3(1000000, 100000, 100000);
            }
        }
        /// <summary>
        /// 初始或啊工具
        /// </summary>
        private void inittool()
        {
            //漫游工具
            ToolManage.Singleton.AddTool(new PanToolClass());
            ToolManage.Singleton.AddTool(new AddModelTool());
            SelectModelTool smt = new SelectModelTool();
            smt.SetModifyTextEd = SetModifyTextEd;
            ToolManage.Singleton.AddTool(smt);
            //设置当前工具为漫游
            ToolManage.Singleton.ToolType = typeof(PanToolClass);
        }
        public void SetModifyTextEd(bool flag, SceneNode node)
        {

        }
        private void Window1_OnLoaded(object sender, RoutedEventArgs e)
        {
            _ogreImage.InitOgreAsync();
            EsdSceneManager.CreateSceneManager(_ogreImage);
            OpenModelGroup();
            LoadAllModel();
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
   
        //打开模型库分组文件
        private void OpenModelGroup()
        {

            String appStartPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);//得到应用程序启动目录
            if (File.Exists(appStartPath + "\\ModelGroup.XML"))
            {
                //打开场景文件， 通过XML的返序列化读取文件，
                XmlSerializer serializer = new XmlSerializer(typeof(ModelManage));
                FileStream file = File.OpenRead(appStartPath + "\\ModelGroup.XML");
                EsdSceneManager.Singleton.ModelGroup = (ModelManage)serializer.Deserialize(file);
                file.Close();
            }
            else
            {
                EsdSceneManager.Singleton.ModelGroup = new ModelManage();
            }
        }
        String appStartPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);//得到应用程序启动目录
        private void AddModelGroup(BitmapImage wuyulanbi,List<ModelStruct> modelgroup,string header)
        {
            GroupBox groupbox = new GroupBox();
            groupbox.Header = header;
            moxingwrappanel.Children.Add(groupbox);
            WrapPanel wrappanel = new WrapPanel();
            groupbox.Content = wrappanel;
            foreach (ModelStruct temp in modelgroup)
            {
                BitmapImage bi = wuyulanbi;
                StackPanel stackpanel = new StackPanel();
                string imagename = appStartPath + "\\Media\\Model\\" + temp.ModelName.Substring(0, temp.ModelName.Length - 5) + ".jpg";
                if (File.Exists(imagename))
                {
                    bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri(imagename);
                    bi.EndInit();
                }

                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.Source = bi;
                image.Width = 64;
                image.Height = 64;
                image.Stretch = Stretch.Fill;
                stackpanel.Children.Add(image);
                stackpanel.Children.Add(new Label() { Content = temp.Name });
                stackpanel.Margin = new Thickness(4);                
                stackpanel.MouseDown += new MouseButtonEventHandler(stackpanel_MouseDown);
                wrappanel.Children.Add(stackpanel);
            }
        }

        void stackpanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach (GroupBox gb in moxingwrappanel.Children)
            {
                foreach(StackPanel sp in ((WrapPanel)gb.Content).Children)
                {
                    sp.Background = Brushes.White;
                }
            }
            StackPanel spanel = sender as StackPanel;
            spanel.Background = Brushes.BlueViolet;
           
            AddModelTool amt = ToolManage.Singleton.GetTool(typeof(AddModelTool))as AddModelTool;

            amt.SetAddModel(((Label)spanel.Children[1]).Content.ToString(), ((GroupBox)((WrapPanel)spanel.Parent).Parent).Header.ToString());
            if (EsdSceneManager.Singleton.IsStarEdit)
            {               
                //设置当前工具为漫游
                ToolManage.Singleton.ToolType = typeof(AddModelTool);
            }
            else
            {
                MessageBox.Show("请新建一个场景");
            }
        }
        /// <summary>
        /// 载入模型到面板上的
        /// </summary>
        public void LoadAllModel()
        { 

            BitmapImage wuyulanbi = new BitmapImage();
            wuyulanbi.BeginInit();
            wuyulanbi.UriSource = new Uri(appStartPath + "\\yulan.JPG");
            wuyulanbi.EndInit();          
           
            moxingwrappanel.Children.Clear();
            AddModelGroup(wuyulanbi,EsdSceneManager.Singleton. ModelGroup.建筑物, "建筑物");
            AddModelGroup(wuyulanbi, EsdSceneManager.Singleton.ModelGroup.植物, "植物");
            AddModelGroup(wuyulanbi, EsdSceneManager.Singleton.ModelGroup.室内元素, "室内元素");
            AddModelGroup(wuyulanbi, EsdSceneManager.Singleton.ModelGroup.杂项, "杂项");          
        }
        #endregion

        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            //设置当前工具为漫游
            //panel1.Cursor = Cursors.Arrow;
           
            ToolManage.Singleton.ToolType = typeof(PanToolClass);
        }

        private void Image_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            //设置当前工具为漫游
            ToolManage.Singleton.ToolType = typeof(SelectModelTool);
        }

        private void Image_MouseDown_3(object sender, MouseButtonEventArgs e)
        {

            if (EsdSceneManager.Singleton.CurrentOperateNode != null)
            {
                ModelEntryStruct mm = EsdSceneManager.Singleton.ModelDataManage.modelEntry.GetModelEntry(EsdSceneManager.Singleton.CurrentOperateNode);
                if (mm != null)
                {
                    AddModelForm dlg = new AddModelForm();
                    dlg.name_textBox.Text = mm.名称;
                    dlg.remark_textBox.Text = mm.备注属性;
                    dlg.picture_textBox.Text = mm.图片名称;
                    dlg.vido_textBox.Text = mm.视频名称;

                    if ((bool)dlg.ShowDialog())
                    {
                        mm.名称 = dlg.name_textBox.Text;
                        mm.备注属性 = dlg.remark_textBox.Text;
                        if (dlg.picture_textBox.Text != "")
                        {
                            if (dlg.picture_textBox.Text != mm.图片名称)
                            {
                                mm.图片名称 = dlg.picture_textBox.Text;
                                FileStream file = File.OpenRead(dlg.picturename);
                                BinaryReader reader = new BinaryReader(file);
                                mm.图片 = reader.ReadBytes((int)file.Length);
                                reader.Close();
                                file.Close();
                            }
                        }
                        else
                        {
                            mm.图片 = null;
                            mm.图片名称 = "";
                        }
                        if (dlg.vido_textBox.Text != "")
                        {
                            if (dlg.vido_textBox.Text != mm.视频名称)
                            {
                                mm.视频名称 = dlg.vido_textBox.Text;
                                FileStream file = File.OpenRead(dlg.vidoname);
                                BinaryReader reader = new BinaryReader(file);
                                mm.视频 = reader.ReadBytes((int)file.Length);
                                reader.Close();
                                file.Close();
                            }
                        }
                        else
                        {
                            mm.视频名称 = "";
                            mm.视频 = null;
                        }
                    }
                }
            }
        }

        private void Image_MouseDown_4(object sender, MouseButtonEventArgs e)
        {
            //panel1.Cursor = Cursors.Cross;
            //设置当前工具为
            ToolManage.Singleton.ToolType = typeof(AddWaterTool);
            //ToolManageObject.ToolType = typeof(AddWaterTool);
        }

    }
}
