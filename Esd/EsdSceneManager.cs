using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using OgreLib;
using EsdCommon;

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
        public static void CreateSceneManager(OgreImage ogreimg)
        {
            singleton = new EsdSceneManager();
            singleton.OgreImage = ogreimg;
            singleton.ModelDataManage = new ModelDataMaintenance();
        }

    }
}
