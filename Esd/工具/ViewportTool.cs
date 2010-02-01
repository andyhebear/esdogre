using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Esd.Tool
{
    public enum ViewportToolEnum
    {
        None,
        ViewportUp,
        ViewportDown,
        ViewportLeft,
        ViewportRight
    }
    public class ViewportTool
    {
        private ViewportToolEnum curop = ViewportToolEnum.None;
        public void MouseDown(ViewportToolEnum opt)
        {
            curop = opt;
        }
        public void MouseLeave(ViewportToolEnum opt)
        {
            curop = ViewportToolEnum.None;
        }
        public void MouseUp(ViewportToolEnum opt)
        {
            curop = ViewportToolEnum.None;
        }
        /// <summary>
        /// 更新视野范围
        /// </summary>
        public void UpdateViewport()
        {
            //工具栏场景操作
            switch (curop)
            {
                case ViewportToolEnum.ViewportDown:
                    EsdSceneManager.Singleton.OgreImage.CamerLookdownDegree -= System.Math.PI / 180;
                    EsdSceneManager.Singleton.OgreImage.UpdataCamera();
                    break;
                case ViewportToolEnum.ViewportUp:
                    EsdSceneManager.Singleton.OgreImage.CamerLookdownDegree += System.Math.PI / 180;
                    EsdSceneManager.Singleton.OgreImage.UpdataCamera();
                    break;
                case ViewportToolEnum.ViewportLeft:
                    EsdSceneManager.Singleton.OgreImage.CamerRoateDegree += System.Math.PI / 180;
                    EsdSceneManager.Singleton.OgreImage.UpdataCamera();
                    break;
                case ViewportToolEnum.ViewportRight:
                    EsdSceneManager.Singleton.OgreImage.CamerRoateDegree -= System.Math.PI / 180;
                    EsdSceneManager.Singleton.OgreImage.UpdataCamera();
                    break;
                default:
                    break;
            }
        }
    }
}
