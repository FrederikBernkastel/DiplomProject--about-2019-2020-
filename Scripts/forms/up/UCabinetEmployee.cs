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
    public partial class UCabinetEmployee : Form
    {
        public IEnumerable<CabinetEmployee> List;
        public int Index = 0;
        public UCabinetEmployee(IEnumerable<CabinetEmployee> list)
        {
            InitializeComponent();
            List = list;
            LoadRes();
        }
        public void LoadRes()
        {
            NGDB.List["Кабинеты"].PrintInfo(dataGridView1, List);
        }
        public void Clear()
        {
            Index = 0;
            toolTip1.SetToolTip(button4, "");
            button4.BackColor = Color.LightGray;
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (new FormOther(NGDB.List["Сотрудники"]).ShowDialog() == DialogResult.Cancel)
                return;
            toolTip1.SetToolTip(button4, NGOptions.StackObjects.Pop().ToString());
            Index = Convert.ToInt32(NGOptions.StackObjects.Pop());
            button4.BackColor = Color.Yellow;
        }
        public bool isValid()
        {
            if (numericUpDown1.Value == 0 &&
                numericUpDown2.Value == 0 &&
                Index == 0)
            {
                MessageBox.Show("Заполните хотя бы одно поле!");
                return false;
            }
            else if (List.Count() > 1 &&
                numericUpDown1.Value != 0 &&
                numericUpDown2.Value != 0 &&
                Index != 0)
            {
                MessageBox.Show("Множественное изменение невозможно при заполнении всех полей!");
                return false;
            }
            else if (List.Count() < 2 && NGDB.List["Кабинеты"].SelectInfo().Cast<CabinetEmployee>().Any(u =>
                u.Cabinet == numericUpDown2.Value &&
                u.Campus == numericUpDown1.Value))
            {
                MessageBox.Show("Данный кабинет уже существует!");
                return false;
            }
            else if (List.Count() < 2 && NGDB.List["Кабинеты"].SelectInfo().Cast<CabinetEmployee>().Any(u =>
                u.Cabinet == numericUpDown2.Value &&
                u.Campus == numericUpDown1.Value &&
                u.Employee != null ? u.Employee.ID == Index : false))
            {
                MessageBox.Show("Такая запись уже существует!");
                return false;
            }
            else
                return true;
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!isValid())
                return;
            if (MessageBox.Show("Вы точно желаете изменить выбранные записи?", "Сообщение", 
                MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            foreach (var s in List)
            {
                s.Cabinet = numericUpDown2.Value != 0 ? (int)numericUpDown2.Value : s.Cabinet;
                s.Campus = numericUpDown1.Value != 0 ? (int)numericUpDown1.Value : s.Campus;
                s.Employee = Index != 0 ? NGDB.List["Сотрудники"].SelectInfo().First(u => u.ID == Index) as Employee : s.Employee;
                NGDB.List["Кабинеты"].UpdateInfo(s.NGToString());
            }
            NGDB.List["Кабинеты"].PrintInfo(dataGridView1, List);
            MessageBox.Show("Выбранные записи успешно изменены!");
            //Clear();
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
