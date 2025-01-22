using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DipProject
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
            FormClosing += Options_FormClosing;
            treeView1.NodeMouseClick += treeView1_NodeMouseClick;
            LoadRes();
        }
        public void LoadRes()
        {
            foreach (var s in NGDB.List)
            {
                TreeNode node = new TreeNode(s.RUSNameTable);
                node.Name = s.ENGNameTable;
                node.Checked = NGOptions.List.Contains(s.ENGNameTable);
                treeView2.Nodes.Add(node);
            }
            treeView3.Nodes["save_user"].Checked = NGOptions.isSave;
            treeView3.Nodes["if_format"].Checked = NGOptions.isIFFormat;
        }
        private void Options_FormClosing(object sender, FormClosingEventArgs e)
        {
            NGOptions.List.Clear();
            foreach (TreeNode s in treeView2.Nodes)
                if (s.Checked)
                    NGOptions.List.Push(s.Name);
            NGOptions.isSave = treeView3.Nodes["save_user"].Checked;
            NGOptions.isIFFormat = treeView3.Nodes["if_format"].Checked;
        }
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            switch (e.Node.Name)
            {
                case "general":
                    tabControl1.SelectedTab = tabPage1;
                    break;
                case "tables":
                    tabControl1.SelectedTab = tabPage2;
                    break;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Перезапустите приложение, чтобы изменения вступили в силу!");
            Close();
        }
    }
}
