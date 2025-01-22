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
    public partial class FilterOther : Form
    {
        public BaseTable Table;
        public FilterOther(BaseTable table)
        {
            InitializeComponent();
            Table = table;
            dataGridView1.CellClick += dataGridView1_CellClick;
            LoadRes();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                (dataGridView1.Rows[e.RowIndex].Cells[3] as DataGridViewComboBoxCell).Items.Clear();
                (dataGridView1.Rows[e.RowIndex].Cells[3] as DataGridViewComboBoxCell).Items.AddRange(NGDB.List[dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString()]
                    .RUSPrintField.Skip(1).Cast<object>().ToArray());
                dataGridView1.Rows[e.RowIndex].Cells[3].Value = NGDB.List[dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString()].RUSPrintField.Last();
            }
        }
        public void LoadRes()
        {
            var array1 = new object[] 
            {
                "И",
                "ИЛИ",
            };
            var array2 = new object[]
            {
                "Равно",
                "Не равно",
                "Больше",
                "Меньше",
                "Содержит",
                "Не содержит",
            };
            for (int i = 0; i < 4; i++)
                dataGridView1.Rows.Add();
            for (int i = 0; i < 4; i++)
                (dataGridView1.Rows[i].Cells[1] as DataGridViewComboBoxCell).Items.AddRange(array1);
            for (int i = 0; i < 4; i++)
                (dataGridView1.Rows[i].Cells[2] as DataGridViewComboBoxCell).Items.AddRange(Table.WebTables.Select(u => u.RUSNameTable).Cast<object>().ToArray());
            for (int i = 0; i < 4; i++)
                (dataGridView1.Rows[i].Cells[3] as DataGridViewComboBoxCell).Items.AddRange(Table.WebTables.First().RUSPrintField.Skip(1).Cast<object>().ToArray());
            for (int i = 0; i < 4; i++)
                (dataGridView1.Rows[i].Cells[4] as DataGridViewComboBoxCell).Items.AddRange(array2);
            for (int i = 0; i < 4; i++)
                dataGridView1.Rows[i].Cells[0].Value = false;
            for (int i = 0; i < 4; i++)
                dataGridView1.Rows[i].Cells[1].Value = array1[0];
            for (int i = 0; i < 4; i++)
                dataGridView1.Rows[i].Cells[2].Value = Table.WebTables.First().RUSNameTable;
            for (int i = 0; i < 4; i++)
                dataGridView1.Rows[i].Cells[3].Value = Table.WebTables.First().RUSPrintField.Last();
            for (int i = 0; i < 4; i++)
                dataGridView1.Rows[i].Cells[4].Value = array2[0];
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var rty = dataGridView1.Rows.Cast<DataGridViewRow>().Where(u => Convert.ToBoolean(u.Cells[0].Value));
            if (!isValid(rty))
                return;
            var hjkv = rty.Select(u => new string[]
            {
                u.Cells[2].Value.ToString(),
                u.Cells[3].Value.ToString(),
                u.Cells[4].Value.ToString(),
                u.Cells[5].Value.ToString(),
            });
            var yhn = Enumerable.Empty<string>();
            if (rty.Count() != 1)
                yhn = rty.Skip(1).Select(u => u.Cells[1].Value.ToString());
            NGOptions.StackObjects.Push(yhn.ToArray());
            NGOptions.StackObjects.Push(hjkv.ToArray());
            DialogResult = DialogResult.Yes;
            Close();
        }
        public bool isValid(IEnumerable<DataGridViewRow> obj)
        {
            if (obj.Count() == 0)
            {
                MessageBox.Show("Необходимо включить хотя бы одно условие!");
                return false;
            }
            else if (obj.Any(u => u.Cells[5].Value == null))
            {
                MessageBox.Show("Необходимо заполнить значения всех включенных условий!");
                return false;
            }
            else
                return true;
        }
    }
}
