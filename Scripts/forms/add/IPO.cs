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
    public partial class IPO : Form
    {
        public int Index = 0;
        public string MultiTextNG1 = "";
        public IPO()
        {
            InitializeComponent();
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
            NGDB.List["ПО"].PrintInfo(dataGridView1);
            dataGridView1.Rows.Clear();
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
        public bool isValid(bool flag)
        {
            if (numericUpDown1.Value == 0)
            {
                if (MultiTextNG1 == "" ||
                    Index == 0)
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
                else if (NGDB.List["ПО"].SelectInfo().Cast<PO>().Any(u =>
                    u.Name == MultiTextNG1 &&
                    u.Cost == (float)numericUpDown1.Value &&
                    u.Device.ID == Index))
                {
                    if (flag) 
                        MessageBox.Show("Такая запись уже существует!");
                    return false;
                }
                else
                    return true;
            }
            else
            {
                if (MultiTextNG1 == "" ||
                    Index == 0 ||
                    textBox1.Text == "" ||
                    comboBox1.SelectedItem == null)
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
                else if (NGDB.List["ПО"].SelectInfo().Cast<PO>().Any(u =>
                    u.Name == MultiTextNG1 &&
                    u.Cost == (float)numericUpDown1.Value &&
                    u.Device.ID == Index &&
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
            if (!isValid(true))
                return;
            if (MessageBox.Show("Вы точно желаете добавить данную запись?", "Сообщение", 
                MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            PO po = new PO
            {
                Name = MultiTextNG1,
                Device = NGDB.List["Устройства"].SelectInfo().First(u => u.ID == Index) as Device,
                Cost = (float)numericUpDown1.Value,
                RegistrationKey = numericUpDown1.Value == 0 ? " " : textBox1.Text,
                PurchaseDate = dateTimePicker1.Value.Date,
                Gurantee = numericUpDown1.Value == 0 ? 0 : Convert.ToInt32(comboBox1.SelectedItem),
                StatusPO = "Действительно",
            };
            NGDB.List["ПО"].InsertInfo(po.NGToString());
            dataGridView1.Rows.Add(new object[]
            {
                0,
                po.Name,
                po.Cost,
                po.RegistrationKey,
                po.PurchaseDate.Day + "." + po.PurchaseDate.Month + "." + po.PurchaseDate.Year,
                po.Gurantee,
                po.StatusPO,
                po.Cost == 0 ? "Нелицензионное" : "Лицензионное",
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
        private void button1_Click(object sender, EventArgs e)
        {
            if (new NGMultiText().ShowDialog() == DialogResult.Cancel)
                return;
            MultiTextNG1 = NGOptions.StackObjects.Pop().ToString();
            toolTip1.SetToolTip(button1, MultiTextNG1);
            button1.BackColor = Color.Yellow;
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
                    PO.POST obj = serializer.Deserialize<PO.POST>(reader);
                    MultiTextNG1 = obj.Name;
                    Index = obj.DeviceID;
                    numericUpDown1.Value = (decimal)obj.Cost;
                    textBox1.Text = obj.RegistrationKey;
                    dateTimePicker1.Value = NGExtens.ToDateTime(obj.PurchaseDate).Date;
                    comboBox1.SelectedItem = obj.Gurantee.ToString();
                    if (!isValid(false))
                        list.Add(ij);
                    else
                    {
                        PO po = new PO
                        {
                            Name = MultiTextNG1,
                            Device = NGDB.List["Устройства"].SelectInfo().First(u => u.ID == Index) as Device,
                            Cost = (float)numericUpDown1.Value,
                            RegistrationKey = numericUpDown1.Value == 0 ? " " : textBox1.Text,
                            PurchaseDate = dateTimePicker1.Value.Date,
                            Gurantee = numericUpDown1.Value == 0 ? 0 : Convert.ToInt32(comboBox1.SelectedItem),
                            StatusPO = obj.StatusPO,
                        };
                        NGDB.List["ПО"].InsertInfo(po.NGToString());
                        dataGridView1.Rows.Add(new object[]
                        {
                            0,
                            po.Name,
                            po.Cost,
                            po.RegistrationKey,
                            po.PurchaseDate.Day + "." + po.PurchaseDate.Month + "." + po.PurchaseDate.Year,
                            po.Gurantee,
                            po.StatusPO,
                            po.Cost == 0 ? "Нелицензионное" : "Лицензионное",
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
    }
}
