using System;
using System.Collections.Generic;
using System.Text;
using Mogre;

namespace MyOgre
{
    class DrawLine3DClass
    {
        /// <summary>
        /// 线对象
        /// </summary>
        public ManualObject ManualLineObject;
        string ColorMaterial;
        List<Vector3> ptlist = new List<Vector3>();
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="PtList"></param>
        public DrawLine3DClass(List<Vector3> PtList, string ColorMaterial, string name)
        {
            this.ColorMaterial = ColorMaterial;
            this.ptlist = PtList;
            ManualLineObject = OgreView.Singleton.sceneMgr.CreateManualObject(name);

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
            OgreView.Singleton.sceneMgr.DestroyManualObject(ManualLineObject);
            ManualLineObject.Dispose();
        }
    }
}
