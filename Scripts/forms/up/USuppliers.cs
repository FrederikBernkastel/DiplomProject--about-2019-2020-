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
    public partial class USuppliers : Form
    {
        public IEnumerable<Provider> List;
        public string MultiTextNG1 = "";
        public string MultiTextNG2 = "";
        public USuppliers(IEnumerable<Provider> list)
        {
            InitializeComponent();
            List = list;
            LoadRes();
        }
        public void LoadRes()
        {
            NGDB.List["Поставщики"].PrintInfo(dataGridView1, List);
        }
        public void Clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";
            textBox6.Text = "";
            maskedTextBox1.Text = "";
            MultiTextNG1 = "";
            MultiTextNG2 = "";
            toolTip1.SetToolTip(button2, "");
            toolTip2.SetToolTip(button1, "");
            button1.BackColor = Color.LightGray;
            button2.BackColor = Color.LightGray;
        }
        public bool isValid()
        {
            if (MultiTextNG1 == "" &&
                textBox1.Text == "" &&
                textBox4.Text == "" &&
                textBox2.Text == "" &&
                maskedTextBox1.Text == "" &&
                textBox6.Text == "" &&
                MultiTextNG2 == "")
            {
                MessageBox.Show("Заполните хотя бы одно поле!");
                return false;
            }
            else if (List.Count() > 1 &&
                MultiTextNG1 != "" &&
                textBox1.Text != "" &&
                textBox4.Text != "" &&
                textBox2.Text != "" &&
                maskedTextBox1.Text != "" &&
                textBox6.Text != "" &&
                MultiTextNG2 != "")
            {
                MessageBox.Show("Множественное изменение невозможно при заполнении всех полей!");
                return false;
            }
            else if (List.Count() < 2 && NGDB.List["Поставщики"].SelectInfo().Cast<Provider>().Any(u => 
                u.Name == MultiTextNG1 &&
                u.FullName == textBox1.Text &&
                u.CheckingAccount == textBox4.Text &&
                u.INN == textBox2.Text &&
                u.Phone == maskedTextBox1.Text &&
                u.Address == textBox6.Text))
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
                s.Name = MultiTextNG1 != "" ? MultiTextNG1 : s.Name;
                s.FullName = textBox1.Text != "" ? textBox1.Text : s.FullName;
                s.CheckingAccount = textBox4.Text != "" ? textBox4.Text : s.CheckingAccount;
                s.INN = textBox2.Text != "" ? textBox2.Text : s.INN;
                s.Phone = maskedTextBox1.Text != "" ? maskedTextBox1.Text : s.Phone;
                s.E_Mail = textBox6.Text != "" ? textBox6.Text : s.E_Mail;
                s.Address = MultiTextNG2 != "" ? MultiTextNG2 : s.Address;
                NGDB.List["Поставщики"].UpdateInfo(s.NGToString());
            }
            NGDB.List["Поставщики"].PrintInfo(dataGridView1, List);
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
        private void button1_Click(object sender, EventArgs e)
        {
            if (new NGMultiText().ShowDialog() == DialogResult.Cancel)
                return;
            MultiTextNG2 = NGOptions.StackObjects.Pop().ToString();
            toolTip2.SetToolTip(button1, MultiTextNG2);
            button1.BackColor = Color.Yellow;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (new NGMultiText().ShowDialog() == DialogResult.Cancel)
                return;
            MultiTextNG1 = NGOptions.StackObjects.Pop().ToString();
            toolTip1.SetToolTip(button2, MultiTextNG1);
            button2.BackColor = Color.Yellow;
        }
    }
}
