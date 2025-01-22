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
    public partial class FormOther : Form
    {
        public FormOther(BaseTable table)
        {
            InitializeComponent();
            table.PrintInfo(dataGridView1);
            dataGridView1.CellClick += DataGridView1_CellClick;
        }
        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            NGOptions.StackObjects.Push(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            NGOptions.StackObjects.Push(dataGridView1.Rows[e.RowIndex].Cells.Cast<DataGridViewCell>().Skip(1).Select(u => u.Value.ToString()).Aggregate((u, h) => u + "||" + h));
            DialogResult = DialogResult.Yes;
            Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
