using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using System.Windows.Input;
using System.Windows;
using Esd;
using OgreLib;

namespace EsdCommon.Tool
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
        OgreImage ogreimage=null;
        EsdSceneManager esmanager = null;
        public bool PeopleChecked
        {
            get;set;
        }
        
        public PanToolClass()
        {
            ogreimage = EsdSceneManager.Singleton.OgreImage;
            esmanager =EsdSceneManager.Singleton;
            PeopleChecked = false;
            
        }
        public override void MouseDown(object sender, MouseButtonEventArgs e)
        {            
            //得到当前按下的是左键还是右键，以便看是漫游还是透视或俯视
            if (e.LeftButton== MouseButtonState.Pressed)
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
            MouseDownPt = e.GetPosition(sender as System.Windows.Controls.Image);// e.Location;
            MouseDownPtTemp = e.GetPosition(sender as System.Windows.Controls.Image);// e.Location;
            autopanflag = false;

        }
        public override void MouseMove(object sender, MouseEventArgs e)
        {
            var temppoint = e.GetPosition(sender as System.Windows.Controls.Image);
            if (MouseDownFlag)
            {
                switch (CurrentOperate)
                {
                    case 1://移动，此功能同地图漫游算法相同，
                        {

                            if (esmanager.PanState)
                                break;
                            //将屏幕点转换成三维点，
                            Vector3 v1 = ogreimage.ScreenPtToSpaceVector(temppoint);
                            Vector3 v2 = ogreimage.ScreenPtToSpaceVector(MouseDownPt);
                            //得到三维空间中的漫游偏移
                            float offsetx = v1.x - v2.x;
                            float offsety = v1.y - v2.y;
                            //得到当前摄像机的位置，
                            Vector3 position = ogreimage.Camera.Position;
                            //加上偏移
                            position.x -= offsetx;
                            position.y -= offsety;
                            //更新摄像机的位置
                            ogreimage.Camera.Position = position;
                            //更新摄像机观看位置
                            Vector3 lockat = ogreimage.LockAt;
                            lockat.x -= offsetx;
                            lockat.y -= offsety;
                            ogreimage.LockAt = lockat;
                            //更新摄像机
                            ogreimage.UpdataCamera();
                        }
                        break;
                    case 2://俯视旋转
                        {
                           
                            //得到俯视，透视的角度
                            float offsetx = (float)(temppoint.X - MouseDownPt.X) / (float)ogreimage.PixelWidth;// MainOgreForm.Singleton.panel1.Width;
                            float offsety = (float)(temppoint.Y - MouseDownPt.Y) / (float)ogreimage.PixelHeight;// MainOgreForm.Singleton.panel1.Height;
                            //更新角度
                            ogreimage.CamerRoateDegree += offsetx;
                            ogreimage.CamerLookdownDegree += offsety;
                            ogreimage.UpdataCamera();
                        }
                        break;
                    default:
                        break;
                }
                MouseDownPt = temppoint;
            }
        }
        public override void MouseUp(object sender, MouseButtonEventArgs e)
        {
            var temppoint = e.GetPosition(sender as System.Windows.Controls.Image);
            MouseDownFlag = false;
            //当前用户只单击了右键，就开始自动漫游
            if (MouseDownPtTemp.X == temppoint.X && MouseDownPtTemp.Y == temppoint.Y && e.RightButton== MouseButtonState.Pressed)
            {
                Vector3 v1 = ogreimage.ScreenPtToSpaceVector(temppoint);
                //得到自动漫游向量，即方向
                autopanvertor3 = v1 - ogreimage.LockAt;
                autopanvertor3.z = 0;
                autopanflag = true;
            }

            //当前用户只单击了左键，就开始移动模型
            if (MouseDownPtTemp.X == temppoint.X && MouseDownPtTemp.Y == temppoint.Y && e.LeftButton == MouseButtonState.Pressed && PeopleChecked)
            {
                Vector3 v1 = ogreimage.ScreenPtToSpaceVector(temppoint);
                esmanager.Deslocate = new Point(v1.x, v1.y);
                //ogreimage.deslocate = new PointF(v1.x, v1.y);
            }
        }
        public override void KeyDown(object sender, KeyEventArgs e)
        {
            if (esmanager.PanState)
            {
                Vector3 autopanvertor3 = new Vector3();
                //得到当前摄像机的位置，
                Vector3 position = ogreimage.Camera.Position;
                Vector3 lockat = ogreimage.LockAt;               

                switch (e.Key)
                {
                    case Key.W:
                        {
                            autopanvertor3 = lockat - position;
                        }
                        break;
                    case Key.A:
                        {
                            autopanvertor3 = lockat - position;
                            float xx = 90 * (float)(System.Math.PI / 180);
                            Quaternion q = new Quaternion(new Radian(xx), Vector3.UNIT_Z);
                            autopanvertor3.z = 0;
                            autopanvertor3 = q * autopanvertor3;
                        }
                        break;
                    case Key.S:
                        {
                            autopanvertor3 = position - lockat;
                        }
                        break;
                    case Key.D:
                        {
                            autopanvertor3 = lockat - position;
                            float xx = -90 * (float)(System.Math.PI / 180);
                            Quaternion q = new Quaternion(new Radian(xx), Vector3.UNIT_Z);
                            autopanvertor3.z = 0;
                            autopanvertor3 = q * autopanvertor3;
                        }
                        break;
                    default:
                        return;
                }

                esmanager.animFlag = true;
                esmanager.animState.Enabled = true;
                Vector3 v1 = esmanager.GetPointOnLine(lockat, autopanvertor3.x, autopanvertor3.y, 0, 0.5f);
                Vector3 v2 = esmanager.GetPointOnLine(position, autopanvertor3.x, autopanvertor3.y, 0, 0.5f);

                Vector3 tempv = new Vector3((float)esmanager.manlocate.X, (float)esmanager.manlocate.Y, 0);

                Vector3 v3 = esmanager.GetPointOnLine(tempv, autopanvertor3.x, autopanvertor3.y, 0, 0.5f);

                //更新人物模型位置
                double dis = System.Math.Sqrt((esmanager.manlocate.X - v3.x) * (esmanager.manlocate.X - v3.x) + (esmanager.manlocate.Y - v3.y) * (esmanager.manlocate.Y - v3.y));

                float angle = esmanager.GetManAngle(new Point(v3.x, v3.y), esmanager.manlocate, 0.5f);

                Quaternion y = new Quaternion(new Radian(-(float)ogreimage.CamerRoateDegree), Vector3.UNIT_Z);
                float xxx = 90 * (float)(System.Math.PI / 180);
                Quaternion x = new Quaternion(new Radian(xxx), Vector3.UNIT_X);

                esmanager.animNode.Orientation = y * x;
                esmanager.animNode.Position = v1;
                esmanager.manlocate.X = v1.x;
                esmanager.manlocate.Y = v1.y;
                esmanager.dpt = new Point(v1.x, v1.y);
                //更新摄像机的位置
                ogreimage.Camera.Position = v2;
                ogreimage.LockAt = v1;
                //OgreView.Singleton.animNode.Position = new Vector3(v1.x, v1.y, 0);
                //OgreView.Singleton.deslocate = new PointF(v1.x, v1.y);
                //更新摄像机
                ogreimage.UpdataCamera();
            }
        }

        public override void KeyUp(object sender, KeyEventArgs e)
        {
            esmanager.animFlag = false;
            esmanager.animState.Enabled = false;
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
            float d = ogreimage.CamerDistanceLock * 0.015f;
            //根据自动漫游的方向，移动一定的距离，距离为d
            Vector3 v1 = esmanager.GetPointOnLine(ogreimage.LockAt, autopanvertor3.x, autopanvertor3.y, autopanvertor3.z, d);
            //更新摄像机位置
            float offsetx = ogreimage.LockAt.x - v1.x;
            float offsety = ogreimage.LockAt.y - v1.y;
            Vector3 position = ogreimage.Camera.Position;
            position.x -= offsetx;
            position.y -= offsety;
            ogreimage.Camera.Position = position;
            //更新所看方向位置
            ogreimage.LockAt = v1;
            ogreimage.UpdataCamera();

        }
        //停止自动漫游
        public override void Click()
        {
            autopanflag = false;
        }
    }
}
