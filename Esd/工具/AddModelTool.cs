using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EsdCommon;
using System.Windows;
using Mogre;
using OgreLib;
using System.Windows.Input;
using System.IO;
using EsdCommon.Tool;

namespace Esd.Tool
{
    //增加模型工具
    public class AddModelTool : AbstractTool
    {
        private Entity CurrentEntity = null;
        public Point MouseDownPt;
        public Point TempMouseMovePt;
        /// <summary>
        /// 增加的模型名
        /// </summary>
        private ModelStruct ms = null;
        /// <summary>
        /// 当前鼠标是否按下
        /// </summary>
        bool MouseDownFlag = false;
        OgreImage ogreimage = null;
        EsdSceneManager esmanager = null;
        public AddModelTool()
        {
            ogreimage = EsdSceneManager.Singleton.OgreImage;
            esmanager = EsdSceneManager.Singleton;
        }
        string modelname;
        string group;
        public void SetAddModel(string modelname, string group)
        {
            this.modelname = modelname;
            this.group = group;
        }
        //当激活该工具时会调用的方法
        public override void Click()
        {
            if (CurrentEntity != null)
            {
                esmanager.AddmodelNode.DetachObject(CurrentEntity);
                CurrentEntity.Dispose();
            }

            //查看当前模型列表中是否有选中的项，没有则返回，当单模型列表是会响应该事伯
           // if (MainOgreForm.Singleton.listView1.SelectedItems.Count < 0)
           //     return;
            //得到当前选择模型的名称
            //string ModelName = MainOgreForm.Singleton.listView1.SelectedItems[0].Text;
            //string group = MainOgreForm.Singleton.listView1.SelectedItems[0].Group.Header;
            ms = EsdSceneManager.Singleton.ModelGroup.GetMeshName(modelname, group);
            

            //得到创建实体的名称，根据当前时间得到，不会有重复
            string name = Guid.NewGuid().ToString("N");
            //创建模型实体
            CurrentEntity = ogreimage.SceneManager.CreateEntity(name, ms.ModelName);
            esmanager.AddmodelNode.ResetToInitialState();

            //旋转模型，饶X轴转90度，因为创建模型和下载的模型视角不同，即初始到当前场景时会躺下，
            float xx = 90 * (float)(System.Math.PI / 180);
            Quaternion xp = new Quaternion(new Radian(xx), Vector3.UNIT_X);
            esmanager.AddmodelNode.Orientation *= xp;


            esmanager.AddmodelNode.Scale(ms.Scale, ms.Scale, ms.Scale);
            //将模型实体加入到场景节点中
            esmanager.AddmodelNode.AttachObject(CurrentEntity);
        }
        public override void Init()
        {
            CurrentEntity = null;
            ms = null;

        }
        public override void MouseDown(object sender, MouseButtonEventArgs e)
        {
            MouseDownPt = e.GetPosition(sender as System.Windows.Controls.Image); 
            TempMouseMovePt = e.GetPosition(sender as System.Windows.Controls.Image); 
            MouseDownFlag = true;
        }
        public override void MouseMove(object sender, MouseEventArgs e)
        {
            var temppoing = e.GetPosition(sender as System.Windows.Controls.Image);
            if (MouseDownFlag)
            {
                Vector3 v1 = ogreimage.ScreenPtToSpaceVector(temppoing);
                Vector3 v2 = ogreimage.ScreenPtToSpaceVector(TempMouseMovePt);

                float offsetx = v1.x - v2.x;
                float offsety = v1.y - v2.y;
                Vector3 position = ogreimage.Camera.Position;

                position.x -= offsetx;
                position.y -= offsety;

                ogreimage.Camera.Position = position;

                Vector3 lockat = ogreimage.LockAt;
                lockat.x -= offsetx;
                lockat.y -= offsety;

                ogreimage.LockAt = lockat;

                ogreimage.UpdataCamera();
                TempMouseMovePt = temppoing;
            }
            else
            {
                Vector3 v1 = ogreimage.ScreenPtToSpaceVector(temppoing);

                esmanager.AddmodelNode.Position = v1;
            }
        }
        public override void MouseUp(object sender, MouseButtonEventArgs e)
        {
            MouseDownFlag = false;
            var temppoing = e.GetPosition(sender as System.Windows.Controls.Image);
            //增加模型
            if (MouseDownPt.X == temppoing.X && MouseDownPt.Y == temppoing.Y)
            {
                //得到创建实体的名称，根据当前时间得到，不会有重复
                string name = Guid.NewGuid().ToString("N");
                //创建模型实体
                Entity ent = ogreimage.SceneManager.CreateEntity(name, ms.ModelName);

                SceneNode node = esmanager.MainNode.CreateChildSceneNode();

                //旋转模型，饶X轴转90度，因为创建模型和下载的模型视角不同，即初始到当前场景时会躺下，
                float xx = 90 * (float)(System.Math.PI / 180);
                Quaternion x = new Quaternion(new Radian(xx), Vector3.UNIT_X);
                node.Orientation *= x;
                node.Scale(ms.Scale, ms.Scale, ms.Scale);
                //将模型实体加入到场景节点中
                node.AttachObject(ent);
                node.Position = esmanager.AddmodelNode.Position;

                //向模型状态链表中增加模型信息，以便更新状态和保存场景信息

                ModelEntryStruct mm = esmanager.ModelDataManage.AddRemoveState(node, 1);
                AddModelForm dlg = new AddModelForm();

                if ((bool)dlg.ShowDialog())
                {
                    mm.名称 = dlg.name_textBox.Text;
                    mm.备注属性 = dlg.remark_textBox.Text;
                    if (dlg.picture_textBox.Text != "")
                    {
                        mm.图片名称 = dlg.picture_textBox.Text;
                        FileStream file = File.OpenRead(dlg.picturename);
                        BinaryReader reader = new BinaryReader(file);
                        mm.图片 = reader.ReadBytes((int)file.Length);
                        reader.Close();
                        file.Close();
                    }
                    if (dlg.vido_textBox.Text != "")
                    {
                        mm.视频名称 = dlg.vido_textBox.Text;
                        FileStream file = File.OpenRead(dlg.vidoname);
                        BinaryReader reader = new BinaryReader(file);
                        mm.视频 = reader.ReadBytes((int)file.Length);
                        reader.Close();
                        file.Close();
                    }
                }
                //更新模型的旋转角度，因为旋转是用四元素做。通过节点得不到饶某个轴旋转的角度。所以在这单独记下，
                esmanager.ModelDataManage.UpdateModelState(node, 'x', xx);
                //设置当前工具为漫游
                ToolManage.Singleton.ToolType = typeof(SelectModelTool);
            }
        }
    }
}
