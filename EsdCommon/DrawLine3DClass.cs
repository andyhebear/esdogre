using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OgreLib;
using Esd;
using Mogre;

namespace EsdCommon
{
    public class DrawLine3DClass
    {
        /// <summary>
        /// 线对象
        /// </summary>
        public ManualObject ManualLineObject;
        string ColorMaterial;
        List<Vector3> ptlist = new List<Vector3>();
        OgreImage ogreimage = null;
        EsdSceneManager esmanager = null;
        public DrawLine3DClass()
        {
            ogreimage = EsdSceneManager.Singleton.OgreImage;
            esmanager = EsdSceneManager.Singleton;
        }
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="PtList"></param>
        public DrawLine3DClass(List<Vector3> PtList, string ColorMaterial, string name)
        {
            ogreimage = EsdSceneManager.Singleton.OgreImage;
            esmanager = EsdSceneManager.Singleton;

            this.ColorMaterial = ColorMaterial;
            this.ptlist = PtList;
            ManualLineObject = ogreimage.SceneManager.CreateManualObject(name);

            ManualLineObject.Begin(ColorMaterial, RenderOperation.OperationTypes.OT_TRIANGLE_FAN);

            foreach (Vector3 pt in PtList)
            {
                ManualLineObject.Position(pt);
                ManualLineObject.TextureCoord(pt.x / 100, pt.y / 100);
            }

            ManualLineObject.End();

        }
        public void AddPt(Vector3 pt)
        {
            this.ptlist.Add(pt);
            ManualLineObject.Begin(ColorMaterial, RenderOperation.OperationTypes.OT_TRIANGLE_FAN);
            foreach (Vector3 ptt in this.ptlist)
            {
                ManualLineObject.Position(ptt);
                ManualLineObject.TextureCoord(ptt.x / 100, ptt.y / 100);
            }

            ManualLineObject.End();
        }
        /// <summary>
        /// 释放所占资源
        /// </summary>
        public void Dispose()
        {
            ogreimage.SceneManager.DestroyManualObject(ManualLineObject);
            ManualLineObject.Dispose();
        }
    }
}
