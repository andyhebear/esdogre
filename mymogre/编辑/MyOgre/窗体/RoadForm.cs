using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MyOgre
{
    public partial class RoadForm : Form
    {
        public RoadForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                RoadStruct road = MainOgreForm.Singleton.ModelDataManage.modelEntry.路[listBox1.SelectedIndex];
                AddRoadTool.DeleteRoad(road);
                MainOgreForm.Singleton.ModelDataManage.modelEntry.路.RemoveAt(listBox1.SelectedIndex);
                InitListBox();
            }
        }

        private void RoadForm_Load(object sender, EventArgs e)
        {
            InitListBox();
        }
        private void InitListBox()
        {
            listBox1.Items.Clear();
            foreach (RoadStruct road in MainOgreForm.Singleton.ModelDataManage.modelEntry.路)
            {
                listBox1.Items.Add(road.RoadName);
            }
        }
    }
}
