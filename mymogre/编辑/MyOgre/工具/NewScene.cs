using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Mogre;

namespace MyOgre
{
    //新建场景工具，此工具不是操作工具，
    public class NewScene : AbstractMenuTool
    {
        public NewScene(ToolStripItem control)
            : base(control)
        {

        }
        //菜单单击事件，
        public override void Click(object sender, EventArgs e)
        {
            if (OgreView.Singleton.floorNode != null)
            {
                if (MessageBox.Show("是否关闭当前场景，然后新建场景。", "新建场景", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    MainOgreForm.Singleton.CloseScene();
                }
                else
                {
                    return;
                }
            }

            //新建创建场景窗口，可以指定场景的长和宽，和地表纹理
            NewSceneForm dlg = new NewSceneForm();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                //清空当前场景状态链表，
                MainOgreForm.Singleton.ModelDataManage.modelEntry.模型链表.Clear();
                //将场景清空
                OgreView.Singleton.CreateWindows((uint)MainOgreForm.Singleton.panel1.Width, (uint)MainOgreForm.Singleton.panel1.Height);
                string filename = "";
                if (dlg.blackground_listView.SelectedItems.Count <= 0)
                {
                    //如果当前没有选择地面背景，则默认为第一个，
                    filename = dlg.blackground_listView.Items[0].Text;
                }
                else
                {
                    //得到地面所选的纹理
                    filename = dlg.blackground_listView.SelectedItems[0].Text;
                }

                //得到场景的长和宽
                int width = int.Parse(dlg.width_textBox.Text);
                int height = int.Parse(dlg.height_textBox.Text);
                string planename = MainOgreForm.GetDateName();
                // 地面的法线方向。可以决定地面的朝向，
                Plane p;
                p.normal = Vector3.UNIT_Z;
                p.d = 0;
                //判断地面模型是否已经创建，主要是处理新建场景后，关闭文档，在次打开时，模型已和纹理材质已创建，在次创建就会报错，在此做判断
                if (!MeshManager.Singleton.ResourceExists(planename))
                {
                    //创建场景地面模型，通过指定的长和宽
                    MeshManager.Singleton.CreatePlane(planename, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, p, width, height, 20, 20, true, 1, 1F, 1F, Vector3.UNIT_Y);
                    //同上面
                    if (!TextureManager.Singleton.ResourceExists(filename))
                    {
                        //载入纹理
                        TexturePtr textureptr = TextureManager.Singleton.Load(filename, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
                        //创建材质
                        MaterialPtr materialptr = MaterialManager.Singleton.Load(filename, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
                        TextureUnitState state = materialptr.GetTechnique(0).GetPass(0).CreateTextureUnitState(filename);
                        //设置纹理的寻址模式，如果不设置。有可能在结边的时候出现缝隙
                        state.SetTextureAddressingMode(TextureUnitState.TextureAddressingMode.TAM_MIRROR);
                        //材质相应设置，可参见API文档
                        materialptr.GetTechnique(0).GetPass(0).DepthWriteEnabled = false;
                        materialptr.GetTechnique(0).GetPass(0).DepthFunction = CompareFunction.CMPF_LESS_EQUAL;

                    }

                }

                MainOgreForm.Singleton.materialptr = MaterialManager.Singleton.GetByName(filename);


                // 创建地面实体，
                Entity ent = OgreView.Singleton.sceneMgr.CreateEntity("floor", planename);
                OgreView.Singleton.floorNode = OgreView.Singleton.sceneMgr.RootSceneNode.CreateChildSceneNode();
                OgreView.Singleton.floorNode.AttachObject(ent);
                //设计材质
                ent.SetMaterialName(filename);
                //保存当前场景状态。
                MainOgreForm.Singleton.ModelDataManage.modelEntry.场景地面图片 = filename;
                MainOgreForm.Singleton.ModelDataManage.modelEntry.场景宽 = width;
                MainOgreForm.Singleton.ModelDataManage.modelEntry.场景高 = height;
                MainOgreForm.Singleton.ModelDataManage.modelEntry.模型名 = planename;

                //开始编辑标志
                MainOgreForm.Singleton.IsStarEdit = true;

                //设置当前工具为漫游
                MainOgreForm.Singleton.ToolManageObject.InitTool();
                MainOgreForm.Singleton.ToolManageObject.ToolType = typeof(PanToolClass);
                OgreView.Singleton.RestCamer();
                MainOgreForm.Singleton.ClearTree();
            }
        }


    }
}
