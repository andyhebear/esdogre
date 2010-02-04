using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Mogre;
using OgreLib;
using Esd;

namespace EsdCommon.Tool
{
    //选择模型工具。也可做漫游操作
    public class SelectModelTool : AbstractTool
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

        public Action<bool, SceneNode> SetModifyTextEd
        {
            get;
            set;
        }

        OgreImage ogreimage = null;
        EsdSceneManager esmanager = null;
        public SelectModelTool()
        {
            ogreimage = EsdSceneManager.Singleton.OgreImage;
            esmanager = EsdSceneManager.Singleton;
        }
        public override void MouseDown(object sender, MouseButtonEventArgs e)
        {
            //同漫游操作。
            if (e.LeftButton== MouseButtonState.Pressed)
            {
                CurrentOperate = 1;
            }
            else
            {
                CurrentOperate = 2;
            }
            MouseDownFlag = true;
            MouseDownPt = e.GetPosition(sender as System.Windows.Controls.Image);
            MouseDownPtTemp = e.GetPosition(sender as System.Windows.Controls.Image);

            //选择模型，并设置当前标志
            isSelectModelFlag = SelectObject(e.GetPosition(sender as System.Windows.Controls.Image));
            if (!isSelectModelFlag)
            {
                SelectWaterGrass(e.GetPosition(sender as System.Windows.Controls.Image));
            }
            MouseDownPt = e.GetPosition(sender as System.Windows.Controls.Image);
        }
        public override void MouseMove(object sender, MouseEventArgs e)
        {
            var temppt = e.GetPosition(sender as System.Windows.Controls.Image);
            if (isSelectModelFlag)//如果有模型被选择中，并且鼠标按下
            {
                //将屏幕点转换成三维点。以便计算偏移
                Vector3 v1 = ogreimage.ScreenPtToSpaceVector(temppt);
                Vector3 v2 = ogreimage.ScreenPtToSpaceVector(MouseDownPt);
                //计算偏移
                float offsetx = v1.x - v2.x;
                float offsety = v1.y - v2.y;
                Vector3 position = esmanager.CurrentOperateNode.Position;
                position.x += offsetx;
                position.y += offsety;
                //更新当前模型位置
                esmanager.CurrentOperateNode.Position = position;
                MouseDownPt = temppt;

                esmanager.ModelDataManage.UpdateModelState(esmanager.CurrentOperateNode, ' ', 0);
                //更新属性显示面板
                SetModifyTextEd(true, esmanager.CurrentOperateNode);
            }
            else
            {
                if (MouseDownFlag)//同漫游操作
                {
                    switch (CurrentOperate)
                    {
                        case 1://移动
                            {
                                Vector3 v1 = ogreimage.ScreenPtToSpaceVector(temppt);
                                Vector3 v2 = ogreimage.ScreenPtToSpaceVector(MouseDownPt);
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
                            }
                            break;
                        case 2://俯视旋转
                            {
                                float offsetx = (float)(temppt.X - MouseDownPt.X) / (float)ogreimage.PixelWidth;
                                float offsety = (float)(temppt.Y - MouseDownPt.Y) / (float)ogreimage.PixelHeight;

                                ogreimage.CamerRoateDegree += offsetx;
                                ogreimage.CamerLookdownDegree += offsety;
                                ogreimage.UpdataCamera();
                            }
                            break;
                        default:
                            break;
                    }
                    MouseDownPt = temppt;
                }
            }
        }
        public override void MouseUp(object sender, MouseButtonEventArgs e)
        {
            isSelectModelFlag = false;//鼠标抬起
            MouseDownFlag = false;
        }

        /// <summary>
        /// 选择模型对象
        /// </summary>
        public bool SelectObject(Point pt)
        {

            if (esmanager.MainNode == null)
                return false;

            if (esmanager.CurrentOperateNode != null)
            {
                esmanager.CurrentOperateNode.ShowBoundingBox = false;
            }
            if (esmanager.CurrentWgOperageNode != null)
            {
                esmanager.CurrentWgOperageNode.ShowBoundingBox = false;
            }
            float screenx = ((float)pt.X) / ((float)ogreimage.PixelWidth);
            float screeny = ((float)pt.Y) / ((float)ogreimage.PixelHeight);
            //根据屏幕点生成一条射线，用来选择模型
            Ray rayaaa = ogreimage.Camera.GetCameraToViewportRay(screenx, screeny);
            //临时保存与射线相交模型到屏幕的距离
            float dis = 0;
            bool flag = true;
            //循环得到每个模型，和射线进行比较，得到和射线相交并且距屏幕最近的模型
            foreach (SceneNode node in esmanager.MainNode.GetChildIterator())
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
                        esmanager.CurrentOperateNode = node;
                    }
                    else//如果有相交的模型，比较那个模型距离屏幕最近
                    {
                        if (dis < result.second)
                        {
                            dis = result.second;
                            esmanager.CurrentOperateNode = node;
                        }
                    }
                }
            }
            if (!flag)
            {
                //更新属性面板，
                SetModifyTextEd(true, esmanager.CurrentOperateNode);

                esmanager.CurrentOperateNode.ShowBoundingBox = true;

                return true;
            }
            //如果没有选择对象
            //将属性面板设置为不可用。
            SetModifyTextEd(false, esmanager.CurrentOperateNode);
            //将操作对象置为空。
            esmanager.CurrentOperateNode = null;

            return false;
        }
        public bool SelectWaterGrass(Point pt)
        {
            if (esmanager.WRnode == null)
                return false;

            if (esmanager.CurrentWgOperageNode != null)
            {
                esmanager.CurrentWgOperageNode.ShowBoundingBox = false;
            }
            float screenx = ((float)pt.X) / ((float)ogreimage.PixelWidth);
            float screeny = ((float)pt.Y) / ((float)ogreimage.PixelHeight);
            //根据屏幕点生成一条射线，用来选择模型
            Ray rayaaa = ogreimage.Camera.GetCameraToViewportRay(screenx, screeny);
            //临时保存与射线相交模型到屏幕的距离
            float dis = 0;
            bool flag = true;
            //循环得到每个模型，和射线进行比较，得到和射线相交并且距屏幕最近的模型
            foreach (SceneNode node in esmanager.WRnode.GetChildIterator())
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
                        esmanager.CurrentWgOperageNode = node;
                    }
                    else//如果有相交的模型，比较那个模型距离屏幕最近
                    {
                        if (dis < result.second)
                        {
                            dis = result.second;
                            esmanager.CurrentWgOperageNode = node;
                        }
                    }
                }
            }
            if (!flag)
            {
                esmanager.CurrentWgOperageNode.ShowBoundingBox = true;
                return true;
            }
            //将操作对象置为空。
            esmanager.CurrentWgOperageNode = null;
            return false;
        }


    }
}
