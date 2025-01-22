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
    public partial class Filter : Form
    {
        public DataGridView Grid;
        public Filter(DataGridView grid)
        {
            InitializeComponent();
            Grid = grid;
            LoadRes();
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
            var array3 = Grid.Columns.Cast<DataGridViewColumn>().Skip(1).Select(u => u.HeaderText);
            for (int i = 0; i < 4; i++)
                dataGridView1.Rows.Add();
            for (int i = 0; i < 4; i++)
                (dataGridView1.Rows[i].Cells[1] as DataGridViewComboBoxCell).Items.AddRange(array1);
            for (int i = 0; i < 4; i++)
                (dataGridView1.Rows[i].Cells[2] as DataGridViewComboBoxCell).Items.AddRange(array3.Cast<object>().ToArray());
            for (int i = 0; i < 4; i++)
                (dataGridView1.Rows[i].Cells[3] as DataGridViewComboBoxCell).Items.AddRange(array2);
            for (int i = 0; i < 4; i++)
                dataGridView1.Rows[i].Cells[0].Value = false;
            for (int i = 0; i < 4; i++)
                dataGridView1.Rows[i].Cells[1].Value = array1[0];
            for (int i = 0; i < 4; i++)
                dataGridView1.Rows[i].Cells[2].Value = array3.First();
            for (int i = 0; i < 4; i++)
                dataGridView1.Rows[i].Cells[3].Value = array2[0];
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
            else if (obj.Any(u => u.Cells[4].Value == null))
            {
                MessageBox.Show("Необходимо заполнить значения всех включенных условий!");
                return false;
            }
            else
                return true;
        }
    }
}
