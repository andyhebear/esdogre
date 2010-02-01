using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Mogre;

namespace Esd.Tool
{
    /// <summary>
    /// 新建场景工具
    /// </summary>
    public class NewSceneTool
    {
        /// <summary>
        /// 新建场景
        /// </summary>
        public void Click()
        {
            //选择对话框
            NewSceneWindow win = new NewSceneWindow();
            var result = win.ShowDialog();
            if (result.HasValue && result.Value)
            {
                int width,height;
                if (int.TryParse(win.textBox1.Text, out width) && int.TryParse(win.textBox2.Text, out height))
                {
                    string filename=System.IO.Path.GetFileName(((ListViewItem)win.listView1.SelectedItem).Content.ToString());
                    //创建场景
                    CreateScene(width, height, filename);
                }
            }
            return;
        }
        //创建场景
        private void CreateScene(int width, int height, string filename)
        {
            string planename = Guid.NewGuid().ToString("N");
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
            EsdSceneManager.Singleton.MaterialPtr = MaterialManager.Singleton.GetByName(filename);
            Entity ent = EsdSceneManager.Singleton.SceneManager.CreateEntity("floor", planename);
            EsdSceneManager.Singleton.FloorNode = EsdSceneManager.Singleton.SceneManager.RootSceneNode.CreateChildSceneNode();
            EsdSceneManager.Singleton.FloorNode.AttachObject(ent);
            //设计材质
            ent.SetMaterialName(filename);
            //保存当前场景状态。
            EsdSceneManager.Singleton.ModelDataManage.modelEntry.场景地面图片 = filename;
            EsdSceneManager.Singleton.ModelDataManage.modelEntry.场景宽 = width;
            EsdSceneManager.Singleton.ModelDataManage.modelEntry.场景高 = height;
            EsdSceneManager.Singleton.ModelDataManage.modelEntry.模型名 = planename;

            //开始编辑标志
            EsdSceneManager.Singleton.IsStarEdit = true;
        }
    }
}
