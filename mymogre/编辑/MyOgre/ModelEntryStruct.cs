using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using System.Drawing;
using System.Runtime.Serialization;

namespace MyOgre
{
    /// <summary>
    /// 模型结构，
    /// </summary>
    public class ModelEntryStruct
    {
        public string 模型名;
        public string 实体名;
        public string 材质;
        public Vector3 位置;
        public Vector3 缩放比例;
        public Vector3 旋转角度;
        public string 备注属性;
        public string 名称;
        public string 图片名称;
        public byte[] 图片;
        public string 视频名称;
        public byte[] 视频;
        /// <summary>
        /// 室内场景名称
        /// </summary>
        public string BspName;
    }
    /// <summary>
    /// 注记结构，即广告牌
    /// </summary>
    public class BillBoardstruct
    {
        public string 实体名;
        public string 注记名;
        public Vector3 位置;
        public string 字体名;
        public float 字体大小;
        public bool 粗;
        public bool 下划线;
        public bool 斜体;
        public int 颜色;
    }
    /// <summary>
    /// 水和草数据
    /// </summary>
    public class WaterGrass
    {
        public string Name;
        public List<Vector3>PtList=new List<Vector3>();
        public string MaterialName;
    }
    /// <summary>
    /// 路的数据结构
    /// </summary>
    public class RoadStruct 
    {
        public List<Vector3> PtList = new List<Vector3>();
        public string Name;
        public string RoadName;
        public int RoadWidth;
        public int LineWidth;
        public int RoadColor;
        public int LineColor;      

    }
    /// <summary>
    /// 模型管理保存对象，用来将编辑过的场景序列化成xml文件或反序列化场景对象。
    /// </summary>
    public class ModeEntryMain
    {
        public List<ModelEntryStruct> 模型链表 = new List<ModelEntryStruct>();
        public List<BillBoardstruct> 广告牌 = new List<BillBoardstruct>();
        public List<WaterGrass>水和草=new List<WaterGrass>();
        public List<RoadStruct> 路 = new List<RoadStruct>();
        public int 场景宽;
        public int 场景高;
        public string 场景地面图片;
        public string 模型名;

        public ModelEntryStruct GetModelEntry(SceneNode node)
        {
             Entity en = node.GetAttachedObject(0) as Entity;
             if (en != null)
             {
                 foreach (ModelEntryStruct men in 模型链表)
                 {
                     if (men.实体名 == en.Name)
                     {
                         return men;
                     }
                 }
             }
             return null;
        }
        public void Clear()
        {
            模型链表.Clear();
            广告牌.Clear();
            水和草.Clear();
            路.Clear();
            模型名 = "";
            场景地面图片 = "";
        }
    }
    /// <summary>
    /// 模型管理
    /// </summary>
    public class ModelManage
    {
        public List<ModelStruct>植物=new List<ModelStruct>();
        public List<ModelStruct> 建筑物= new List<ModelStruct>();
        public List<ModelStruct>室内元素=new List<ModelStruct>();
        public List<ModelStruct> 杂项 = new List<ModelStruct>();
        /// <summary>
        /// 增加模型
        /// </summary>
        /// <param name="name"></param>
        /// <param name="modelname"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public void AddModel(string name, string modelname, string group,float scale)
        {
            switch (group)
            {
                case "植物":
                    {
                        ModelStruct temp = new ModelStruct();
                        temp.Name = name;
                        temp.ModelName = modelname;
                        temp.Scale = scale;
                        植物.Add(temp);
                    }
                    break;
                case "室内元素":
                    {
                        ModelStruct temp = new ModelStruct();
                        temp.Name = name;
                        temp.ModelName = modelname;
                        temp.Scale = scale;
                        室内元素.Add(temp);
                    }
                    break;
                case "建筑物":
                    {
                        ModelStruct temp = new ModelStruct();
                        temp.Name = name;
                        temp.ModelName = modelname;
                        temp.Scale = scale;
                        建筑物.Add(temp);
                    }
                    break;
                default:
                    {
                        ModelStruct temp = new ModelStruct();
                        temp.Name = name;
                        temp.ModelName = modelname;
                        temp.Scale = scale;
                        杂项.Add(temp);
                    }
                    break;
            }
        }
        /// <summary>
        /// 根据模型名和分组名的到模型文件名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public ModelStruct GetMeshName(string name, string group)
        {
            List<ModelStruct> temp = null;
            switch (group)
            {
                case "植物":
                    {
                        temp = 植物;
                    }
                    break;
                case "室内元素":
                    {
                        temp = 室内元素;
                    }
                    break;
                case "建筑物":
                    {
                        temp = 建筑物;
                    }
                    break;
                default:
                    {
                        temp = 杂项;
                    }
                    break;
            }
            if (temp == null)
                return null;
            foreach (ModelStruct model in temp)
            {
                if (model.Name == name)
                {
                    return model;
                }
            }
            return null;
        }
        public bool DeleteMesh(string name, string group)
        {

            List<ModelStruct> temp = null;
            switch (group)
            {
                case "植物":
                    {
                        temp = 植物;
                    }
                    break;
                case "室内元素":
                    {
                        temp = 室内元素;
                    }
                    break;
                case "建筑物":
                    {
                        temp = 建筑物;
                    }
                    break;
                default:
                    {
                        temp = 杂项;
                    }
                    break;
            }
            if (temp == null)
                return false;
            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Name == name)
                {
                    temp.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

    }
    public class ModelStruct
    {
        /// <summary>
        /// 缩放比例
        /// </summary>
        public float Scale = 1;
        /// <summary>
        /// 模型的名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 对应的模型文件名
        /// </summary>
        public string ModelName;
    }
}
