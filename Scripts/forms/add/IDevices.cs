using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DipProject
{
    public partial class IDevices : Form
    {
        public int[] ArrayIndex = new int[3];
        public string MultiTextNG1 = "";
        public string MultiTextNG2 = "";
        public IDevices()
        {
            InitializeComponent();
            LoadRes();
        }
        public void LoadRes()
        {
            NGDB.List["Устройства"].PrintInfo(dataGridView1);
            dataGridView1.Rows.Clear();
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
        public bool isValid(bool flag)
        {
            if (textBox2.Text == "" ||
                textBox3.Text == "" ||
                ArrayIndex.Contains(0) ||
                numericUpDown1.Value == 0 ||
                numericUpDown2.Value == 0 ||
                numericUpDown3.Value == 0 ||
                comboBox1.SelectedItem == null ||
                comboBox2.SelectedItem == null ||
                MultiTextNG1 == "")
            {
                if (flag) 
                    MessageBox.Show("Поля должны быть заполнены!");
                return false;
            }
            else if (dateTimePicker1.Value.Date > DateTime.Now.Date)
            {
                if (flag) 
                    MessageBox.Show("Дата не может быть больше текущей!");
                return false;
            }
            else if (NGDB.List["Устройства"].SelectInfo().Cast<Device>().Any(u => u.InventoryNumber == textBox2.Text || u.SerialNumber == textBox3.Text))
            {
                if (flag) 
                    MessageBox.Show("Ни инвентарный номер, ни серийный не должны повторяться!");
                return false;
            }
            else if (NGDB.List["Устройства"].SelectInfo().Cast<Device>().Any(u => 
                u.Name == MultiTextNG1 &&
                u.Characteristics == MultiTextNG2 &&
                u.TypeDevice.ID == ArrayIndex[0] &&
                u.InventoryNumber == textBox2.Text &&
                u.SerialNumber == textBox2.Text &&
                u.Cost == (float)numericUpDown1.Value &&
                u.Employee.ID == ArrayIndex[1] &&
                u.PurchaseDate.Date == dateTimePicker1.Value.Date &&
                u.Gurantee == Convert.ToInt32(comboBox1.SelectedItem) &&
                u.StatusDevice == comboBox2.SelectedItem.ToString() &&
                u.Provider.ID == ArrayIndex[2] &&
                u.Campus == (int)numericUpDown2.Value &&
                u.Cabinet == (int)numericUpDown3.Value))
            {
                if (flag) 
                    MessageBox.Show("Такая запись уже существует!");
                return false;
            }
            else
                return true;
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!isValid(true))
                return;
            if (MessageBox.Show("Вы точно желаете добавить данную запись?", "Сообщение", 
                MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            Device device = new Device
            {
                Name = MultiTextNG1,
                Characteristics = MultiTextNG2,
                TypeDevice = NGDB.List["Типы устройств"].SelectInfo().First(u => u.ID == ArrayIndex[0]) as TypeDevice,
                InventoryNumber = textBox2.Text,
                SerialNumber = textBox3.Text,
                Cost = (float)numericUpDown1.Value,
                Employee = NGDB.List["Сотрудники"].SelectInfo().First(u => u.ID == ArrayIndex[1]) as Employee,
                PurchaseDate = dateTimePicker1.Value.Date,
                Gurantee = Convert.ToInt32(comboBox1.SelectedItem),
                StatusDevice = comboBox2.SelectedItem.ToString(),
                Provider = NGDB.List["Поставщики"].SelectInfo().First(u => u.ID == ArrayIndex[2]) as Provider,
                Campus = (int)numericUpDown2.Value,
                Cabinet = (int)numericUpDown3.Value,
            };
            NGDB.List["Устройства"].InsertInfo(device.NGToString());
            dataGridView1.Rows.Add(new object[]
            {
                0,
                device.Name,
                device.Characteristics,
                device.TypeDevice.Name,
                device.InventoryNumber,
                device.SerialNumber,
                device.Cost,
                device.PurchaseDate.Day + "." + device.PurchaseDate.Month + "." + device.PurchaseDate.Year,
                device.Gurantee,
                device.StatusDevice,
                device.Campus,
                device.Cabinet,
            });
            MessageBox.Show("Запись успешно добавлена!");
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
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "json files (*.json)|*.json";
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            try
            {
                JsonTextReader reader = new JsonTextReader(new StringReader(File.ReadAllText(openFileDialog1.FileName, Encoding.Default)))
                {
                    SupportMultipleContent = true,
                };
                JsonSerializer serializer = new JsonSerializer();
                List<int> list = new List<int>();
                int ij = 1;
                while (reader.Read())
                {
                    Device.DeviceST obj = serializer.Deserialize<Device.DeviceST>(reader);
                    MultiTextNG1 = obj.Name;
                    MultiTextNG2 = obj.Characteristics;
                    ArrayIndex[0] = obj.TypeDeviceID;
                    textBox2.Text = obj.InventoryNumber;
                    textBox3.Text = obj.SerialNumber;
                    numericUpDown1.Value = (decimal)obj.Cost;
                    ArrayIndex[1] = obj.EmployeeID;
                    dateTimePicker1.Value = NGExtens.ToDateTime(obj.PurchaseDate).Date;
                    comboBox1.SelectedItem = obj.Gurantee.ToString();
                    comboBox2.SelectedItem = obj.StatusDevice;
                    ArrayIndex[2] = obj.ProviderID;
                    numericUpDown2.Value = obj.Campus;
                    numericUpDown3.Value = obj.Cabinet;
                    if (!isValid(false))
                        list.Add(ij);
                    else
                    {
                        Device device = new Device
                        {
                            Name = MultiTextNG1,
                            Characteristics = MultiTextNG2,
                            TypeDevice = NGDB.List["Типы устройств"].SelectInfo().First(u => u.ID == ArrayIndex[0]) as TypeDevice,
                            InventoryNumber = textBox2.Text,
                            SerialNumber = textBox3.Text,
                            Cost = (float)numericUpDown1.Value,
                            Employee = NGDB.List["Сотрудники"].SelectInfo().First(u => u.ID == ArrayIndex[1]) as Employee,
                            PurchaseDate = dateTimePicker1.Value.Date,
                            Gurantee = Convert.ToInt32(comboBox1.SelectedItem),
                            StatusDevice = comboBox2.SelectedItem.ToString(),
                            Provider = NGDB.List["Поставщики"].SelectInfo().First(u => u.ID == ArrayIndex[2]) as Provider,
                            Campus = (int)numericUpDown2.Value,
                            Cabinet = (int)numericUpDown3.Value,
                        };
                        NGDB.List["Устройства"].InsertInfo(device.NGToString());
                        dataGridView1.Rows.Add(new object[]
                        {
                            0,
                            device.Name,
                            device.Characteristics,
                            device.TypeDevice.Name,
                            device.InventoryNumber,
                            device.SerialNumber,
                            device.Cost,
                            device.PurchaseDate.Day + "." + device.PurchaseDate.Month + "." + device.PurchaseDate.Year,
                            device.Gurantee,
                            device.StatusDevice,
                            device.Campus,
                            device.Cabinet,
                        });
                    }
                    ij++;
                }
                MessageBox.Show(@"Импорт произошёл успешно!
                                Не удалось импортировать записи - " + string.Join("|", list.ToArray().Select(u => u.ToString())));
            }
            catch
            {
                MessageBox.Show("Не удалось импортировать json-файл!");
            }
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)) && e.KeyChar != (char)Keys.Back)
                e.Handled = true;
        }
    }
}
