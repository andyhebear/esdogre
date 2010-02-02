using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using OgreLib;
using EsdCommon;
using System.Windows;

namespace Esd
{
    /// <summary>
    /// 电子沙盘场景管理
    /// </summary>
    public class EsdSceneManager
    {
        private static EsdSceneManager singleton = null;
        public static EsdSceneManager Singleton
        {
            get
            {
                return singleton;
            }
        }

        public SceneManager SceneManager
        {
            get
            {
                return OgreImage.SceneManager;
            }
       
        }
        //模型的管理（保存和读取）
        public ModelDataMaintenance ModelDataManage
        {
            get;
            set;
        }
        /// <summary>
        /// 背景材质引用
        /// </summary>
        public MaterialPtr MaterialPtr
        {
            get;
            set;
        }
        //场景地面节点
        public SceneNode FloorNode
        {
            get;
            set;
        }
        public bool IsStarEdit
        {
            get;
            set;
        }
        public OgreImage OgreImage
        {
            get;
            set;
        }
        /// <summary>
        /// 当前漫游的方式
        /// </summary>
        public bool PanState
        {
            get;
            set;
        }
        #region 人物状态变量
        public Point manlocate = new Point();
        public Point dpt = new Point();
        public Point Deslocate
        {
            get
            {
                return dpt;
            }
            set
            {
                dpt = value;
                double dis = System.Math.Sqrt((manlocate.X - dpt.X) * (manlocate.X - dpt.X) + (manlocate.Y - dpt.Y) * (manlocate.Y - dpt.Y));
                movestep = (int)(dis / 0.5f);
                animState.Enabled = true;
                float angle = GetManAngle(dpt, manlocate, dis);

                Quaternion y = new Quaternion(new Radian(angle), Vector3.UNIT_Z);
                float xx = 90 * (float)(System.Math.PI / 180);
                Quaternion x = new Quaternion(new Radian(xx), Vector3.UNIT_X);

                animNode.Orientation = y * x;
            }
        }
        private int movestep = 0;
        /// <summary>
        /// 人物模型动画标志，
        /// </summary>
        public bool animFlag = false;
        //动画状态
        public AnimationState animState;
        public Entity animEntry = null;
        /// <summary>
        /// 人物节点
        /// </summary>
        public SceneNode animNode = null;
        #endregion
        public static void CreateSceneManager(OgreImage ogreimg)
        {
            singleton = new EsdSceneManager();
            singleton.OgreImage = ogreimg;
            singleton.ModelDataManage = new ModelDataMaintenance();
            singleton.PanState = false;
        }

        public float GetManAngle(Point dpt, Point manlocate, double dis)
        {
            float angle = 0;
            if (dpt.X >= manlocate.X && dpt.Y >= manlocate.Y)
            {
                angle = (float)System.Math.Asin((dpt.Y - manlocate.Y) / dis) - (float)System.Math.PI / 2.0f;
            }
            else if (dpt.Y >= manlocate.Y && dpt.X <= manlocate.X)
            {
                angle = (float)System.Math.PI / 2 - (float)System.Math.Asin((dpt.Y - manlocate.Y) / dis);
            }
            else if (dpt.Y <= manlocate.Y && dpt.X <= manlocate.X)
            {
                angle = (float)System.Math.Asin((dpt.Y - manlocate.Y) / dis) - (float)System.Math.PI;
            }
            else if (dpt.Y <= manlocate.Y && dpt.X >= manlocate.X)
            {
                angle = (float)System.Math.Asin((dpt.Y - manlocate.Y) / dis) - (float)System.Math.PI + (float)System.Math.PI / 2;
            }


            if (angle > System.Math.PI / 2 && angle < System.Math.PI + System.Math.PI / 2)
            {
                angle += (float)System.Math.PI;
            }
            return angle;
        }
        /// <summary>
        /// 根据起点与方向向量，求在线上指定距离的点
        /// </summary>
        /// <param name="pt">起点</param>
        /// <param name="x">方向向量x</param>
        /// <param name="y">方向向量y</param>
        /// <param name="z">方向向量z</param>
        /// <param name="d">距离</param>
        /// <returns>计算点</returns>
        public Vector3 GetPointOnLine(Vector3 pt, double x, double y, double z, double d)
        {
            double max = System.Math.Max(System.Math.Abs(x), System.Math.Abs(y));
            max = System.Math.Max(max, System.Math.Abs(z));
            x /= max;
            y /= max;
            z /= max;

            x = pt.x + x * d;
            y = pt.y + y * d;
            z = pt.z + z * d;
            return new Vector3((float)x, (float)y, (float)z);
        }
       

    }
}
