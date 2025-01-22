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
    public partial class UDevices : Form
    {
        public IEnumerable<Device> List;
        public int[] ArrayIndex = new int[3];
        public string MultiTextNG1 = "";
        public string MultiTextNG2 = "";
        public UDevices(IEnumerable<Device> list)
        {
            InitializeComponent();
            List = list;
            LoadRes();
        }
        public void LoadRes()
        {
            NGDB.List["Устройства"].PrintInfo(dataGridView1, List);
        }
        public void Clear()
        {
            MultiTextNG1 = "";
            MultiTextNG2 = "";
            ArrayIndex = new int[3];
            toolTip1.SetToolTip(button6, "");
            toolTip2.SetToolTip(button1, "");
            toolTip3.SetToolTip(button3, "");
            toolTip4.SetToolTip(button4, "");
            toolTip5.SetToolTip(button5, "");
            button6.BackColor = Color.LightGray;
            button3.BackColor = Color.LightGray;
            button4.BackColor = Color.LightGray;
            button5.BackColor = Color.LightGray;
            button1.BackColor = Color.LightGray;
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            numericUpDown3.Value = 0;
            dateTimePicker1.Value = DateTime.Now;
            comboBox1.SelectedItem = null;
            comboBox2.SelectedItem = null;
            textBox2.Text = "";
            textBox3.Text = "";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (new NGMultiText().ShowDialog() == DialogResult.Cancel)
                return;
            MultiTextNG1 = NGOptions.StackObjects.Pop().ToString();
            toolTip2.SetToolTip(button1, MultiTextNG1);
            button1.BackColor = Color.Yellow;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (new NGMultiText().ShowDialog() == DialogResult.Cancel)
                return;
            MultiTextNG2 = NGOptions.StackObjects.Pop().ToString();
            toolTip3.SetToolTip(button3, MultiTextNG2);
            button3.BackColor = Color.Yellow;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (new FormOther(NGDB.List["Типы устройств"]).ShowDialog() == DialogResult.Cancel)
                return;
            toolTip1.SetToolTip(button6, NGOptions.StackObjects.Pop().ToString());
            ArrayIndex[0] = Convert.ToInt32(NGOptions.StackObjects.Pop());
            button6.BackColor = Color.Yellow;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (new FormOther(NGDB.List["Сотрудники"]).ShowDialog() == DialogResult.Cancel)
                return;
            toolTip4.SetToolTip(button4, NGOptions.StackObjects.Pop().ToString());
            ArrayIndex[1] = Convert.ToInt32(NGOptions.StackObjects.Pop());
            button4.BackColor = Color.Yellow;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (new FormOther(NGDB.List["Поставщики"]).ShowDialog() == DialogResult.Cancel)
                return;
            toolTip4.SetToolTip(button5, NGOptions.StackObjects.Pop().ToString());
            ArrayIndex[2] = Convert.ToInt32(NGOptions.StackObjects.Pop());
            button5.BackColor = Color.Yellow;
        }
        public bool isValid()
        {
            if (textBox2.Text == "" &&
                textBox3.Text == "" &&
                ArrayIndex.All(u => u == 0) &&
                numericUpDown1.Value == 0 &&
                numericUpDown2.Value == 0 &&
                numericUpDown3.Value == 0 &&
                comboBox1.SelectedItem == null &&
                comboBox2.SelectedItem == null &&
                MultiTextNG1 == "" &&
                MultiTextNG2 == "")
            {
                MessageBox.Show("Заполните хотя бы одно поле!");
                return false;
            }
            else if (List.Count() > 1 &&
                textBox2.Text != "" &&
                textBox3.Text != "" &&
                ArrayIndex.All(u => u != 0) &&
                numericUpDown1.Value != 0 &&
                numericUpDown2.Value != 0 &&
                numericUpDown3.Value != 0 &&
                comboBox1.SelectedItem != null &&
                comboBox2.SelectedItem != null &&
                MultiTextNG1 != "" &&
                MultiTextNG2 != "")
            {
                MessageBox.Show("Множественное изменение невозможно при заполнении всех полей!");
                return false;
            }
            else if (List.Count() < 2 && dateTimePicker1.Value.Date > DateTime.Now.Date)
            {
                MessageBox.Show("Дата не может быть больше текущей!");
                return false;
            }
            else if (List.Count() < 2 && NGDB.List["Устройства"].SelectInfo().Cast<Device>().Any(u => u.InventoryNumber == textBox2.Text || u.SerialNumber == textBox3.Text))
            {
                MessageBox.Show("Ни инвентарный номер, ни серийный не должны повторяться!");
                return false;
            }
            else if (List.Count() < 2 && NGDB.List["Устройства"].SelectInfo().Cast<Device>().Any(u => 
                u.Name == MultiTextNG1 &&
                u.Characteristics == MultiTextNG2 &&
                u.TypeDevice != null ? u.TypeDevice.ID == ArrayIndex[0] : false &&
                u.InventoryNumber == textBox2.Text &&
                u.SerialNumber == textBox2.Text &&
                u.Cost == (float)numericUpDown1.Value &&
                u.Employee != null ? u.Employee.ID == ArrayIndex[1] : false &&
                u.PurchaseDate.Date == dateTimePicker1.Value.Date &&
                u.Gurantee == Convert.ToInt32(comboBox1.SelectedItem) &&
                u.StatusDevice == comboBox2.SelectedItem.ToString() &&
                u.Provider != null ? u.Provider.ID == ArrayIndex[2] : false &&
                u.Campus == (int)numericUpDown2.Value &&
                u.Cabinet == (int)numericUpDown3.Value))
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
                s.Characteristics = MultiTextNG2 != "" ? MultiTextNG2 : s.Characteristics;
                s.TypeDevice = ArrayIndex[0] != 0 ? NGDB.List["Типы устройств"].SelectInfo().First(u => u.ID == ArrayIndex[0]) as TypeDevice : s.TypeDevice;
                s.InventoryNumber = textBox2.Text != "" ? textBox2.Text : s.InventoryNumber;
                s.SerialNumber = textBox3.Text != "" ? textBox3.Text : s.SerialNumber;
                s.Cost = numericUpDown1.Value != 0 ? (float)numericUpDown1.Value : s.Cost;
                s.Employee = ArrayIndex[1] != 0 ? NGDB.List["Сотрудники"].SelectInfo().First(u => u.ID == ArrayIndex[1]) as Employee : s.Employee;
                s.Gurantee = comboBox1.SelectedItem != null ? Convert.ToInt32(comboBox1.SelectedItem) : s.Gurantee;
                s.StatusDevice = comboBox2.SelectedItem != null ? comboBox2.SelectedItem.ToString() : s.StatusDevice;
                s.Provider = ArrayIndex[2] != 0 ? NGDB.List["Поставщики"].SelectInfo().First(u => u.ID == ArrayIndex[2]) as Provider : s.Provider;
                s.Campus = numericUpDown2.Value != 0 ? (int)numericUpDown2.Value : s.Campus;
                s.Cabinet = numericUpDown3.Value != 0 ? (int)numericUpDown3.Value : s.Cabinet;
                NGDB.List["Устройства"].UpdateInfo(s.NGToString());
            }
            NGDB.List["Устройства"].PrintInfo(dataGridView1, List);
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
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)) && e.KeyChar != (char)Keys.Back)
                e.Handled = true;
        }
    }
}
