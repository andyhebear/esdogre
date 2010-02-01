using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace MyOgre
{
    //保存场景景景
    public class SaveScene : AbstractMenuTool
    {
        public SaveScene(ToolStripItem control)
            : base(control)
        {

        }
        //同新建
        public override void Click(object sender, EventArgs e)
        {
            //如果当前不是打开的文件，则执行另存为操作，如果是执行保存
            if (MainOgreForm.Singleton.OpenFileName == "")
            {
                //另存为操作
                SaveAsScene saveas = new SaveAsScene(null);
                saveas.Click(null, null);
                MainOgreForm.Singleton.OpenFileName = saveas.openfilename;
            }
            else
            {
                //执行保存操作，将场景序列化，可参见。net的序列化
                XmlSerializer serializer = new XmlSerializer(typeof(ModeEntryMain));
                FileStream stream = new FileStream(MainOgreForm.Singleton.OpenFileName, FileMode.Create);
                StreamWriter writer = new StreamWriter(stream, Encoding.GetEncoding(936));
                serializer.Serialize(writer, MainOgreForm.Singleton.ModelDataManage.modelEntry);
                writer.Close();
                stream.Close();
            }
        }
    }
    //另存为
    public class SaveAsScene : AbstractMenuTool
    {
        public string openfilename = "";
        public SaveAsScene(ToolStripItem control)
            : base(control)
        {

        }

        public override void Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.RestoreDirectory = true;
            dlg.Filter = "场景文件|*.xml|所有文件|*.*";
            try
            {
                //保存对话框
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    //序列化保存文件
                    XmlSerializer serializer = new XmlSerializer(typeof(ModeEntryMain));
                    FileStream stream = new FileStream(dlg.FileName, FileMode.Create);
                    StreamWriter writer = new StreamWriter(stream, Encoding.GetEncoding(936));
                    serializer.Serialize(writer, MainOgreForm.Singleton.ModelDataManage.modelEntry);
                    writer.Close();
                    stream.Close();
                    openfilename = dlg.FileName;

                }
            }
            catch (IOException ee)
            {
                MessageBox.Show(ee.Message);
            }
        }
    }
}
