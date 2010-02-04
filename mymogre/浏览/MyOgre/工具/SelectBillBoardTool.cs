using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Mogre;

namespace MyOgre
{
    public class SelectBillBoardTool:AbstractTool
    {
        /// <summary>
        /// 当前操作
        /// 1:移动
        /// 2:旋转俯视
        /// </summary>
        int CurrentOperate = 1;
        /// <summary>
        /// 当前鼠标是否按下
        /// </summary>
        bool MouseDownFlag = false;
        /// <summary>
        /// 鼠标按下点
        /// </summary>
        Point MouseDownPt;
        /// <summary>
        /// 用来保存鼠标按下状态.该值直到鼠标抬起进才改变
        /// </summary>
        Point MouseDownPtTemp;

        //是否有模型被选择中
        bool isSelectModelFlag = false;

        public override void MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //同漫游操作。
            if (e.Button == MouseButtons.Left)
            {
                CurrentOperate = 1;
            }
            else
            {
                CurrentOperate = 2;
            }
            MouseDownFlag = true;
            MouseDownPt = e.Location;
            MouseDownPtTemp = e.Location;

            //选择模型，并设置当前标志
            isSelectModelFlag = SelectObject(e.Location);
            MouseDownPt = e.Location;
        }
        public override void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (isSelectModelFlag)//如果有模型被选择中，并且鼠标按下
            {
                //将屏幕点转换成三维点。以便计算偏移
                Vector3 v1 = OgreView.Singleton.ScreenPtToSpaceVector(e.Location);
                Vector3 v2 = OgreView.Singleton.ScreenPtToSpaceVector(MouseDownPt);
                //计算偏移
                float offsetx = v1.x - v2.x;
                float offsety = v1.y - v2.y;
                Vector3 position = OgreView.Singleton.CurrentOperateNode.Position;
                position.x += offsetx;
                position.y += offsety;
                //更新当前模型位置
                OgreView.Singleton.CurrentOperateNode.Position = position;
                MouseDownPt = e.Location;

                MainOgreForm.Singleton.ModelDataManage.UpdateBillState(OgreView.Singleton.CurrentOperateNode);
              
            }
            else
            {
                if (MouseDownFlag)//同漫游操作
                {
                    switch (CurrentOperate)
                    {
                        case 1://移动
                            {
                                Vector3 v1 = OgreView.Singleton.ScreenPtToSpaceVector(e.Location);
                                Vector3 v2 = OgreView.Singleton.ScreenPtToSpaceVector(MouseDownPt);
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
                            }
                            break;
                        case 2://俯视旋转
                            {
                                float offsetx = (float)(e.X - MouseDownPt.X) / (float)MainOgreForm.Singleton.panel1.Width;
                                float offsety = (float)(e.Y - MouseDownPt.Y) / (float)MainOgreForm.Singleton.panel1.Height;

                                OgreView.Singleton.CamerRoateDegree += offsetx;
                                OgreView.Singleton.CamerLookdownDegree += offsety;
                                OgreView.Singleton.UpdataCamera();
                            }
                            break;
                        default:
                            break;
                    }
                    MouseDownPt = e.Location;
                }
            }
        }
        public override void MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            isSelectModelFlag = false;//鼠标抬起
            MouseDownFlag = false;
        }
        /// <summary>
        /// 选择对象
        /// </summary>
        public bool SelectObject(Point pt)
        {
            if (OgreView.Singleton.mainNode == null)
                return false;

            if (OgreView.Singleton.CurrentOperateNode != null)
            {
                OgreView.Singleton.CurrentOperateNode.ShowBoundingBox = false;
            }
            float screenx = ((float)pt.X) / ((float)OgreView.Singleton.PtrWidth);
            float screeny = ((float)pt.Y) / ((float)OgreView.Singleton.PtrHeight);
            //根据屏幕点生成一条射线，用来选择模型
            Ray rayaaa = OgreView.Singleton.camera.GetCameraToViewportRay(screenx, screeny);
            //临时保存与射线相交模型到屏幕的距离
            float dis = 0;
            bool flag = true;
            //循环得到每个模型，和射线进行比较，得到和射线相交并且距屏幕最近的模型
            foreach (SceneNode node in OgreView.Singleton.billNode.GetChildIterator())
            {
                //得到模型的边框值
                AxisAlignedBox aa = node._getWorldAABB();
                //是否和射线相交
                Pair<bool, float> result = rayaaa.Intersects(aa);
                if (result.first)
                {
                    //如果是相交的第一个模型，得到距离和模型
                    if (flag)
                    {
                        flag = false;
                        dis = result.second;
                        OgreView.Singleton.CurrentOperateNode = node;
                    }
                    else//如果有相交的模型，比较那个模型距离屏幕最近
                    {
                        if (dis < result.second)
                        {
                            dis = result.second;
                            OgreView.Singleton.CurrentOperateNode = node;
                        }
                    }
                }
            }
            if (!flag)
            {
               

                OgreView.Singleton.CurrentOperateNode.ShowBoundingBox = true;

                return true;
            }
            
            //将操作对象置为空。
            OgreView.Singleton.CurrentOperateNode = null;

            return false;
        }

       
    }
}
