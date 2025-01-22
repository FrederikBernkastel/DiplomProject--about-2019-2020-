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
    public partial class UTypeDevice : Form
    {
        public IEnumerable<TypeDevice> List;
        public UTypeDevice(IEnumerable<TypeDevice> list)
        {
            InitializeComponent();
            List = list;
            LoadRes();
        }
        public void LoadRes()
        {
            NGDB.List["Типы устройств"].PrintInfo(dataGridView1, List);
        }
        public void Clear()
        {
            textBox1.Text = "";
        }
        public bool isValid()
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Заполните хотя бы одно поле!");
                return false;
            }
            else if (List.Count() > 1 &&
                textBox1.Text != "")
            {
                MessageBox.Show("Множественное изменение невозможно при заполнении всех полей!");
                return false;
            }
            else if (List.Count() < 2 && NGDB.List["Типы устройств"].SelectInfo().Cast<TypeDevice>().Any(u => u.Name == textBox1.Text))
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
                s.Name = textBox1.Text != "" ? textBox1.Text : s.Name;
                NGDB.List["Типы устройств"].UpdateInfo(s.NGToString());
            }
            NGDB.List["Типы устройств"].PrintInfo(dataGridView1, List);
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
