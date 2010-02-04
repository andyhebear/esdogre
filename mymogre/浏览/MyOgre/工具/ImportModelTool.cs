using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Mogre;

namespace MyOgre
{
    /// <summary>
    /// 导入模型工具
    /// </summary>
    public class ImportModelTool : AbstractMenuTool
    {
        List<string> materialNamelist = new List<string>();
        public ImportModelTool(ToolStripItem control)
            : base(control)
        {
            
        }
        public override void Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string[] files = Directory.GetFiles(dlg.SelectedPath);

                string apppath = Application.StartupPath + "\\Media\\Model";

                ImportModelForm importdlg = new ImportModelForm();
                if (importdlg.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }

                foreach (string filename in files)
                {
                    if (filename.Contains(".mesh"))
                    {
                        string name = Path.GetFileName(filename);
                        MainOgreForm.Singleton.ModelGroup.AddModel(importdlg.textBox1.Text, name, importdlg.comboBox1.Text);
                        break;
                    }
                }

              
                string sfilename="";
                foreach (string filename in files)
                {
                    string name = Path.GetFileName(filename);
                    sfilename = filename;
                    if (filename.Contains(".material"))
                    {
                        if (!MaterialIsExists(sfilename))
                            return;
                        else
                        {
                            sfilename = Application.StartupPath + "\\temp.temp";                            
                        }
                    }
                    
                   
                    if (MainOgreForm.Singleton.IsTrueModelFile(name))
                    {
                        DialogResult result=MessageBox.Show("文件" + name + "在库中已经存在，选择“是”覆盖模型、“否”忽略当前文件、“取消”取消模型导入", "导入文件", MessageBoxButtons.YesNoCancel);

                        if (result == DialogResult.No)
                        {
                            continue;
                        }
                        else if (result == DialogResult.Yes)
                        {
                            name = apppath + "\\" + name;
                            File.Copy(sfilename, name, true);
                            continue;
                        }
                        else if (result== DialogResult.Cancel)
                        {
                            MessageBox.Show("导主模型失败");
                            //退出导入模型
                            return;
                        }
                    }

                    name = apppath + "\\" + name;
                    File.Copy(sfilename, name,true);
                }
                MessageBox.Show("文件导入成功！");
            }
        }
        private bool MaterialIsExists(string filename)
        {

            string p = Application.StartupPath;
            FileStream tempstream = new FileStream(p + "\\temp.temp", FileMode.Create);
            StreamWriter writer = new StreamWriter(tempstream);

            FileStream filestream = new FileStream(filename, FileMode.Open);
            StreamReader reader = new StreamReader(filestream,Encoding.Default);


            List<string> templist = new List<string>();

            bool existflag = true;
            bool flag = true;
            string strline = "";
            while((strline=reader.ReadLine())!=null)
            {
                if (strline.Contains("material"))
                {
                    string[] words = strline.Split(' ');
                    string materialname = words[1].Trim();
                    if (MaterialManager.Singleton.ResourceExists(materialname) || MaterialExists(materialname))
                    {
                        string name = Path.GetFileName(filename);
                        if (MessageBox.Show("材质" + materialname + "已经存在(所在文件：" + name + ")，“是”覆盖材质，“否”取消模型导入。", "导入文件", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            existflag = false;
                        }
                        else
                        {
                            MessageBox.Show("导主模型失败");
                            flag = false;
                            break;
                        }
                    }
                    else
                    {
                        existflag = true;
                        writer.WriteLine(strline);
                        materialNamelist.Add(materialname);
                    }
                }
                else
                {
                    if (existflag)
                    {
                        writer.WriteLine(strline);
                    }                    
                }               
            }
            reader.Close();
            filestream.Close();

            writer.Close();
            tempstream.Close();

            return flag;
        }
        private bool MaterialExists(string materialname)
        {
            foreach (string name in materialNamelist)
            {
                if (name.Equals(materialname))
                    return true;          
            }
            return false;
        }

    }
}
