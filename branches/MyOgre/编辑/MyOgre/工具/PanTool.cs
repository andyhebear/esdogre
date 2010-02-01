using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Mogre;
using System.Windows.Forms;

namespace MyOgre
{

    /// <summary>
    /// 漫游工具类
    /// </summary>
    public class PanToolClass : AbstractTool
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
        /// <summary>
        /// 自动漫游方向
        /// </summary>
        Vector3 autopanvertor3;
        /// <summary>
        /// 是否开始自动漫游
        /// </summary>
        bool autopanflag = false;

        public override void MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //得到当前按下的是左键还是右键，以便看是漫游还是透视或俯视
            if (e.Button == MouseButtons.Left)
            {
                CurrentOperate = 1;
            }
            else
            {
                CurrentOperate = 2;
            }
            //记录当前鼠标状态，
            //鼠标是否按下
            MouseDownFlag = true;
            MouseDownPt = e.Location;
            MouseDownPtTemp = e.Location;
            autopanflag = false;

        }
        public override void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (MouseDownFlag)
            {
                switch (CurrentOperate)
                {
                    case 1://移动，此功能同地图漫游算法相同，
                        {
                            //将屏幕点转换成三维点，
                            Vector3 v1 = OgreView.Singleton.ScreenPtToSpaceVector(e.Location);
                            Vector3 v2 = OgreView.Singleton.ScreenPtToSpaceVector(MouseDownPt);
                            //得到三维空间中的漫游偏移
                            float offsetx = v1.x - v2.x;
                            float offsety = v1.y - v2.y;
                            //得到当前摄像机的位置，
                            Vector3 position = OgreView.Singleton.camera.Position;
                            //加上偏移
                            position.x -= offsetx;
                            position.y -= offsety;
                            //更新摄像机的位置
                            OgreView.Singleton.camera.Position = position;
                            //更新摄像机观看位置
                            Vector3 lockat = OgreView.Singleton.LockAt;
                            lockat.x -= offsetx;
                            lockat.y -= offsety;
                            OgreView.Singleton.LockAt = lockat;
                            //更新摄像机
                            OgreView.Singleton.UpdataCamera();
                        }
                        break;
                    case 2://俯视旋转
                        {
                            //得到俯视，透视的角度
                            float offsetx = (float)(e.X - MouseDownPt.X) / (float)MainOgreForm.Singleton.panel1.Width;
                            float offsety = (float)(e.Y - MouseDownPt.Y) / (float)MainOgreForm.Singleton.panel1.Height;
                            //更新角度
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
        public override void MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            MouseDownFlag = false;
            //当前用户只单击了右键，就开始自动漫游
            if (MouseDownPtTemp.X == e.X && MouseDownPtTemp.Y == e.Y && e.Button == MouseButtons.Right)
            {
                Vector3 v1 = OgreView.Singleton.ScreenPtToSpaceVector(e.Location);
                //得到自动漫游向量，即方向
                autopanvertor3 = v1 - OgreView.Singleton.LockAt;
                autopanvertor3.z = 0;
                autopanflag = true;
            }
            
        }
        //定时器，每隔一段执行的方法
        public override void TimerTick(object sender, EventArgs e)
        {
            if (autopanflag)
            {
                //自动漫游
                AutoMoveCamera();
            }
        }
        //自动漫游
        public void AutoMoveCamera()
        {
            //得到摄像机的高度，以便计算自动漫游的步长
            float d = OgreView.Singleton.CamerDistanceLock * 0.015f;
            //根据自动漫游的方向，移动一定的距离，距离为d
            Vector3 v1 = OgreView.Singleton.GetPointOnLine(OgreView.Singleton.LockAt, autopanvertor3.x, autopanvertor3.y, autopanvertor3.z, d);
            //更新摄像机位置
            float offsetx = OgreView.Singleton.LockAt.x - v1.x;
            float offsety = OgreView.Singleton.LockAt.y - v1.y;
            Vector3 position = OgreView.Singleton.camera.Position;
            position.x -= offsetx;
            position.y -= offsety;
            OgreView.Singleton.camera.Position = position;
            //更新所看方向位置
            OgreView.Singleton.LockAt = v1;
            OgreView.Singleton.UpdataCamera();
          
        }
        //停止自动漫游
        public override void Click()
        {
            autopanflag = false;
        }
    }
}
