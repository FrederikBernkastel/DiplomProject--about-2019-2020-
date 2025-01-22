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
    public partial class UPO : Form
    {
        public IEnumerable<PO> List;
        public int Index = 0;
        public string MultiTextNG1 = "";
        public UPO(IEnumerable<PO> list)
        {
            InitializeComponent();
            List = list;
            numericUpDown1.ValueChanged += NumericUpDown1_ValueChanged;
            LoadRes();
        }
        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value == 0)
            {
                textBox1.Enabled = false;
                comboBox1.Enabled = false;
            }
            else
            {
                textBox1.Enabled = true;
                comboBox1.Enabled = true;
            }
        }
        public void LoadRes()
        {
            NGDB.List["ПО"].PrintInfo(dataGridView1, List);
        }
        public void Clear()
        {
            MultiTextNG1 = "";
            numericUpDown1.Value = 0;
            button3.BackColor = Color.LightGray;
            toolTip1.SetToolTip(button3, "");
            Index = 0;
            textBox1.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            comboBox1.SelectedItem = null;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (new FormOther(NGDB.List["Устройства"]).ShowDialog() == DialogResult.Cancel)
                return;
            toolTip2.SetToolTip(button3, NGOptions.StackObjects.Pop().ToString());
            Index = Convert.ToInt32(NGOptions.StackObjects.Pop());
            button3.BackColor = Color.Yellow;
        }
        public bool isValid()
        {
            if (numericUpDown1.Value == 0)
            {
                if (MultiTextNG1 == "" &&
                    Index == 0)
                {
                    MessageBox.Show("Заполните хотя бы одно поле!");
                    return false;
                }
                else if (List.Count() > 1 &&
                    MultiTextNG1 != "" &&
                    Index != 0)
                {
                    MessageBox.Show("Множественное изменение невозможно при заполнении всех полей!");
                    return false;
                }
                else if (List.Count() < 2 && dateTimePicker1.Value.Date > DateTime.Now.Date)
                {
                    MessageBox.Show("Дата не может быть больше текущей!");
                    return false;
                }
                else if (List.Count() < 2 && NGDB.List["ПО"].SelectInfo().Cast<PO>().Any(u =>
                    u.Name == MultiTextNG1 &&
                    u.Cost == (float)numericUpDown1.Value &&
                    u.Device != null ? u.Device.ID == Index : false))
                {
                    MessageBox.Show("Такая запись уже существует!");
                    return false;
                }
                else
                    return true;
            }
            else
            {
                if (MultiTextNG1 == "" &&
                    Index == 0 &&
                    textBox1.Text == "" &&
                    comboBox1.SelectedItem == null)
                {
                    MessageBox.Show("Заполните хотя бы одно поле!");
                    return false;
                }
                else if (List.Count() > 1 &&
                    MultiTextNG1 != "" &&
                    Index != 0 &&
                    textBox1.Text != "" &&
                    comboBox1.SelectedItem != null)
                {
                    MessageBox.Show("Множественное изменение невозможно при заполнении всех полей!");
                    return false;
                }
                else if (List.Count() < 2 && dateTimePicker1.Value.Date > DateTime.Now.Date)
                {
                    MessageBox.Show("Дата не может быть больше текущей!");
                    return false;
                }
                else if (List.Count() < 2 && NGDB.List["ПО"].SelectInfo().Cast<PO>().Any(u =>
                    u.Name == MultiTextNG1 &&
                    u.Cost == (float)numericUpDown1.Value &&
                    u.Device != null ? u.Device.ID == Index : false &&
                    u.RegistrationKey == textBox1.Text &&
                    u.PurchaseDate.Date == dateTimePicker1.Value.Date &&
                    u.Gurantee == Convert.ToInt32(comboBox1.SelectedItem)))
                {
                    MessageBox.Show("Такая запись уже существует!");
                    return false;
                }
                else
                    return true;
            }
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
                s.Device = Index != 0 ? NGDB.List["Устройства"].SelectInfo().First(u => u.ID == Index) as Device : s.Device;
                s.Cost = numericUpDown1.Value != 0 ? (float)numericUpDown1.Value : s.Cost;
                s.RegistrationKey = numericUpDown1.Value == 0 ? " " : textBox1.Text != "" ? textBox1.Text : s.RegistrationKey;
                s.Gurantee = numericUpDown1.Value == 0 ? 0 : comboBox1.SelectedItem != null ? Convert.ToInt32(comboBox1.SelectedItem) : s.Gurantee;
                NGDB.List["ПО"].UpdateInfo(s.NGToString());
            }
            NGDB.List["ПО"].PrintInfo(dataGridView1, List);
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
            MultiTextNG1 = NGOptions.StackObjects.Pop().ToString();
            toolTip1.SetToolTip(button1, MultiTextNG1);
            button1.BackColor = Color.Yellow;
        }
    }
}
