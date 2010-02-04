using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EsdCommon;
using OgreLib;
using System.Windows;
using Mogre;
using System.Windows.Input;
using EsdCommon.Tool;

namespace Esd.Tool
{
    public class AddWaterTool : AbstractTool
    {
        bool starflag = false;

        // List<Vector3>  PtList = new List<Vector3>();
        DrawLine3DClass dlc = null;
        WaterGrass wg = null;
         OgreImage ogreimage = null;
        EsdSceneManager esmanager = null;
        public AddWaterTool()
        {
            ogreimage = EsdSceneManager.Singleton.OgreImage;
            esmanager = EsdSceneManager.Singleton;
        }
        public override void Click()
        {
            //PtList.Clear();
            starflag = false;
        }
        public override void MouseDown(object sender, MouseButtonEventArgs e)
        {
            var temppt = e.GetPosition(sender as System.Windows.Controls.Image);
            if (!esmanager.IsStarEdit)
            {
                MessageBox.Show("请新建一个场景");
                return;
            }
            Vector3 vv = ogreimage.ScreenPtToSpaceVector(temppt);

            if (e.LeftButton== MouseButtonState.Pressed)
            {
                if (!starflag)
                {
                    //开始绘制
                    //PtList.Clear();
                    wg = new WaterGrass();
                    starflag = true;
                }

                if (starflag)
                {
                    wg.PtList.Add(vv);
                    if (wg.PtList.Count > 2)
                    {
                        if (wg.PtList.Count == 3)
                        {
                            string meshname = Guid.NewGuid().ToString();
                            dlc = new DrawLine3DClass(wg.PtList, "OceanCg", meshname);
                            dlc.ManualLineObject.RenderQueueGroup = (byte)80;
                            SceneNode node = esmanager.WRnode.CreateChildSceneNode();
                            node.AttachObject(dlc.ManualLineObject);

                            wg.MaterialName = "OceanCg";
                            wg.Name = meshname;
                            //wg.PtList = new List<Vector3>(PtList);
                            esmanager.ModelDataManage.modelEntry.水和草.Add(wg);
                        }
                        else
                        {
                            //PtList.Add(vv);
                            wg.PtList.Add(vv);
                            dlc.AddPt(vv);
                        }
                    }
                }
            }
            else if (e.RightButton== MouseButtonState.Pressed)
            {
                starflag = false;
                if (wg.PtList.Count > 1)
                {
                    //PtList.Add(vv);
                    wg.PtList.Add(vv);
                    //绘制水
                    dlc.AddPt(vv);
                }
               // MainOgreForm.Singleton.panel1.Cursor = Cursors.Arrow;
                //设置当前工具为漫游
                ToolManage.Singleton.ToolType = typeof(SelectModelTool);
               //MainOgreForm.Singleton.ToolManageObject.ToolType = typeof(SelectModelTool);
            }

        }


    }
}
