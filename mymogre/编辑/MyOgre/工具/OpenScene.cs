using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using Mogre;

namespace MyOgre
{
    //打开场景功能
    public class OpenScene : AbstractMenuTool
    {
        public OpenScene(ToolStripItem control)
            : base(control)
        {
            
        }
        //菜单单击事件，
        public override void Click(object sender, EventArgs e)
        {
            //打开文件对话框
            OpenFileDialog dlg = new OpenFileDialog();
            //此属性是用来当打开文件后，当前程序操作的路径不会改为打开文件的路径，如果不设，当打开文件后，
            //ogre就会找不到相应的资源，因为它载入资源时使用的是相对路径 
            dlg.RestoreDirectory = true;

            dlg.Filter = "模型文件|*.xml|所有文件|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                //得到当前打开的文件名，方便保存和其它操作
                MainOgreForm.Singleton.OpenFileName = dlg.FileName;
                try
                {
                    //打开场景文件， 通过XML的返序列化读取文件，
                    XmlSerializer serializer = new XmlSerializer(typeof(ModeEntryMain));
                    FileStream file = File.OpenRead(dlg.FileName);
                    MainOgreForm.Singleton.ModelDataManage.modelEntry = (ModeEntryMain)serializer.Deserialize(file);
                    file.Close();


                    //载入场景的地面，
                    string filename = MainOgreForm.Singleton.ModelDataManage.modelEntry.场景地面图片;
                    int width = MainOgreForm.Singleton.ModelDataManage.modelEntry.场景宽;
                    int height = MainOgreForm.Singleton.ModelDataManage.modelEntry.场景高;
                    string planename = MainOgreForm.Singleton.ModelDataManage.modelEntry.模型名;
                    // 以下同新建场景
                    Plane p;
                    p.normal = Vector3.UNIT_Z;
                    p.d = 0;

                    if (!MeshManager.Singleton.ResourceExists(planename))
                    {
                        MeshManager.Singleton.CreatePlane(planename, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, p, width, height, 20, 20, true, 1, 1F, 1F, Vector3.UNIT_Y);

                        if (!TextureManager.Singleton.ResourceExists(filename))
                        {
                            //载入纹理
                            TexturePtr textureptr = TextureManager.Singleton.Load(filename, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
                            //创建材质
                            MaterialPtr materialptr = MaterialManager.Singleton.Load(filename, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
                            TextureUnitState state = materialptr.GetTechnique(0).GetPass(0).CreateTextureUnitState(filename);
                            //设置纹理的寻址模式，如果不设置。有可能在结边的时候出现缝隙
                            state.SetTextureAddressingMode(TextureUnitState.TextureAddressingMode.TAM_MIRROR);

                            materialptr.GetTechnique(0).GetPass(0).DepthWriteEnabled = false;
                            materialptr.GetTechnique(0).GetPass(0).DepthFunction = CompareFunction.CMPF_LESS_EQUAL;
                            
                        }
                    }
                   
                        MainOgreForm.Singleton.materialptr = MaterialManager.Singleton.GetByName(filename);
                    

                    OgreView.Singleton.CreateWindows((uint)MainOgreForm.Singleton.panel1.Width, (uint)MainOgreForm.Singleton.panel1.Height);

                    //开始编辑
                    MainOgreForm.Singleton.IsStarEdit = true;

                    foreach (ModelEntryStruct en in MainOgreForm.Singleton.ModelDataManage.modelEntry.模型链表)
                    {
                        MainOgreForm.Singleton.AddNodeTree(en);
                    }
                   
                }
                catch (IOException ee)
                {
                    MessageBox.Show(ee.Message);
                  
                }
            }
        }
    }
}
