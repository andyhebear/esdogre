using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MyOgre
{
    //工具接口
    public interface ITool
    {
        void Click();
        void MouseDown(object sender, MouseEventArgs e);
        void MouseMove(object sender, MouseEventArgs e);
        void MouseUp(object sender, MouseEventArgs e);
        void KeyDown(object sender, KeyEventArgs e);
        void TimerTick(object sender, EventArgs e);
        void Init();
    }
    /// <summary>
    /// 工具抽像类，方便新建工具，
    /// </summary>
    public abstract class AbstractTool : ITool
    {

        #region ITool 成员

        virtual public void Click()
        {

        }
        virtual public void MouseDown(object sender, MouseEventArgs e)
        {
        }

        virtual public void MouseMove(object sender, MouseEventArgs e)
        {
        }

        virtual public void MouseUp(object sender, MouseEventArgs e)
        {
        }

        virtual public void KeyDown(object sender, KeyEventArgs e)
        {
        }

        virtual public void TimerTick(object sender, EventArgs e)
        {
        }
        virtual public void Init()
        {

        }
        #endregion
    }
    //抽象菜单工具，方便程序管理，
    public abstract class AbstractMenuTool
    {
        public AbstractMenuTool(ToolStripItem control)
        {
            if (control == null)
                return;

            control.Click += new EventHandler(Click);
        }
       
        virtual public void Click(object sender, EventArgs e)
        {
            
        }
    }
}
