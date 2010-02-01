using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace MyOgre
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

        //当激活该工具时会调用的方法
        public override void Click()
        {
            if (CurrentEntity != null)
            {
                OgreView.Singleton.addmodelNode.DetachObject(CurrentEntity);
                CurrentEntity.Dispose();
            }

            //查看当前模型列表中是否有选中的项，没有则返回，当单模型列表是会响应该事伯
            if (MainOgreForm.Singleton.listView1.SelectedItems.Count < 0)
                return;
            //得到当前选择模型的名称
           string ModelName = MainOgreForm.Singleton.listView1.SelectedItems[0].Text;
            string group = MainOgreForm.Singleton.listView1.SelectedItems[0].Group.Header;
            ms = MainOgreForm.Singleton.ModelGroup.GetMeshName(ModelName, group);
           

            //得到创建实体的名称，根据当前时间得到，不会有重复
            string name = MainOgreForm.GetDateName();
            //创建模型实体
            CurrentEntity = OgreView.Singleton.sceneMgr.CreateEntity(name, ms.ModelName);
            OgreView.Singleton.addmodelNode.ResetToInitialState();

            //旋转模型，饶X轴转90度，因为创建模型和下载的模型视角不同，即初始到当前场景时会躺下，
            float xx = 90 * (float)(System.Math.PI / 180);
            Quaternion xp = new Quaternion(new Radian(xx), Vector3.UNIT_X);
            OgreView.Singleton.addmodelNode.Orientation *= xp;


            OgreView.Singleton.addmodelNode.Scale(ms.Scale, ms.Scale, ms.Scale);
            //将模型实体加入到场景节点中
            OgreView.Singleton.addmodelNode.AttachObject(CurrentEntity);
        }
        public override void Init()
        {
            CurrentEntity = null;
            ms = null;

        }
        public override void MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            MouseDownPt = e.Location;
            TempMouseMovePt = e.Location;
            MouseDownFlag = true;
        }
        public override void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (MouseDownFlag)
            {
                Vector3 v1 = OgreView.Singleton.ScreenPtToSpaceVector(e.Location);
                Vector3 v2 = OgreView.Singleton.ScreenPtToSpaceVector(TempMouseMovePt);

                float offsetx = v1.x - v2.x;
                float offsety = v1.y - v2.y;
                Vector3 position = OgreView.Singleton.camera.Position;

                position.x -= offsetx;
                position.y -= offsety;

                OgreView.Singleton.camera.Position = position;

                Vector3 lockat = OgreView.Singleton.LockAt;
                lockat.x -= offsetx;
                lockat.y -= offsety;

                OgreView.Singleton.LockAt = lockat;

                OgreView.Singleton.UpdataCamera();
                TempMouseMovePt = e.Location;
            }
            else
            {
                Vector3 v1 = OgreView.Singleton.ScreenPtToSpaceVector(e.Location);

                OgreView.Singleton.addmodelNode.Position = v1;
            }
        }
        public override void MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            MouseDownFlag = false;
            //增加模型
            if (MouseDownPt.X == e.X && MouseDownPt.Y == e.Y)
            {
                //得到创建实体的名称，根据当前时间得到，不会有重复
                string name = MainOgreForm.GetDateName();
                //创建模型实体
                Entity ent = OgreView.Singleton.sceneMgr.CreateEntity(name, ms.ModelName);

                SceneNode node = OgreView.Singleton.mainNode.CreateChildSceneNode();

                //旋转模型，饶X轴转90度，因为创建模型和下载的模型视角不同，即初始到当前场景时会躺下，
                float xx = 90 * (float)(System.Math.PI / 180);
                Quaternion x = new Quaternion(new Radian(xx), Vector3.UNIT_X);
                node.Orientation *= x;
                node.Scale(ms.Scale, ms.Scale, ms.Scale);
                //将模型实体加入到场景节点中
                node.AttachObject(ent);
                node.Position = OgreView.Singleton.addmodelNode.Position;

                //向模型状态链表中增加模型信息，以便更新状态和保存场景信息
                ModelEntryStruct mm = MainOgreForm.Singleton.ModelDataManage.AddRemoveState(node, 1);
                AddModelForm dlg = new AddModelForm();
                if (dlg.ShowDialog() == DialogResult.OK)
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
                MainOgreForm.Singleton.ModelDataManage.UpdateModelState(node, 'x', xx);
                //设置当前工具为漫游
                MainOgreForm.Singleton.ToolManageObject.ToolType = typeof(SelectModelTool);
            }
        }
    }
}
