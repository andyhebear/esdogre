using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Mogre;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Diagnostics;

namespace MyOgre
{
    public partial class MainOgreForm : Form
    {
        /// <summary>
        /// OGRE对象
        /// </summary>
        OgreView MogreWin;
        /// <summary>
        /// 工具管理类对象
        /// </summary>
        public ToolManage ToolManageObject = new ToolManage();
        /// <summary>
        /// 菜单功能管理
        /// </summary>
        List<AbstractMenuTool> MenuTool = new List<AbstractMenuTool>();
        //模型的管理（保存和读取）
        public ModelDataMaintenance ModelDataManage = new ModelDataMaintenance();
        /// <summary>
        /// 模型分组对象
        /// </summary>
        public ModelManage ModelGroup = new ModelManage();
        //当前打开的文件名
        public string OpenFileName = "";
        /// <summary>
        /// 背景材质引用
        /// </summary>
        public MaterialPtr materialptr = null;
        //当前场景是否开始编辑
        public bool IsStarEdit = false;
        /// <summary>
        /// 当前的工具栏操作，
        /// 0；没有任何操作
        /// 1；躺下
        /// 2；起来
        /// 3；左转
        /// 4；右转
        /// </summary>
        private int tooloperateflag = 0;
        /// <summary>
        /// 动态创建名称的个数，也用来处理创建唯一字符时的重复问题
        /// </summary>
        static private int namecount = 0;
        /// <summary>
        /// 模型文件夹中的列表，用来在导入模型时进行判断
        /// </summary>
        List<string> ModelFolderFiles = new List<string>();
        /// <summary>
        /// 路的相关属性属性
        /// 一下是默认值
        /// </summary>
        public int RoadWidth = 10;
        public int LineWidth = 1;
        public Color RoadColor = Color.FromArgb(100, 100, 100);
        public Color LineColor = Color.Yellow;
        static MainOgreForm singleton;
        static public MainOgreForm Singleton
        {
            get
            {
                return singleton;
            }
        }
        public MainOgreForm()
        {
            singleton = this;
            InitializeComponent();
            MogreWin = new OgreView(panel1.Handle);
            this.Disposed += new EventHandler(MainOgreForm_Disposed);
        }

        void MainOgreForm_Disposed(object sender, EventArgs e)
        {
            MogreWin.Dispose();
        }

        private void MainOgreForm_Load(object sender, EventArgs e)
        {
            panel1.MouseDown += new MouseEventHandler(ToolManageObject.MouseDown);
            panel1.MouseMove += new MouseEventHandler(ToolManageObject.MouseMove);
            panel1.MouseUp += new MouseEventHandler(ToolManageObject.MouseUp);
            this.KeyDown += new KeyEventHandler(ToolManageObject.KeyDown);
            this.KeyUp += new KeyEventHandler(ToolManageObject.KeyUp);

            this.MouseWheel += new MouseEventHandler(MainForm_MouseWheel);
           // listView1.MouseWheel += new MouseEventHandler(MainForm_MouseWheel);
            treeView1.MouseWheel += new MouseEventHandler(MainForm_MouseWheel);
          
            //打开场景文件
            MenuTool.Add(new OpenScene(open_item));
           

            //向工具管理器增加工具
            //漫游工具
            ToolManageObject.AddTool(new PanToolClass());
            //选择模型工具
            ToolManageObject.AddTool(new SelectModelTool());

            //增加注记工具
            ToolManageObject.AddTool(new AddBillBoardTool());
            //选择广告牌
            ToolManageObject.AddTool(new SelectBillBoardTool());
            //设置当前工具为漫游
            ToolManageObject.ToolType = typeof(PanToolClass);

            //初始化ogre环境
            OgreView.Singleton.InitMogre();
            //创建渲染窗口
            MogreWin.CreateWindows((uint)panel1.Width, (uint)panel1.Height);
            //采用定时器渲染场景
            timer1.Enabled = true;
            //载入模型分组数据
           // OpenModelGroup();
            //将模型库中的模型加开到模型列表
            //LoadAllModel();

            //初始化树控件的根节点 
            TreeNode rootnode = new TreeNode();
            rootnode.Text = "模型列表";
            treeView1.Nodes.Add(rootnode);

            //实始化设置编辑框
            SetModifyTextEd(false, null);
            //每次启动时，清空广告牌图片
            ClearBillImage();
            //载入模型文件夹文件名
            LoadModelFiles();
        }
      

        private void 删除模型ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OgreView.Singleton.CurrentOperateNode != null)
            {
                //
                ModelDataManage.AddRemoveState(OgreView.Singleton.CurrentOperateNode, 2);//删除模型节点
                OgreView.Singleton.mainNode.RemoveChild(OgreView.Singleton.CurrentOperateNode);//删除在场景中的节点
            }
            else
            {
                MessageBox.Show("当前没有需删除的节点!");
            }
        }
        //定时器
        private void timer1_Tick(object sender, EventArgs e)
        {
            ToolManageObject.TimerTick(sender, e);

          

            switch (tooloperateflag)
            {
                case 1:
                    OgreView.Singleton.CamerLookdownDegree -= System.Math.PI / 180;
                    MogreWin.UpdataCamera();
                    break;
                case 2:
                    OgreView.Singleton.CamerLookdownDegree += System.Math.PI / 180;
                    MogreWin.UpdataCamera();
                    break;
                case 3:
                    OgreView.Singleton.CamerRoateDegree += System.Math.PI / 180;
                    MogreWin.UpdataCamera();
                    break;
                case 4:
                    OgreView.Singleton.CamerRoateDegree -= System.Math.PI / 180;
                    MogreWin.UpdataCamera();
                    break;
                default:
                    break;
            }

            if (toolStripMenuItem_sun.Checked)
            {
                //太阳
                OgreView.Singleton.sunNode.Rotate(Vector3.UNIT_Y, new Degree(0.5f));
               
            }

            MogreWin.RenderView();

        }
        //场景改变大小
        private void panel1_Resize(object sender, EventArgs e)
        {
            //重表创建渲染窗口
            MogreWin.CreateWindows((uint)panel1.Width, (uint)panel1.Height);
        }
        //中间滚轮
        void MainForm_MouseWheel(object sender, MouseEventArgs e)
        {
            if (OgreView.Singleton.PanState)
                return;
            if (e.Delta > 0)//放大
            {
                if (MogreWin.CamerDistanceLock - MogreWin.CamerDistanceLock * 0.05f < 10)
                    return;
                MogreWin.CamerDistanceLock -= MogreWin.CamerDistanceLock * 0.05f;
                MogreWin.UpdataCamera();
            }
            else//缩小
            {
                if (MogreWin.CamerDistanceLock + MogreWin.CamerDistanceLock * 0.05f > 1000)
                    return;
                MogreWin.CamerDistanceLock += MogreWin.CamerDistanceLock * 0.05f;
                MogreWin.UpdataCamera();
            }
        }

        //得到当前时间，在创建模型实体时用，可以达到唯一
        public static string GetDateName()
        {
            namecount++;
            return string.Format("{0}{1}{2}{3}{4}{5}{6}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, namecount);
           
        }
        //移动
        private void move_item_Click(object sender, EventArgs e)
        {
            //设置当前工具为漫游
            ToolManageObject.ToolType = typeof(PanToolClass);
        }
        //选择模型
        private void select_item_Click(object sender, EventArgs e)
        {
            //设置当前工具为漫游
            ToolManageObject.ToolType = typeof(SelectModelTool);
        }
        //增加注记工具
        private void addremark_item_Click(object sender, EventArgs e)
        {
            ToolManageObject.ToolType = typeof(AddBillBoardTool);
        }
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseScene();
        }
        //当更编辑框模型属性时，更新模型结点的当前状态
        private void shuxing_textBox_TextChanged(object sender, EventArgs e)
        {


            if (OgreView.Singleton.CurrentOperateNode == null)
                return;


            //得到结点所对应的相应信息。以便结点的旋转
            ModelEntryStruct modelentry = null;
            Entity en = OgreView.Singleton.CurrentOperateNode.GetAttachedObject(0) as Entity;
            for (int i = 0; i < ModelDataManage.modelEntry.模型链表.Count; i++)
            {
                if (ModelDataManage.modelEntry.模型链表[i].实体名 == en.Name)
                {
                    modelentry = ModelDataManage.modelEntry.模型链表[i];
                    break;
                }
            }
            if (modelentry == null)
                return;

          

            TextBox textbox = sender as TextBox;
            if (textbox.Text == "")
                return;

            
            if (textbox.Name.Equals("modelname_textBox"))
            {
                modelentry.名称 = textbox.Text;
                //更新树节点
                UpdataNodeTree(modelentry);
                return;
            }
            //输入是否合法
            if (!IsNumber(textbox.Text))
            {
                textbox.Text = "";
                return;
            }

            //模型的缩放
            if (textbox.Name.Equals("scalex_textBox"))
            {
                Vector3 v = OgreView.Singleton.CurrentOperateNode.GetScale();
                v.x = float.Parse(textbox.Text);
                OgreView.Singleton.CurrentOperateNode.SetScale(v);
            }
            else if (textbox.Name.Equals("scaley_textBox"))
            {
                Vector3 v = OgreView.Singleton.CurrentOperateNode.GetScale();
                v.y = float.Parse(textbox.Text);
                OgreView.Singleton.CurrentOperateNode.SetScale(v);
            }
            else if (textbox.Name.Equals("scalez_textBox"))
            {
                Vector3 v = OgreView.Singleton.CurrentOperateNode.GetScale();
                v.z = float.Parse(textbox.Text);
                OgreView.Singleton.CurrentOperateNode.SetScale(v);
            }
            else if (textbox.Name.Equals("roatx_textBox"))//模型的旋转
            {
                if (modelentry == null)
                    return;
                float xx = float.Parse(textbox.Text) * (float)(System.Math.PI / 180);
                Quaternion x = new Quaternion(new Radian(xx), Vector3.UNIT_X);
                Quaternion y = new Quaternion(new Radian(modelentry.旋转角度.y), Vector3.UNIT_Y);
                Quaternion z = new Quaternion(new Radian(modelentry.旋转角度.z), Vector3.UNIT_Z);
                OgreView.Singleton.CurrentOperateNode.Orientation = new Quaternion(1, 0, 0, 0);
                //四元素相乘得到旋转角度。
                OgreView.Singleton.CurrentOperateNode.Orientation *= x * y * z;
                //更新更改过后的状态，以下同理
                ModelDataManage.UpdateModelState(OgreView.Singleton.CurrentOperateNode, 'x', xx);
                return;
            }
            else if (textbox.Name.Equals("roaty_textBox"))
            {

                float yy = float.Parse(textbox.Text) * (float)(System.Math.PI / 180);
                Quaternion x = new Quaternion(new Radian(modelentry.旋转角度.x), Vector3.UNIT_X);
                Quaternion y = new Quaternion(new Radian(yy), Vector3.UNIT_Y);
                Quaternion z = new Quaternion(new Radian(modelentry.旋转角度.z), Vector3.UNIT_Z);
                OgreView.Singleton.CurrentOperateNode.Orientation = new Quaternion(1, 0, 0, 0);
                OgreView.Singleton.CurrentOperateNode.Orientation *= x * y * z;
                ModelDataManage.UpdateModelState(OgreView.Singleton.CurrentOperateNode, 'y', yy);
                return;
            }
            else if (textbox.Name.Equals("roatz_textBox"))
            {

                float zz = float.Parse(textbox.Text) * (float)(System.Math.PI / 180);
                Quaternion x = new Quaternion(new Radian(modelentry.旋转角度.x), Vector3.UNIT_X);
                Quaternion y = new Quaternion(new Radian(modelentry.旋转角度.y), Vector3.UNIT_Y);
                Quaternion z = new Quaternion(new Radian(zz), Vector3.UNIT_Z);
                OgreView.Singleton.CurrentOperateNode.Orientation = new Quaternion(1, 0, 0, 0);
                OgreView.Singleton.CurrentOperateNode.Orientation *= x * y * z;
                ModelDataManage.UpdateModelState(OgreView.Singleton.CurrentOperateNode, 'z', zz);
                return;
            }
            else if (textbox.Name.Equals("positionx_textBox"))//更新位置
            {
                Vector3 q = OgreView.Singleton.CurrentOperateNode.Position;
                q.x = float.Parse(textbox.Text);
                OgreView.Singleton.CurrentOperateNode.Position = q;
            }
            else if (textbox.Name.Equals("positiony_textBox"))
            {
                Vector3 q = OgreView.Singleton.CurrentOperateNode.Position;
                q.y = float.Parse(textbox.Text);
                OgreView.Singleton.CurrentOperateNode.Position = q;
            }
            else if (textbox.Name.Equals("positionz_textBox"))
            {
                Vector3 q = OgreView.Singleton.CurrentOperateNode.Position;
                q.z = float.Parse(textbox.Text);
                OgreView.Singleton.CurrentOperateNode.Position = q;
            }
            ModelDataManage.UpdateModelState(OgreView.Singleton.CurrentOperateNode, ' ', 0);
        }
        /// <summary>
        /// 更改修改属性编辑框可编辑状态。
        /// </summary>
        /// <param name="flag"></param>
        public void SetModifyTextEd(bool flag, SceneNode node)
        {


        }
        //当编辑框的上下按钮值改变时，响应的事件
        private void scalex_vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
           
        }

       
        #region 工具栏的操作，俯视，透视
        private void tooli_toolStripButton_MouseDown(object sender, MouseEventArgs e)
        {
            ToolStripButton item = sender as ToolStripButton;
            if (item.Name.Equals("tianxia_toolStripButton"))//躺下
            {
                tooloperateflag = 1;
            }
            else if (item.Name.Equals("shanglai_toolStripButton"))//起来
            {
                tooloperateflag = 2;
            }
            else if (item.Name.Equals("zuozuan_toolStripButton"))//左转
            {
                tooloperateflag = 3;
            }
            else if (item.Name.Equals("youzuan_toolStripButton"))//右转
            {
                tooloperateflag = 4;
            }
        }

        private void tooli_toolStripButton_MouseLeave(object sender, EventArgs e)
        {
            tooloperateflag = 0;
        }

        private void tooli_toolStripButton_MouseUp(object sender, MouseEventArgs e)
        {
            tooloperateflag = 0;
        }
        #endregion
        //复位
        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            MogreWin.RestCamer();
        }
        /// <summary>
        /// 雪效果开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void snowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripMenuItem_snow.Checked = !toolStripMenuItem_snow.Checked;
            MogreWin.ShowSnow(toolStripMenuItem_snow.Checked);            
        }
        /// <summary>
        /// 雨效果开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripMenuItem_Rain.Checked = !toolStripMenuItem_Rain.Checked;
            MogreWin.ShowRain(toolStripMenuItem_Rain.Checked);
        }
        //显示人物
        private void manToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripMenuItem_People.Checked = !toolStripMenuItem_People.Checked;
            MogreWin.ShowMan(toolStripMenuItem_People.Checked);
        }

        public void InitSRMTool()
        {
            toolStripMenuItem_snow.Checked = false;
            toolStripMenuItem_Rain.Checked = false;
            toolStripMenuItem_sun.Checked = false;
            toolStripMenuItem_People.Checked = false;
        }

        private void 选择注记ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //设置当前工具为漫游
            ToolManageObject.ToolType = typeof(SelectBillBoardTool);
        }

        private void 删除注记ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModelDataManage.RemoveBill(OgreView.Singleton.CurrentOperateNode);//删除模型节点
            OgreView.Singleton.billNode.RemoveChild(OgreView.Singleton.CurrentOperateNode);//删除在场景中的节点
        }
        /// <summary>
        /// 更新树节点
        /// </summary>
        public void UpdataTree()
        {
            treeView1.Nodes[0].Nodes.Clear();

            foreach (ModelEntryStruct entry in ModelDataManage.modelEntry.模型链表)
            {
                TreeNode node = new TreeNode();
                node.Text = entry.名称;
                node.Name = entry.实体名;
                treeView1.Nodes[0].Nodes.Add(node);
            }
        }

        /// <summary>
        /// 关闭场景
        /// </summary>
        public void CloseScene()
        {
            ModelDataManage.modelEntry.Clear();
            //清空场景地面
            ModelDataManage.modelEntry.模型名 = "";
            ModelDataManage.modelEntry.场景地面图片 = "";
            OgreView.Singleton.sceneMgr.ClearScene();
            //
            OpenFileName = "";
            //开始编辑
            IsStarEdit = false;

            OgreView.Singleton.floorNode = null;
            OgreView.Singleton.addmodelNode = null;
            OgreView.Singleton.mainNode = null;
            OgreView.Singleton.billNode = null;           
            OgreView.Singleton.CreateWindows((uint)panel1.Width, (uint)panel1.Height);
            ToolManageObject.InitTool();
            ToolManageObject.ToolType = typeof(PanToolClass);
            ClearTree();
        }
        public void AddNodeTree(ModelEntryStruct entry)
        {
            TreeNode node = new TreeNode();
            node.Text = entry.名称;
            node.Name = entry.实体名;
            treeView1.Nodes[0].Nodes.Add(node);
        }
        public void ClearTree()
        {
            treeView1.Nodes[0].Nodes.Clear();
        }
        public void UpdataNodeTree(ModelEntryStruct entry)
        {
            for (int i = 0; i < treeView1.Nodes[0].Nodes.Count; i++)
            {
                if (treeView1.Nodes[0].Nodes[i].Name.Equals(entry.实体名))
                {
                    treeView1.Nodes[0].Nodes[i].Text = entry.名称;
                    return;
                }
            }
        }
        public void RemoveNodeTree(ModelEntryStruct entry)
        {
            for (int i = 0; i < treeView1.Nodes[0].Nodes.Count; i++)
            {
                if (treeView1.Nodes[0].Nodes[i].Name.Equals(entry.实体名))
                {
                    treeView1.Nodes[0].Nodes[i].Remove();
                    return;
                }
            }
        }
        /// <summary>
        /// 树节点的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SelectSceneNode(e.Node.Name);
        }

        /// <summary>
        /// 根据创建模型的实体名，找到相应的模型。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public void SelectSceneNode(string name)
        {
            if (OgreView.Singleton.CurrentOperateNode != null)
            {
                OgreView.Singleton.CurrentOperateNode.ShowBoundingBox = false;
            }
            bool flag = true;

            //循环得到每个模型，和射线进行比较，得到和射线相交并且距屏幕最近的模型
            foreach (SceneNode node in OgreView.Singleton.mainNode.GetChildIterator())
            {
                if (node.GetAttachedObject(0).Name == name)
                {
                    flag = false;
                    OgreView.Singleton.CurrentOperateNode = node;
                    break;
                }
            }
            if (!flag)
            {
                //更新属性面板，
                SetModifyTextEd(true, OgreView.Singleton.CurrentOperateNode);

                OgreView.Singleton.CurrentOperateNode.ShowBoundingBox = true;
            }
        }
        /// <summary>
        /// 清空广告牌图片
        /// </summary>
        private void ClearBillImage()
        {
            string path = Application.StartupPath + "\\TempBillBoard\\";
            string[] filenames = Directory.GetFiles(path);
            foreach (string name in filenames)
            {
                File.Delete(name);
            }
        }
        /// <summary>
        /// 载入模型文件夹中的文件名，用来做导入模型时用。
        /// </summary>
        private void LoadModelFiles()
        {
            string path = Application.StartupPath + "\\Media\\Model";
            string[] filenames = Directory.GetFiles(path);

            foreach (string filename in filenames)
            {
                string name = Path.GetFileName(filename);
                ModelFolderFiles.Add(name);
            }
        }
        /// <summary>
        /// 模型文件是否已经存在于模型文件夹中
        /// </summary>
        /// <param name="name">需判断的文件名</param>
        /// <returns>true：文件已存在，false：文件不存在</returns>
        public bool IsTrueModelFile(string name)
        {
            name = name.ToLower();
            foreach (string str in ModelFolderFiles)
            {
                if (str.ToLower().Equals(name))
                    return true;
            }
            return false;
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string helpfile = "help.chm";
            if (File.Exists(helpfile))
            {
                Help.ShowHelp(this, helpfile);
            }
            else
            {
                MessageBox.Show("系统文件丢失！", "提示");
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
        //程序关闭
        private void MainOgreForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (MainOgreForm.Singleton.materialptr != null)
            {
                MainOgreForm.Singleton.materialptr.Dispose();
                MainOgreForm.Singleton.materialptr = null;
            }
           // SaveModelGroup();
        }

        private void 更换漫游方式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            更换漫游方式ToolStripMenuItem.Checked = !更换漫游方式ToolStripMenuItem.Checked;
            OgreView.Singleton.PanState = 更换漫游方式ToolStripMenuItem.Checked;
            if (OgreView.Singleton.PanState)
            {
               // manToolStripMenuItem.Checked = true;
                MogreWin.ShowMan(true);
                //更新角度
               // OgreView.Singleton.CamerRoateDegree = System.Math.PI / 90 ;
                //将摄像机位置设到模型的位置
                OgreView.Singleton.SetWADS();

                OgreView.Singleton.CamerDistanceLock = 20;
                OgreView.Singleton.CamerLookdownDegree =System.Math.PI/180*16 ;
                OgreView.Singleton.UpdataCamera();
              
            }

        }
        //效果雨
        private void toolStripMenuItem_Rain_Click(object sender, EventArgs e)
        {
            toolStripMenuItem_Rain.Checked = !toolStripMenuItem_Rain.Checked;
            MogreWin.ShowRain(toolStripMenuItem_Rain.Checked);
        }
        //效果雪
        private void toolStripMenuItem_snow_Click(object sender, EventArgs e)
        {
            toolStripMenuItem_snow.Checked = !toolStripMenuItem_snow.Checked;
            MogreWin.ShowSnow(toolStripMenuItem_snow.Checked);
        }
        //效果光照
        private void toolStripMenuItem_sun_Click(object sender, EventArgs e)
        {
            toolStripMenuItem_sun.Checked = !toolStripMenuItem_sun.Checked;

            OgreView.Singleton.sunNode.SetVisible(toolStripMenuItem_sun.Checked);
            if (toolStripMenuItem_sun.Checked)
            {
                OgreView.Singleton.sceneMgr.AmbientLight = new ColourValue(0f, 0f, 0f);
            }
            else
            {
                OgreView.Singleton.sceneMgr.AmbientLight = new ColourValue(1f, 1f, 1f);
            }
        }
        //效果人
        private void toolStripMenuItem_People_Click(object sender, EventArgs e)
        {
            toolStripMenuItem_People.Checked = !toolStripMenuItem_People.Checked;
            MogreWin.ShowMan(toolStripMenuItem_People.Checked);
        }
        //全景
        private void toolStripMenuItem_AllView_Click(object sender, EventArgs e)
        {
            MogreWin.RestCamer();
        }
        //信息查询
        private void toolStripMenuItem_QueryInfo_Click(object sender, EventArgs e)
        {
            ToolManageObject.ToolType = typeof(SelectModelTool);
        }
        //关于
        private void toolStripMenuItem_help_Click(object sender, EventArgs e)
        {
            AboutBox1 dlg = new AboutBox1();
            dlg.ShowDialog();
        }
        //进入室内场景
        private void toolStripButton1_Click(object sender, EventArgs e)
        {                       
            Process myProcess = new Process();
            string myDocumentsPath = Application.StartupPath;

            myProcess.StartInfo.FileName = myDocumentsPath + "\\myogrebsp.exe";
            //myProcess.StartInfo.Verb = "Print";
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.EnableRaisingEvents = true;
            myProcess.Exited += new EventHandler(myProcess_Exited);
            myProcess.Start();
            this.Visible = false;
        }
        public void inbspscene(string bspname)
        {
            switch (bspname)
            {
                case "场景1":
                    bspname = Application.StartupPath + "\\bsp1.cfg";
                    break;
                case "场景2":
                    bspname = Application.StartupPath + "\\bsp2.cfg";
                    break;
                case "场景3":
                    bspname = Application.StartupPath + "\\bsp3.cfg";
                    break;
                case "场景4":
                    bspname = Application.StartupPath + "\\bsp4.cfg";
                    break;
                default:
                    bspname = Application.StartupPath + "\\bsp1.cfg";
                    break;

            }

            Process myProcess = new Process();
            string myDocumentsPath = Application.StartupPath;

            myProcess.StartInfo.FileName = myDocumentsPath + "\\myogrebsp.exe ";
            //myProcess.StartInfo.Verb = "Print";
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.StartInfo.Arguments = bspname;
            myProcess.EnableRaisingEvents = true;
            myProcess.Exited += new EventHandler(myProcess_Exited);
            myProcess.Start();
            this.Visible = false;
        }

        void myProcess_Exited(object sender, EventArgs e)
        {
            Hideform();
        }
        delegate void HideformCallback();

        void Hideform()
        {
            if (this.InvokeRequired)
            {
                HideformCallback hide=new HideformCallback(Hideform);
                this.Invoke(hide);
            }
            else
            {
                this.Visible = true;
            } 
        }
       
       

    }
}
