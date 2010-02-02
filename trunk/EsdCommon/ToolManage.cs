using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace EsdCommon
{
    /// <summary>
    /// 工具管理类，方便各个工具的管理，
    /// </summary>
    public class ToolManage
    {
        static ToolManage singleton;
        static public ToolManage Singleton
        {
            get
            {
                return singleton;
            }
        }

        public ToolManage()
        {
            singleton = this;
        }

        /// <summary>
        /// 工具链表
        /// </summary>
        List<ITool> ToolList = new List<ITool>();

        private Type tooltype;
        /// <summary>
        /// 得到当前选择的工具
        /// </summary>
        public ITool CurrentTool
        {
            get
            {
                foreach (ITool tool in ToolList)
                {
                    if (tool.GetType() == ToolType)
                    {
                        return tool;
                    }
                }
                return null;
            }
        }
        public ITool GetTool(Type t)
        {
            foreach (ITool tool in ToolList)
            {
                if (tool.GetType() == t)
                {
                    return tool;
                   
                }
            }
            return null;
        }
        /// <summary>
        /// 当前工具类型
        /// </summary>
        public Type ToolType
        {
            get
            {
                return tooltype;
            }
            set
            {
                tooltype = value;
                foreach (ITool tool in ToolList)
                {
                    if (tool.GetType() == tooltype)
                    {
                        tool.Click();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 增加工具
        /// </summary>
        /// <param name="tool"></param>
        public void AddTool(ITool tool)
        {
            ToolList.Add(tool);
        }

        public void InitTool()
        {
            foreach (ITool tool in ToolList)
            {
                tool.Init();
            }
        }

        public void MouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach (ITool tool in ToolList)
            {
                if (tool.GetType() == ToolType)
                {
                    tool.MouseDown(sender, e);
                }
            }
        }
        public void MouseMove(object sender, MouseEventArgs e)
        {
            foreach (ITool tool in ToolList)
            {
                if (tool.GetType() == ToolType)
                {
                    tool.MouseMove(sender, e);
                }
            }
        }

        public void MouseUp(object sender, MouseButtonEventArgs e)
        {
            foreach (ITool tool in ToolList)
            {
                if (tool.GetType() == ToolType)
                {
                    tool.MouseUp(sender, e);
                }
            }
        }

        public void KeyDown(object sender, KeyEventArgs e)
        {
            foreach (ITool tool in ToolList)
            {
                if (tool.GetType() == ToolType)
                {
                    tool.KeyDown(sender, e);
                }
            }
        }
        public void KeyUp(object sender, KeyEventArgs e)
        {
            foreach (ITool tool in ToolList)
            {
                if (tool.GetType() == ToolType)
                {
                    tool.KeyUp(sender, e);
                }
            }
        }
        public void TimerTick(object sender, EventArgs e)
        {
            foreach (ITool tool in ToolList)
            {
                if (tool.GetType() == ToolType)
                {
                    tool.TimerTick(sender, e);
                }
            }
        }
    }
}
