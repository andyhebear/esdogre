using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EsdCommon;
using System.Windows.Media;
using System.Windows.Input;
using OgreLib;
using System.Windows;

namespace Esd.Tool
{
    //增加注记，采用广告牌来做
    public class AddBillBoardTool : AbstractTool
    {
        //private System.Drawing.Font selectfont = new System.Drawing.Font("宋体", 15);
        private Color selectcolor = Colors.Black;
        OgreImage ogreimage = null;
        EsdSceneManager esmanager = null;
        public AddBillBoardTool()
        {
            ogreimage = EsdSceneManager.Singleton.OgreImage;
            esmanager = EsdSceneManager.Singleton;
        }
        public override void MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
        public override void MouseUp(object sender, MouseButtonEventArgs e)
        {
            /*if (!esmanager.IsStarEdit)
            {
                MessageBox.Show("请新建一个场景");
                return;
            }

            AddbillBoard dlg = new AddbillBoard();
            dlg.selectcolor = selectcolor;
            dlg.selectfont = selectfont;
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                selectfont = dlg.selectfont;
                selectcolor = dlg.selectcolor;

                Graphics g = MainOgreForm.Singleton.CreateGraphics();
                SizeF size = g.MeasureString(dlg.text, selectfont);
                g.Dispose();

                System.Drawing.Image billboard = new Bitmap((int)size.Width, (int)size.Height);

                g = Graphics.FromImage(billboard);

                SolidBrush brush = new SolidBrush(selectcolor);

                g.DrawString(dlg.text, selectfont, brush, new PointF());

                string texturefilename = Application.StartupPath + "\\TempBillBoard\\" + MainOgreForm.GetDateName() + ".png";

                billboard.Save(texturefilename, System.Drawing.Imaging.ImageFormat.Png);

                billboard.Dispose();
                g.Dispose();
                brush.Dispose();

                TextureManager.Singleton.Load(texturefilename, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);

                MaterialPtr materialptr = MaterialManager.Singleton.Load(texturefilename, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
                materialptr.GetTechnique(0).GetPass(0).CreateTextureUnitState(texturefilename);

                materialptr.GetTechnique(0).GetPass(0).DepthWriteEnabled = false;

                materialptr.GetTechnique(0).GetPass(0).SetSceneBlending(SceneBlendFactor.SBF_SOURCE_ALPHA, SceneBlendFactor.SBF_ONE_MINUS_SOURCE_ALPHA);

                string bstname = MainOgreForm.GetDateName();

                //创建广告牌集合
                BillboardSet bbs = OgreView.Singleton.sceneMgr.CreateBillboardSet(bstname);
                bbs.MaterialName = texturefilename;

                Vector3 v1 = OgreView.Singleton.ScreenPtToSpaceVector(e.Location);
                v1.z = 2;
                Billboard bill = bbs.CreateBillboard(new Vector3());

                SceneNode node = OgreView.Singleton.billNode.CreateChildSceneNode();

                node.AttachObject(bbs);

                node.Scale(0.1f, 0.1f, 0.1f);
                node.Position = v1;

                BillBoardstruct bsb = new BillBoardstruct();
                bsb.实体名 = bstname;
                bsb.位置 = v1;
                bsb.注记名 = dlg.text;
                bsb.字体名 = selectfont.FontFamily.Name;
                bsb.字体大小 = selectfont.Size;
                bsb.斜体 = selectfont.Italic;
                bsb.下划线 = selectfont.Underline;
                bsb.粗 = selectfont.Bold;
                bsb.颜色 = selectcolor.ToArgb();
                MainOgreForm.Singleton.ModelDataManage.modelEntry.广告牌.Add(bsb);

                //设置当前工具为漫游
                MainOgreForm.Singleton.ToolManageObject.ToolType = typeof(SelectBillBoardTool);
            }*/
        }
    }
}
