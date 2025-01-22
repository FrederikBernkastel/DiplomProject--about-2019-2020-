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
    public partial class UEmployee : Form
    {
        public IEnumerable<Employee> List;
        public UEmployee(IEnumerable<Employee> list)
        {
            InitializeComponent();
            List = list;
            LoadRes();
        }
        public void LoadRes()
        {
            NGDB.List["Сотрудники"].PrintInfo(dataGridView1, List);
        }
        public void Clear()
        {
            textBox2.Text = "";
            textBox1.Text = "";
        }
        public bool isValid()
        {
            if (textBox1.Text == "" &&
                textBox2.Text == "")
            {
                MessageBox.Show("Заполните хотя бы одно поле!");
                return false;
            }
            else if (List.Count() > 1 &&
                textBox1.Text != "" &&
                textBox2.Text != "")
            {
                MessageBox.Show("Множественное изменение невозможно при заполнении всех полей!");
                return false;
            }
            else if (List.Count() < 2 && NGDB.List["Сотрудники"].SelectInfo().Cast<Employee>().Any(u =>
                u.Name == textBox2.Text &&
                u.Position == textBox1.Text))
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
                s.Name = textBox2.Text != "" ? textBox2.Text : s.Name;
                s.Position = textBox1.Text != "" ? textBox1.Text : s.Position;
                NGDB.List["Сотрудники"].UpdateInfo(s.NGToString());
            }
            NGDB.List["Сотрудники"].PrintInfo(dataGridView1, List);
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
