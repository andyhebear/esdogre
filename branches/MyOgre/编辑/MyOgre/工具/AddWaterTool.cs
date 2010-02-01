using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using System.Drawing;
using System.Windows.Forms;

namespace MyOgre
{
    public class AddWaterTool : AbstractTool
    {
        bool starflag = false;
       
       // List<Vector3>  PtList = new List<Vector3>();
        DrawLine3DClass dlc = null;
        WaterGrass wg = null;
        public override void Click()
        {
            //PtList.Clear();
            starflag = false;
        }
        public override void MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!MainOgreForm.Singleton .IsStarEdit)            
            {
                MessageBox.Show("请新建一个场景");
                return;
            }
            Vector3 vv = OgreView.Singleton.ScreenPtToSpaceVector(e.Location);

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
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
                            SceneNode node = OgreView.Singleton.WRnode.CreateChildSceneNode();
                            node.AttachObject(dlc.ManualLineObject);
                           
                            wg.MaterialName = "OceanCg";
                            wg.Name = meshname;
                            //wg.PtList = new List<Vector3>(PtList);
                            MainOgreForm.Singleton.ModelDataManage.modelEntry.水和草.Add(wg);
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
            else if (e.Button == MouseButtons.Right)
            {
                starflag = false;
                if (wg.PtList.Count > 1)
                {
                    //PtList.Add(vv);
                    wg.PtList.Add(vv);
                    //绘制水
                    dlc.AddPt(vv);
                }
                MainOgreForm.Singleton.panel1.Cursor = Cursors.Arrow;
                //设置当前工具为漫游
                MainOgreForm.Singleton.ToolManageObject.ToolType = typeof(SelectModelTool);
            }
            
        }
        
        
    }
}
