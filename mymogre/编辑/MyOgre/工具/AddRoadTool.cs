using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;

namespace MyOgre
{
    /// <summary>
    /// 增加路
    /// </summary>
    public class AddRoadTool: AbstractTool
    {
        bool starflag = false;
        string texturefilename = "";
        List<Vector3> PtList = new List<Vector3>();
        string roadname = "";
        float texturewidth = 1000;
        float textureheight = 1000;
        float swidht = MainOgreForm.Singleton.ModelDataManage.modelEntry.场景宽;
        float sheigh = MainOgreForm.Singleton.ModelDataManage.modelEntry.场景高;
        RoadStruct temproad = null;
        public override void Click()
        {
            swidht = MainOgreForm.Singleton.ModelDataManage.modelEntry.场景宽;
            sheigh = MainOgreForm.Singleton.ModelDataManage.modelEntry.场景高;
            starflag = false;
        }
        public override void MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
           
            if (!MainOgreForm.Singleton.IsStarEdit)
            {
                MessageBox.Show("请新建一个场景");
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                if (!starflag)
                {
                    temproad = new RoadStruct();
                    temproad.LineColor = MainOgreForm.Singleton.LineColor.ToArgb();
                    temproad.LineWidth = MainOgreForm.Singleton.LineWidth;
                    temproad.RoadColor = MainOgreForm.Singleton.RoadColor.ToArgb();
                    temproad.RoadWidth = MainOgreForm.Singleton.RoadWidth;
                    temproad.Name = Guid.NewGuid().ToString();
                    temproad.PtList = new List<Vector3>();
                    temproad.RoadName = temproad.Name;
                    //开始绘制
                    starflag = true;                    
                }
                if (starflag)
                {
                    Vector3 vv = OgreView.Singleton.ScreenPtToSpaceVector(e.Location);
                    vv.x = vv.x * (texturewidth / swidht) + texturewidth/2;
                    vv.y = -vv.y * (textureheight / sheigh) + textureheight/2;
                    temproad.PtList.Add(vv);
                    if (temproad.PtList.Count > 1)
                    {      
                        DrawRoad(temproad, (int)texturewidth,(int)textureheight);
                    }
                   
                }
            }
            else
            {
                if (starflag)
                {
                    Vector3 vv = OgreView.Singleton.ScreenPtToSpaceVector(e.Location);
                    vv.x = vv.x * (texturewidth / swidht) + texturewidth / 2;
                    vv.y = -vv.y * (textureheight / sheigh) + textureheight / 2;
                    temproad.PtList.Add(vv);
                    if (temproad.PtList.Count > 1)
                    {
                        DrawRoad(temproad, (int)texturewidth, (int)textureheight);
                    }

                    starflag = false;
                    //设置路名
                    
                    SetRoadNameForm dlg = new SetRoadNameForm();
                    dlg.road_textBox.Text = temproad.RoadName;
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        temproad.RoadName = dlg.road_textBox.Text;
                    }
                    
                    MainOgreForm.Singleton.ModelDataManage.modelEntry.路.Add(temproad);
                    MainOgreForm.Singleton.panel1.Cursor = Cursors.Arrow;
                    //设置当前工具为漫游
                    MainOgreForm.Singleton.ToolManageObject.ToolType = typeof(PanToolClass);
                }
            }
        }
        /// <summary>
        /// 绘制路
        /// </summary>
        /// <param name="road">路的数据对象</param>
        /// <param name="width">绘制路的图片宽度</param>
        /// <param name="height">绘制路的图片高度</param>
        static public void DrawRoad(RoadStruct road, int width, int height)
        {
            if (MainOgreForm.Singleton.materialptr == null)
                return;
            string texturefilename = Application.StartupPath + "\\TempBillBoard\\" + road.Name + ".png";
            
            //查看纹理是否存在
            if (File.Exists(texturefilename))
            {
                Pass p = MainOgreForm.Singleton.materialptr.GetTechnique(0).GetPass(0);
                int i = 0;

                foreach (TextureUnitState tus in p.GetTextureUnitStateIterator())
                {
                    if (tus.TextureName == texturefilename)
                    {
                        p.RemoveTextureUnitState((ushort)i);
                        break;
                    }
                    i++;
                }

                TextureManager.Singleton.Remove(texturefilename);
                File.Delete(texturefilename);
            }

            System.Drawing.Image bitmap = new System.Drawing.Bitmap(width, height);           
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.FillRectangle(Brushes.White, 0, 0, width, height);

            //设置路的样式
            Pen p1 = new Pen(Color.FromArgb(road.RoadColor), road.RoadWidth);
            Pen p2 = new Pen(Color.FromArgb(road.LineColor), road.LineWidth);            
            p1.StartCap= LineCap.Round;
            p1.EndCap = LineCap.Round;
            p2.StartCap = LineCap.Round;
            p2.EndCap = LineCap.Round;
            p2.DashStyle = DashStyle.Custom;
            p2.DashPattern = new float[] { 16, 6 };
            List<Vector3> ptlist = road.PtList;
            for(int i=0;i<ptlist.Count-1;i++)
            {    
                g.DrawLine(p1, ptlist[i].x, ptlist[i].y, ptlist[i + 1].x, ptlist[i + 1].y);   
            }
            for (int i = 0; i < ptlist.Count - 1; i++)
            {
                g.DrawLine(p2, ptlist[i].x, ptlist[i].y, ptlist[i + 1].x, ptlist[i + 1].y);
            }
             //System.Drawing.Font pfont = new System.Drawing.Font("宋体", 15);
             //g.DrawString("新中路", pfont, Brushes.Red, ptlist[0].x, ptlist[0].y);
            bitmap.Save(texturefilename, System.Drawing.Imaging.ImageFormat.Png);
            bitmap.Dispose();
            g.Dispose();
            p1.Dispose();
            p2.Dispose();
           
            //将纹理贴到底图上
            TextureManager.Singleton.Load(texturefilename, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
            MaterialPtr materialptr = MainOgreForm.Singleton.materialptr;
            materialptr.GetTechnique(0).GetPass(0).CreateTextureUnitState(texturefilename);

            //MainOgreForm.Singleton.strnamelist.Add(texturefilename);
            //materialptr.GetTechnique(0).GetPass(0).DepthWriteEnabled = false;
            //materialptr.GetTechnique(0).GetPass(0).SetSceneBlending(SceneBlendFactor.SBF_SOURCE_ALPHA, SceneBlendFactor.SBF_ONE_MINUS_SOURCE_ALPHA);
        }
        static public void DeleteRoad(RoadStruct road)
        {
            if (MainOgreForm.Singleton.materialptr == null)
                return;
            string texturefilename = Application.StartupPath + "\\TempBillBoard\\" + road.Name + ".png";
            //查看纹理是否存在
            if (File.Exists(texturefilename))
            {
                Pass p = MainOgreForm.Singleton.materialptr.GetTechnique(0).GetPass(0);
                int i = 0;

                foreach (TextureUnitState tus in p.GetTextureUnitStateIterator())
                {
                    if (tus.TextureName == texturefilename)
                    {
                        p.RemoveTextureUnitState((ushort)i);
                        break;
                    }
                    i++;
                }

                TextureManager.Singleton.Remove(texturefilename);
                File.Delete(texturefilename);
            }
        }
    }
}
