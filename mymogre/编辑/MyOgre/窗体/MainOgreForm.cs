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
        /// 当前工具栏模型操作
        /// 0:没有任何操作
        /// 1：放大
        /// 2：缩小
        /// 3：X左转
        /// 4：X右转
        /// 5：Y左转
        /// 6：Y右转
        /// 7：Z左转
        /// 8：Z右转
        /// 9：X加
        /// 10：X减
        /// 11：Y加
        /// 12：Y减
        /// 13：Z加
        /// 14：Z减
        /// </summary>
        private int modeloptionflag = 0;
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
        public int RoadWidth=10;
        public int LineWidth=1;
        public Color RoadColor= Color.FromArgb(100,100,100);
        public Color LineColor=Color.Yellow;
        /// <summary>
        /// 用来记录路名
        /// </summary>
        public List<string> strnamelist = new List<string>();

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
            this.KeyDown += new KeyEventHandler(MainOgreForm_KeyDown);            

            this.MouseWheel += new MouseEventHandler(MainForm_MouseWheel);
            listView1.MouseWheel += new MouseEventHandler(MainForm_MouseWheel);
            treeView1.MouseWheel += new MouseEventHandler(MainForm_MouseWheel);
            //新建场景功能
            MenuTool.Add(new NewScene(new_item));
            //打开场景文件
            MenuTool.Add(new OpenScene(open_item));
            //保存场景文件
            MenuTool.Add(new SaveScene(save_item));
            //别存为场景文件
            MenuTool.Add(new SaveAsScene(saveas_item));
            //模型导入
            MenuTool.Add(new ImportModelTool(importmodel_item));
            //背景图管理
            MenuTool.Add(new BackgroupManageClass(BackgroudToolStripMenuItem));
            //删除模型
            MenuTool.Add(new DeleteModelClass(deletemodel_ToolStripMenuItem1));

            //向工具管理器增加工具
            //漫游工具
            ToolManageObject.AddTool(new PanToolClass());
            //选择模型工具
            ToolManageObject.AddTool(new SelectModelTool());
            //增加模型工具
            ToolManageObject.AddTool(new AddModelTool());
            //增加注记工具
            ToolManageObject.AddTool(new AddBillBoardTool());
            //选择广告牌
            ToolManageObject.AddTool(new SelectBillBoardTool());
            //增加水
            ToolManageObject.AddTool(new AddWaterTool());
            //增加草
            ToolManageObject.AddTool(new AddGrassTool());
            //增加路
            ToolManageObject.AddTool(new AddRoadTool());
            //设置当前工具为漫游
            ToolManageObject.ToolType = typeof(PanToolClass);
  

            //初始化ogre环境
            OgreView.Singleton.InitMogre();
            //创建渲染窗口
            MogreWin.CreateWindows((uint)panel1.Width, (uint)panel1.Height);
            //采用定时器渲染场景
            timer1.Enabled = true;
            //载入模型分组数据
            OpenModelGroup();
            //将模型库中的模型加开到模型列表
            LoadAllModel();

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
      
        void MainOgreForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (OgreView.Singleton.CurrentWgOperageNode != null)
            {
                MovableObject wr = OgreView.Singleton.CurrentWgOperageNode.GetAttachedObject(0) ;
                ModelDataManage.RemoveWGrass(wr.Name);
                OgreView.Singleton.WRnode.RemoveChild(OgreView.Singleton.CurrentWgOperageNode);
                OgreView.Singleton.CurrentWgOperageNode = null;
                return;
            }
            
            if (OgreView.Singleton.CurrentOperateNode == null)
                return;

            if (e.KeyValue == 46)
            {
                ModelDataManage.AddRemoveState(OgreView.Singleton.CurrentOperateNode, 2);//删除模型节点
                OgreView.Singleton.mainNode.RemoveChild(OgreView.Singleton.CurrentOperateNode);//删除在场景中的节点

                ModelDataManage.RemoveBill(OgreView.Singleton.CurrentOperateNode);//删除模型节点
                OgreView.Singleton.billNode.RemoveChild(OgreView.Singleton.CurrentOperateNode);//删除在场景中的节点
                OgreView.Singleton.CurrentOperateNode = null;
                return;
            }

            //得到结点所对应的相应信息。以便结点的旋转
            ModelEntryStruct modelentry = null;
            Entity en = OgreView.Singleton.CurrentOperateNode.GetAttachedObject(0) as Entity;
            if (en == null)
                return;
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

            Quaternion q;
            switch (e.KeyValue)
            {
                case 49://放大1
                    OgreView.Singleton.CurrentOperateNode.Scale(1.1f, 1.1f, 1.1f);
                    break;
                case 50://缩小2
                    OgreView.Singleton.CurrentOperateNode.Scale(0.9f, 0.9f, 0.9f);
                    break;
                case 51://X左3
                    {
                        float yy = (float)(System.Math.PI / 90);
                        Quaternion x = new Quaternion(new Radian(modelentry.旋转角度.x + yy), Vector3.UNIT_X);
                        Quaternion y = new Quaternion(new Radian(modelentry.旋转角度.y), Vector3.UNIT_Y);
                        Quaternion z = new Quaternion(new Radian(modelentry.旋转角度.z), Vector3.UNIT_Z);
                        OgreView.Singleton.CurrentOperateNode.Orientation = new Quaternion(1, 0, 0, 0);
                        OgreView.Singleton.CurrentOperateNode.Orientation *= x * y * z;
                        ModelDataManage.UpdateModelState(OgreView.Singleton.CurrentOperateNode, 'x', modelentry.旋转角度.x + yy);
                    }
                    break;
                case 52://X右4
                    {
                        float yy = -(float)(System.Math.PI / 90);
                        Quaternion x = new Quaternion(new Radian(modelentry.旋转角度.x + yy), Vector3.UNIT_X);
                        Quaternion y = new Quaternion(new Radian(modelentry.旋转角度.y), Vector3.UNIT_Y);
                        Quaternion z = new Quaternion(new Radian(modelentry.旋转角度.z), Vector3.UNIT_Z);
                        OgreView.Singleton.CurrentOperateNode.Orientation = new Quaternion(1, 0, 0, 0);
                        OgreView.Singleton.CurrentOperateNode.Orientation *= x * y * z;
                        ModelDataManage.UpdateModelState(OgreView.Singleton.CurrentOperateNode, 'x', modelentry.旋转角度.x + yy);
                    }
                    break;
                case 53://Y左5
                    {
                        float yy = (float)(System.Math.PI / 90);
                        Quaternion x = new Quaternion(new Radian(modelentry.旋转角度.x ), Vector3.UNIT_X);
                        Quaternion y = new Quaternion(new Radian(modelentry.旋转角度.y + yy), Vector3.UNIT_Y);
                        Quaternion z = new Quaternion(new Radian(modelentry.旋转角度.z), Vector3.UNIT_Z);
                        OgreView.Singleton.CurrentOperateNode.Orientation = new Quaternion(1, 0, 0, 0);
                        OgreView.Singleton.CurrentOperateNode.Orientation *= x * y * z;
                        ModelDataManage.UpdateModelState(OgreView.Singleton.CurrentOperateNode, 'y', modelentry.旋转角度.y + yy);
                    }
                    break;
                case 54://Y右6
                    {
                        float yy = -(float)(System.Math.PI / 90);
                        Quaternion x = new Quaternion(new Radian(modelentry.旋转角度.x), Vector3.UNIT_X);
                        Quaternion y = new Quaternion(new Radian(modelentry.旋转角度.y + yy), Vector3.UNIT_Y);
                        Quaternion z = new Quaternion(new Radian(modelentry.旋转角度.z), Vector3.UNIT_Z);
                        OgreView.Singleton.CurrentOperateNode.Orientation = new Quaternion(1, 0, 0, 0);
                        OgreView.Singleton.CurrentOperateNode.Orientation *= x * y * z;
                        ModelDataManage.UpdateModelState(OgreView.Singleton.CurrentOperateNode, 'y', modelentry.旋转角度.y + yy);
                    }
                    break;
                case 55://Z左7
                    {
                        float yy = (float)(System.Math.PI / 90);
                        Quaternion x = new Quaternion(new Radian(modelentry.旋转角度.x), Vector3.UNIT_X);
                        Quaternion y = new Quaternion(new Radian(modelentry.旋转角度.y), Vector3.UNIT_Y);
                        Quaternion z = new Quaternion(new Radian(modelentry.旋转角度.z+yy), Vector3.UNIT_Z);
                        OgreView.Singleton.CurrentOperateNode.Orientation = new Quaternion(1, 0, 0, 0);
                        OgreView.Singleton.CurrentOperateNode.Orientation *= x * y * z;
                        ModelDataManage.UpdateModelState(OgreView.Singleton.CurrentOperateNode, 'z', modelentry.旋转角度.z + yy);
                    }
                    break;
                case 56://Z右8
                    {
                        float yy = -(float)(System.Math.PI / 90);
                        Quaternion x = new Quaternion(new Radian(modelentry.旋转角度.x), Vector3.UNIT_X);
                        Quaternion y = new Quaternion(new Radian(modelentry.旋转角度.y), Vector3.UNIT_Y);
                        Quaternion z = new Quaternion(new Radian(modelentry.旋转角度.z + yy), Vector3.UNIT_Z);
                        OgreView.Singleton.CurrentOperateNode.Orientation = new Quaternion(1, 0, 0, 0);
                        OgreView.Singleton.CurrentOperateNode.Orientation *= x * y * z;
                        ModelDataManage.UpdateModelState(OgreView.Singleton.CurrentOperateNode, 'z', modelentry.旋转角度.z + yy);
                    }
                    break;
                default:
                    break;
            }
            
        }

        
        //定时器
        private void timer1_Tick(object sender, EventArgs e)
        {
            ToolManageObject.TimerTick(sender, e);

            if (ToolManageObject.ToolType != typeof(AddModelTool))
            {
                if (OgreView.Singleton.addmodelNode == null)
                    return;
                OgreView.Singleton.addmodelNode.Position = new Vector3(1000000, 100000, 100000);
            }
            //工具栏场景操作
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
            if (modeloptionflag > 0)
            {
                //得到结点所对应的相应信息。以便结点的旋转
                ModelEntryStruct modelentry = null;
                Entity en = OgreView.Singleton.CurrentOperateNode.GetAttachedObject(0) as Entity;
                if (en == null)
                    return;
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

                Quaternion q;

                switch (modeloptionflag)
                {
                    case 1://放大
                        OgreView.Singleton.CurrentOperateNode.Scale(1.1f, 1.1f, 1.1f);
                        break;
                    case 2://缩小
                        OgreView.Singleton.CurrentOperateNode.Scale(0.9f, 0.9f, 0.9f);
                        break;
                    case 3://x左
                        {
                            float yy = (float)(System.Math.PI / 90);
                            Quaternion x = new Quaternion(new Radian(modelentry.旋转角度.x + yy), Vector3.UNIT_X);
                            Quaternion y = new Quaternion(new Radian(modelentry.旋转角度.y), Vector3.UNIT_Y);
                            Quaternion z = new Quaternion(new Radian(modelentry.旋转角度.z), Vector3.UNIT_Z);
                            OgreView.Singleton.CurrentOperateNode.Orientation = new Quaternion(1, 0, 0, 0);
                            OgreView.Singleton.CurrentOperateNode.Orientation *= x * y * z;
                            ModelDataManage.UpdateModelState(OgreView.Singleton.CurrentOperateNode, 'x', modelentry.旋转角度.x + yy);
                        }
                        break;
                    case 4://x右
                        {
                            float yy = -(float)(System.Math.PI / 90);
                            Quaternion x = new Quaternion(new Radian(modelentry.旋转角度.x + yy), Vector3.UNIT_X);
                            Quaternion y = new Quaternion(new Radian(modelentry.旋转角度.y), Vector3.UNIT_Y);
                            Quaternion z = new Quaternion(new Radian(modelentry.旋转角度.z), Vector3.UNIT_Z);
                            OgreView.Singleton.CurrentOperateNode.Orientation = new Quaternion(1, 0, 0, 0);
                            OgreView.Singleton.CurrentOperateNode.Orientation *= x * y * z;
                            ModelDataManage.UpdateModelState(OgreView.Singleton.CurrentOperateNode, 'x', modelentry.旋转角度.x + yy);
                        }
                        break;
                    case 5://y左
                        {
                            float yy = (float)(System.Math.PI / 90);
                            Quaternion x = new Quaternion(new Radian(modelentry.旋转角度.x), Vector3.UNIT_X);
                            Quaternion y = new Quaternion(new Radian(modelentry.旋转角度.y + yy), Vector3.UNIT_Y);
                            Quaternion z = new Quaternion(new Radian(modelentry.旋转角度.z), Vector3.UNIT_Z);
                            OgreView.Singleton.CurrentOperateNode.Orientation = new Quaternion(1, 0, 0, 0);
                            OgreView.Singleton.CurrentOperateNode.Orientation *= x * y * z;
                            ModelDataManage.UpdateModelState(OgreView.Singleton.CurrentOperateNode, 'y', modelentry.旋转角度.y + yy);
                        }
                        break;
                    case 6://y右
                        {
                            float yy = -(float)(System.Math.PI / 90);
                            Quaternion x = new Quaternion(new Radian(modelentry.旋转角度.x), Vector3.UNIT_X);
                            Quaternion y = new Quaternion(new Radian(modelentry.旋转角度.y + yy), Vector3.UNIT_Y);
                            Quaternion z = new Quaternion(new Radian(modelentry.旋转角度.z), Vector3.UNIT_Z);
                            OgreView.Singleton.CurrentOperateNode.Orientation = new Quaternion(1, 0, 0, 0);
                            OgreView.Singleton.CurrentOperateNode.Orientation *= x * y * z;
                            ModelDataManage.UpdateModelState(OgreView.Singleton.CurrentOperateNode, 'y', modelentry.旋转角度.y + yy);
                        }
                        break;
                    case 7://z左
                        {
                            float yy = (float)(System.Math.PI / 90);
                            Quaternion x = new Quaternion(new Radian(modelentry.旋转角度.x), Vector3.UNIT_X);
                            Quaternion y = new Quaternion(new Radian(modelentry.旋转角度.y), Vector3.UNIT_Y);
                            Quaternion z = new Quaternion(new Radian(modelentry.旋转角度.z + yy), Vector3.UNIT_Z);
                            OgreView.Singleton.CurrentOperateNode.Orientation = new Quaternion(1, 0, 0, 0);
                            OgreView.Singleton.CurrentOperateNode.Orientation *= x * y * z;
                            ModelDataManage.UpdateModelState(OgreView.Singleton.CurrentOperateNode, 'z', modelentry.旋转角度.z + yy);
                        }
                        break;
                    case 8://z右
                        {
                            float yy = -(float)(System.Math.PI / 90);
                            Quaternion x = new Quaternion(new Radian(modelentry.旋转角度.x), Vector3.UNIT_X);
                            Quaternion y = new Quaternion(new Radian(modelentry.旋转角度.y), Vector3.UNIT_Y);
                            Quaternion z = new Quaternion(new Radian(modelentry.旋转角度.z + yy), Vector3.UNIT_Z);
                            OgreView.Singleton.CurrentOperateNode.Orientation = new Quaternion(1, 0, 0, 0);
                            OgreView.Singleton.CurrentOperateNode.Orientation *= x * y * z;
                            ModelDataManage.UpdateModelState(OgreView.Singleton.CurrentOperateNode, 'z', modelentry.旋转角度.z + yy);
                        }
                        break;
                    case 9://x+
                        {
                            Vector3 qq = OgreView.Singleton.CurrentOperateNode.Position;
                            qq.x = qq.x + 1;
                            OgreView.Singleton.CurrentOperateNode.Position = qq;
                        }
                        break;
                    case 10://x-
                        {
                            Vector3 qq = OgreView.Singleton.CurrentOperateNode.Position;
                            qq.x = qq.x - 1;
                            OgreView.Singleton.CurrentOperateNode.Position = qq;
                        }
                        break;
                    case 11://y+
                        {
                            Vector3 qq = OgreView.Singleton.CurrentOperateNode.Position;
                            qq.y = qq.y + 1;
                            OgreView.Singleton.CurrentOperateNode.Position = qq;
                        }
                        break;
                    case 12://y-
                        {
                            Vector3 qq = OgreView.Singleton.CurrentOperateNode.Position;
                            qq.y = qq.y - 1;
                            OgreView.Singleton.CurrentOperateNode.Position = qq;
                        }
                        break;
                    case 13://z+
                        {
                            Vector3 qq = OgreView.Singleton.CurrentOperateNode.Position;
                            qq.z = qq.z + 1;
                            OgreView.Singleton.CurrentOperateNode.Position = qq;
                        }
                        break;
                    case 14://z-
                        {
                            Vector3 qq = OgreView.Singleton.CurrentOperateNode.Position;
                            qq.z = qq.z - 1;
                            OgreView.Singleton.CurrentOperateNode.Position = qq;
                        }
                        break;
                    case 15:
                        {
                            OgreView.Singleton.CurrentOperateNode.Scale(1.1f, 1f, 1f);
                        }
                        break;
                    case 16:
                        {
                            OgreView.Singleton.CurrentOperateNode.Scale(0.9f, 1f, 1f);
                        }
                        break;
                    case 17:
                        {
                            OgreView.Singleton.CurrentOperateNode.Scale(1f, 1.1f, 1f);
                        }
                        break;
                    case 18:
                        {
                            OgreView.Singleton.CurrentOperateNode.Scale(1f, 0.9f, 1f);
                        }
                        break;
                    case 19:
                        {OgreView.Singleton.CurrentOperateNode.Scale(1f, 1f, 1.1f);
                        }
                        break;
                    case 20:
                        {
                            OgreView.Singleton.CurrentOperateNode.Scale(1f, 1f, 0.9f);
                        }
                        break;
                    default:
                        break;
                }
            }
            if (sun_ToolStripMenuItem.Checked)
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
            panel1.Cursor = Cursors.Arrow;
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


            if (textbox == null)
            {
                ToolStripTextBox tb = sender as ToolStripTextBox;
                textbox = tb.TextBox;
                textbox.Name = tb.Name;
                if (textbox.Text=="")
                return;
            }
            
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
            if (textbox.Name.Equals("scalex_textBox") || textbox.Name.Equals("scalex_toolStripTextBox"))
            {
                Vector3 v = OgreView.Singleton.CurrentOperateNode.GetScale();
                v.x = float.Parse(textbox.Text);
                OgreView.Singleton.CurrentOperateNode.SetScale(v);
            }
            else if (textbox.Name.Equals("scaley_textBox") || textbox.Name.Equals("scaley_toolStripTextBox"))
            {
                Vector3 v = OgreView.Singleton.CurrentOperateNode.GetScale();
                v.y = float.Parse(textbox.Text);
                OgreView.Singleton.CurrentOperateNode.SetScale(v);
            }
            else if (textbox.Name.Equals("scalez_textBox") || textbox.Name.Equals("scalez_toolStripTextBox"))
            {
                Vector3 v = OgreView.Singleton.CurrentOperateNode.GetScale();
                v.z = float.Parse(textbox.Text);
                OgreView.Singleton.CurrentOperateNode.SetScale(v);
            }
            else if (textbox.Name.Equals("roatx_textBox") || textbox.Name.Equals("roatx_toolStripTextBox"))//模型的旋转
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
            else if (textbox.Name.Equals("roaty_textBox") || textbox.Name.Equals("roaty_toolStripTextBox"))
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
            else if (textbox.Name.Equals("roatz_textBox") || textbox.Name.Equals("roatz_toolStripTextBox"))
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
            else if (textbox.Name.Equals("positionx_textBox") || textbox.Name.Equals("positionx_toolStripTextBox"))//更新位置
            {
                Vector3 q = OgreView.Singleton.CurrentOperateNode.Position;
                q.x = float.Parse(textbox.Text);
                OgreView.Singleton.CurrentOperateNode.Position = q;
            }
            else if (textbox.Name.Equals("positiony_textBox") || textbox.Name.Equals("positiony_toolStripTextBox"))
            {
                Vector3 q = OgreView.Singleton.CurrentOperateNode.Position;
                q.y = float.Parse(textbox.Text);
                OgreView.Singleton.CurrentOperateNode.Position = q;
            }
            else if (textbox.Name.Equals("positionz_textBox") || textbox.Name.Equals("positionz_toolStripTextBox"))
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
            
            //更新编辑框的可用状态
            scalex_textBox.Enabled = flag;
            scaley_textBox.Enabled = flag;
            scalez_textBox.Enabled = flag;

            roatx_textBox.Enabled = flag;
            roaty_textBox.Enabled = flag;
            roatz_textBox.Enabled = flag;

            positionx_textBox.Enabled = flag;
            positiony_textBox.Enabled = flag;
            positionz_textBox.Enabled = flag;

            modelname_textBox.Enabled = flag;

            if (node == null)
                return;
            //-----------------------
            //如果编辑框可用。则将模型节点的相应信息更新到编辑框
            if (flag)
            {
                //回为控制编辑的上下按钮控制，只能是整型，所以这里将控件的值放大一定的倍数，在用时在还原。，以下同理
                scalex_textBox.Text = node.GetScale().x.ToString();
                scalex_toolStripTextBox.Text = node.GetScale().x.ToString();
                scalex_vScrollBar.Value = (int)(node.GetScale().x / -0.1f);

                scaley_textBox.Text = node.GetScale().y.ToString();
                scaley_toolStripTextBox.Text = node.GetScale().y.ToString();
                scaley_vScrollBar.Value = (int)(node.GetScale().y / -0.1f);

                scalez_textBox.Text = node.GetScale().z.ToString();
                scalez_toolStripTextBox.Text = node.GetScale().z.ToString();
                scalez_vScrollBar.Value = (int)(node.GetScale().z / -0.1f);

                ModelEntryStruct modelentry = null;
                Entity en = node.GetAttachedObject(0) as Entity;
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
                //旋转
                roatx_textBox.Text = (modelentry.旋转角度.x * 180 / System.Math.PI).ToString();
                roatx_toolStripTextBox.Text = (modelentry.旋转角度.x * 180 / System.Math.PI).ToString();
                roatx_vScrollBar.Value = (int)(modelentry.旋转角度.x * 180 / System.Math.PI * -1);

                roaty_textBox.Text = (modelentry.旋转角度.y * 180 / System.Math.PI).ToString();
                roaty_toolStripTextBox.Text = (modelentry.旋转角度.y * 180 / System.Math.PI).ToString();
                roaty_vScrollBar.Value = (int)(modelentry.旋转角度.y * 180 / System.Math.PI * -1);

                roatz_textBox.Text = (modelentry.旋转角度.z * 180 / System.Math.PI).ToString();
                roatz_toolStripTextBox.Text = (modelentry.旋转角度.z * 180 / System.Math.PI).ToString();
                roatz_vScrollBar.Value = (int)(modelentry.旋转角度.z * 180 / System.Math.PI * -1);
                //位置
                positionx_textBox.Text = node.Position.x.ToString();
                positionx_toolStripTextBox.Text = node.Position.x.ToString();
                positionx_vScrollBar.Value = (int)(node.Position.x * -1);

                positiony_textBox.Text = node.Position.y.ToString();
                positiony_toolStripTextBox.Text = node.Position.y.ToString();
                positiony_vScrollBar.Value = (int)(node.Position.y * -1);

                positionz_textBox.Text = node.Position.z.ToString();
                positionz_toolStripTextBox.Text = node.Position.z.ToString();
                positionz_vScrollBar.Value = (int)(node.Position.z * -1);

                modelname_textBox.Text = modelentry.名称;
            }
            else
            {
                //清空编辑框
                scalex_textBox.Text = "";
                scaley_textBox.Text = "";
                scalez_textBox.Text = "";

                scalex_toolStripTextBox.Text = "";
                scaley_toolStripTextBox.Text = "";
                scalez_toolStripTextBox.Text = "";

                roatx_textBox.Text = "";
                roaty_textBox.Text = "";
                roatz_textBox.Text = "";

                roatx_toolStripTextBox.Text = "";
                roaty_toolStripTextBox.Text = "";
                roatz_toolStripTextBox.Text = "";

                positionx_textBox.Text = "";
                positiony_textBox.Text = "";
                positionz_textBox.Text = "";

                positionx_toolStripTextBox.Text = "";
                positiony_toolStripTextBox.Text = "";
                positionz_toolStripTextBox.Text = "";

                modelname_textBox.Text = "";
            }

        }
        //当编辑框的上下按钮值改变时，响应的事件
        private void scalex_vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            VScrollBar scroll = sender as VScrollBar;
            if (scroll == null)
                return;
            //根据传入控件的名称来确定模型的那个值需要更改。
            if (scroll.Name.Equals("scalex_vScrollBar"))
            {
                scalex_textBox.Text = ((float)e.NewValue * -0.1f).ToString();
            }
            else if (scroll.Name.Equals("scaley_vScrollBar"))
            {
                scaley_textBox.Text = ((float)e.NewValue * -0.1f).ToString();
            }
            else if (scroll.Name.Equals("scalez_vScrollBar"))
            {
                scalez_textBox.Text = ((float)e.NewValue * -0.1f).ToString();
            }
            else if (scroll.Name.Equals("roatx_vScrollBar"))
            {
                roatx_textBox.Text = (e.NewValue * -1).ToString();
            }
            else if (scroll.Name.Equals("roaty_vScrollBar"))
            {
                roaty_textBox.Text = (e.NewValue * -1).ToString();
            }
            else if (scroll.Name.Equals("roatz_vScrollBar"))
            {
                roatz_textBox.Text = (e.NewValue * -1).ToString();
            }
            else if (scroll.Name.Equals("positionx_vScrollBar"))
            {
                positionx_textBox.Text = ((float)e.NewValue * -1f).ToString();
            }
            else if (scroll.Name.Equals("positiony_vScrollBar"))
            {
                positiony_textBox.Text = ((float)e.NewValue * -1f).ToString();
            }
            else if (scroll.Name.Equals("positionz_vScrollBar"))
            {
                positionz_textBox.Text = ((float)e.NewValue * -1f).ToString();
            }
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            if (IsStarEdit)
            {
                //设置当前工具为漫游
                ToolManageObject.ToolType = typeof(AddModelTool);
            }
            else
            {
                MessageBox.Show("请新建一个场景");
            }
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
            snowToolStripMenuItem.Checked = !snowToolStripMenuItem.Checked;           
            MogreWin.ShowSnow(snowToolStripMenuItem.Checked);            
        }
        /// <summary>
        /// 雨效果开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rainToolStripMenuItem.Checked = !rainToolStripMenuItem.Checked;
            MogreWin.ShowRain(rainToolStripMenuItem.Checked);
        }
        /// <summary>
        /// 光照效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sun_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sun_ToolStripMenuItem.Checked = !sun_ToolStripMenuItem.Checked;

            OgreView.Singleton.sunNode.SetVisible(sun_ToolStripMenuItem.Checked);
            if (sun_ToolStripMenuItem.Checked)
            {
                OgreView.Singleton.sceneMgr.AmbientLight = new ColourValue(0f, 0f, 0f);
            }
            else
            {
                OgreView.Singleton.sceneMgr.AmbientLight = new ColourValue(1f, 1f, 1f);
            }
        }
        private void 选择注记ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //设置当前工具为漫游
            ToolManageObject.ToolType = typeof(SelectBillBoardTool);
        }

        private void 删除注记ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OgreView.Singleton.CurrentOperateNode == null)
                return;
            ModelDataManage.RemoveBill(OgreView.Singleton.CurrentOperateNode);//删除模型节点
            OgreView.Singleton.billNode.RemoveChild(OgreView.Singleton.CurrentOperateNode);//删除在场景中的节点
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
        //修改模型信息
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (MogreWin.CurrentOperateNode != null)
            {
                ModelEntryStruct mm = ModelDataManage.modelEntry.GetModelEntry(MogreWin.CurrentOperateNode);
                if (mm != null)
                {
                    AddModelForm dlg = new AddModelForm();
                    dlg.name_textBox.Text = mm.名称;
                    dlg.remark_textBox.Text = mm.备注属性;
                    dlg.picture_textBox.Text = mm.图片名称;
                    dlg.vido_textBox.Text = mm.视频名称;

                    if (dlg.ShowDialog() == DialogResult.OK)
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
        #region 模型变换
        private void zoomin_toolStripButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (OgreView.Singleton.CurrentOperateNode == null)
                return;
            ToolStripButton item = sender as ToolStripButton;
            switch (item.Name)
            {
                case "zoomin_toolStripButton":
                    modeloptionflag = 1;
                    break;
                case "zoomout_toolStripButton":
                    modeloptionflag = 2;
                    break;
                case "xleft_toolStripButton":
                    modeloptionflag = 3;
                    break;
                case "xright_toolStripButton":
                    modeloptionflag = 4;
                    break;
                case "yleft_toolStripButton":
                    modeloptionflag = 5;
                    break;
                case "yright_toolStripButton":
                    modeloptionflag = 6;
                    break;
                case "zleft_toolStripButton":
                    modeloptionflag = 7;
                    break;
                case "zright_toolStripButton":
                    modeloptionflag = 8;
                    break;
                case "xadd_toolStripButton":
                    modeloptionflag = 9;
                    break;
                case "xsub_toolStripButton":
                    modeloptionflag = 10;
                    break;
                case "yadd_toolStripButton":
                    modeloptionflag = 11;
                    break;
                case "ysub_toolStripButton":
                    modeloptionflag = 12;
                    break;
                case "zadd_toolStripButton":
                    modeloptionflag = 13;
                    break;
                case "zsub_toolStripButton":
                    modeloptionflag = 14;
                    break;
                case "xmax_toolStripButton":
                    modeloptionflag = 15;
                    break;
                case "xmin_toolStripButton":
                    modeloptionflag = 16;
                    break;
                case "ymax_toolStripButton":
                    modeloptionflag = 17;
                    break;
                case "ymin_toolStripButton":
                    modeloptionflag = 18;
                    break;
                case "zmax_toolStripButton":
                    modeloptionflag = 19;
                    break;
                case "zmin_toolStripButton":
                    modeloptionflag = 20;
                    break;
                default:
                    break;
            }
        }
        private void zoomin_toolStripButton_MouseLeave(object sender, EventArgs e)
        {
            modeloptionflag = 0;
        }
        private void zoomin_toolStripButton_MouseUp(object sender, MouseEventArgs e)
        {
            modeloptionflag = 0;
        }
        #endregion

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            panel1.Cursor = Cursors.Cross;
            //设置当前工具为
            ToolManageObject.ToolType = typeof(AddWaterTool);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            panel1.Cursor = Cursors.Cross;
            //设置当前工具为
            ToolManageObject.ToolType = typeof(AddGrassTool);
        }
        //绘制路
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            panel1.Cursor = Cursors.Cross;
            //设置当前工具为
            ToolManageObject.ToolType = typeof(AddRoadTool);
        }
        //设置路样式
        private void 设置路样式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetRoadStyleForm dlg = new SetRoadStyleForm();
            dlg.road_textBox.Text = this.RoadWidth.ToString();
            dlg.road_button.BackColor = this.RoadColor;
            dlg.line_textBox.Text = this.LineWidth.ToString();
            dlg.line_button.BackColor = this.LineColor;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.RoadWidth = int.Parse(dlg.road_textBox.Text);
                this.RoadColor = dlg.road_button.BackColor;
                this.LineColor = dlg.line_button.BackColor;
                this.LineWidth = int.Parse(dlg.line_textBox.Text);
            }
        }
        
        //绘制湖
        private void toolStripMenuItem_DrawLake_Click(object sender, EventArgs e)
        {
            ToolManageObject.ToolType = typeof(AddWaterTool);
        }
        //绘制草
        private void toolStripMenuItem_DrawGrass_Click(object sender, EventArgs e)
        {
            ToolManageObject.ToolType = typeof(AddGrassTool);
        }
        //关于
        private void toolStripMenuItem_help_Click(object sender, EventArgs e)
        {
            AboutBox1 dlg = new AboutBox1();
            dlg.ShowDialog();
        }

        private void select_item_Click_1(object sender, EventArgs e)
        {
            ToolManageObject.ToolType = typeof(SelectModelTool);
        }

        private void 删除模型ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

            if (OgreView.Singleton.CurrentWgOperageNode != null)
            {
                MovableObject wr = OgreView.Singleton.CurrentWgOperageNode.GetAttachedObject(0);
                ModelDataManage.RemoveWGrass(wr.Name);
                OgreView.Singleton.WRnode.RemoveChild(OgreView.Singleton.CurrentWgOperageNode);
                OgreView.Singleton.CurrentWgOperageNode = null;
                return;
            }
            if (OgreView.Singleton.CurrentOperateNode == null)
                return;

            ModelDataManage.AddRemoveState(OgreView.Singleton.CurrentOperateNode, 2);//删除模型节点
            OgreView.Singleton.mainNode.RemoveChild(OgreView.Singleton.CurrentOperateNode);//删除在场景中的节点

            ModelDataManage.RemoveBill(OgreView.Singleton.CurrentOperateNode);//删除模型节点
            OgreView.Singleton.billNode.RemoveChild(OgreView.Singleton.CurrentOperateNode);//删除在场景中的节点
            OgreView.Singleton.CurrentOperateNode = null;
        }
        //程序关闭
        private void MainOgreForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (MainOgreForm.Singleton.materialptr != null)
            {
                MainOgreForm.Singleton.materialptr.Dispose();
                MainOgreForm.Singleton.materialptr = null;
            }
            SaveModelGroup();
        }

        /// <summary>
        /// 载入模型到面板上的
        /// </summary>
        public void LoadAllModel()
        {
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
            }          
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
                if (string.IsNullOrEmpty(entry.名称))
                    node.Text = entry.实体名;
                else
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
            foreach (RoadStruct road in ModelDataManage.modelEntry.路)
            {
                string texturefilename = Application.StartupPath + "\\TempBillBoard\\" + road.Name + ".png";

                if (File.Exists(texturefilename))
                {
                    Pass p = MainOgreForm.Singleton.materialptr.GetTechnique(0).GetPass(0);
                    int i = 0;

                    foreach (TextureUnitState tus in p.GetTextureUnitStateIterator())
                    {
                        if (tus.TextureName == texturefilename)
                        {
                            p.RemoveTextureUnitState((ushort)i);
                            break;
                        }
                        i++;
                    }

                    TextureManager.Singleton.Remove(texturefilename);
                    File.Delete(texturefilename);
                }

            }

            ModelDataManage.modelEntry.Clear();
           
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
            MainOgreForm.Singleton.ClearTree();

            if (MainOgreForm.Singleton.materialptr != null)
            {
                MainOgreForm.Singleton.materialptr.Dispose();
                MainOgreForm.Singleton.materialptr = null;
            }
        }
        public void AddNodeTree(ModelEntryStruct entry)
        {
            TreeNode node = new TreeNode();
            if (string.IsNullOrEmpty(entry.名称))
                node.Text = entry.实体名;
            else
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
       
        //保持模型分组信息
        private void SaveModelGroup()
        {
            //序列化保存文件
            XmlSerializer serializer = new XmlSerializer(typeof(ModelManage));
            FileStream stream = new FileStream(Application.StartupPath+"\\ModelGroup.XML", FileMode.Create);
            StreamWriter writer = new StreamWriter(stream, Encoding.GetEncoding(936));
            serializer.Serialize(writer,ModelGroup);
            writer.Close();
            stream.Close();
        }
        //打开模型库分组文件
        private void OpenModelGroup()
        {
            if (File.Exists(Application.StartupPath + "\\ModelGroup.XML"))
            {
                //打开场景文件， 通过XML的返序列化读取文件，
                XmlSerializer serializer = new XmlSerializer(typeof(ModelManage));
                FileStream file = File.OpenRead(Application.StartupPath + "\\ModelGroup.XML");
                ModelGroup = (ModelManage)serializer.Deserialize(file);
                file.Close();
            }
            else
            {
                ModelGroup = new ModelManage();
            }
        }

        private void 删除路ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RoadForm dlg = new RoadForm();
            dlg.ShowDialog();
        }

        
    }
}
