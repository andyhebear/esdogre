using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MyOgre
{
    public class DeleteModelClass : AbstractMenuTool
    {
        public DeleteModelClass(ToolStripItem control)
            : base(control)
        {
            
        }
        public override void Click(object sender, EventArgs e)
        {
            DeleteModelForm dlg = new DeleteModelForm();
            dlg.ShowDialog();
        }
    }
}
