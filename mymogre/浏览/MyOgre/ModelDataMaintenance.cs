using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;

namespace MyOgre
{
    /// <summary>
    /// 模型管理类
    /// </summary>
    public class ModelDataMaintenance
    {
        //加入的模型链表
        public ModeEntryMain modelEntry = new ModeEntryMain();
        
        /// <summary>
        /// 将当前结点状态与之同步
        /// operateflag说明
        /// 1增加结点 
        /// 2删除结点
        /// 3更新位置和缩放，旋转
        /// </summary>
        /// <param name="node"></param>
        /// <param name="operateflat"></param>
        public void AddRemoveState(SceneNode node, int operateflag)
        {
            switch (operateflag)
            {
                case 1://增加操作
                    {
                        ModelEntryStruct modelentry = new ModelEntryStruct();
                   

                        Entity en = node.GetAttachedObject(0) as Entity;
                        if (en != null)
                        {
                            //得到模型的相关信息，
                            modelentry.模型名 = en.GetMesh().Name;
                            modelentry.材质 = "";
                            modelentry.实体名 = en.Name;
                            modelentry.缩放比例 = node.GetScale();
                            modelentry.位置 = node.Position;
                            modelentry.旋转角度 = new Vector3();
                            modelEntry.模型链表.Add(modelentry);
                            modelentry.名称 = MainOgreForm.GetDateName();

                            //更新树节点
                            MainOgreForm.Singleton.AddNodeTree(modelentry);
                        }
                    }
                    break;
                case 2://删除
                    {
                        Entity en = node.GetAttachedObject(0) as Entity;
                        if (en == null)
                            return;
                        for (int i = 0; i < modelEntry.模型链表.Count; i++)
                        {
                            if (modelEntry.模型链表[i].实体名 == en.Name)
                            {
                                MainOgreForm.Singleton.RemoveNodeTree(modelEntry.模型链表[i]);     
                                modelEntry.模型链表.RemoveAt(i);                                                           
                                break;
                            }
                        }
                    }
                    break; 
                default:
                    break;
            }
            
        }
        /// <summary>
        /// 更新模型位置大小，选择状态
        /// </summary>
        /// <param name="node">所需更新的节点</param>
        /// <param name="q">节点饶那个轴旋转旋转，只能更新一个角度的旋转，如“x,y,z”</param>
        /// <param name="angle">旋转的角度</param>
        public void UpdateModelState(SceneNode node,char q,float angle)
        {
            Entity en = node.GetAttachedObject(0) as Entity;
            for (int i = 0; i < modelEntry.模型链表.Count; i++)
            {
                if (modelEntry.模型链表[i].实体名 == en.Name)
                {
                    ModelEntryStruct modelentry = modelEntry.模型链表[i];
                    modelentry.缩放比例 = node.GetScale();
                    modelentry.位置 = node.Position;
                    //更新旋转角度
                    if (q.Equals('x'))
                    {
                        modelentry.旋转角度.x = angle;
                    }
                    else if (q.Equals('y'))
                    {
                        modelentry.旋转角度.y = angle;
                    }
                    else if (q.Equals('z'))
                    {
                        modelentry.旋转角度.z = angle;
                    }
                    break;
                }
            }
        }
        public void UpdateBillState(SceneNode node)
        {
            BillboardSet en = node.GetAttachedObject(0) as BillboardSet;
            for (int i = 0; i < modelEntry.广告牌.Count; i++)
            {
                if (modelEntry.广告牌[i].实体名 == en.Name)
                {
                    BillBoardstruct modelentry = modelEntry.广告牌[i];
                    modelentry.位置 = node.Position;
                   
                    break;
                }
            }
        }
        public void RemoveBill(SceneNode node)
        {
            BillboardSet en = node.GetAttachedObject(0) as BillboardSet;
            if (en == null)
                return;
            for (int i = 0; i < modelEntry.广告牌.Count; i++)
            {
                if (modelEntry.广告牌[i].实体名 == en.Name)
                {                    
                    modelEntry.广告牌.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
