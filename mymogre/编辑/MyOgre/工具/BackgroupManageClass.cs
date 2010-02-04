using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MyOgre
{
    public class BackgroupManageClass : AbstractMenuTool
    {
        public BackgroupManageClass(ToolStripItem control)
            : base(control)
        {
            
        }
        public override void Click(object sender, EventArgs e)
        {
            BackgroupForm dlg = new BackgroupForm();
            dlg.ShowDialog();
        }
    }
}
