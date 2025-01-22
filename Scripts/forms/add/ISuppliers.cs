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
    public partial class ISuppliers : Form
    {
        public string MultiTextNG1 = "";
        public string MultiTextNG2 = "";
        public ISuppliers()
        {
            InitializeComponent();
            LoadRes();
        }
        public void LoadRes()
        {
            NGDB.List["Поставщики"].PrintInfo(dataGridView1);
            dataGridView1.Rows.Clear();
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
        public bool isValid(bool flag)
        {
            if (MultiTextNG1 == "" ||
                textBox1.Text == "" ||
                textBox4.Text == "" ||
                textBox2.Text == "" ||
                maskedTextBox1.Text == "" ||
                textBox6.Text == "" ||
                MultiTextNG2 == "")
            {
                if (flag) 
                    MessageBox.Show("Поля должны быть заполнены!");
                return false;
            }
            else if (NGDB.List["Поставщики"].SelectInfo().Cast<Provider>().Any(u => 
                u.Name == MultiTextNG1 &&
                u.FullName == textBox1.Text &&
                u.CheckingAccount == textBox4.Text &&
                u.INN == textBox2.Text &&
                u.Phone == maskedTextBox1.Text &&
                u.Address == textBox6.Text))
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
            Provider provider = new Provider
            {
                Name = MultiTextNG1,
                FullName = textBox1.Text,
                CheckingAccount = textBox4.Text,
                INN = textBox2.Text,
                Phone = maskedTextBox1.Text,
                E_Mail = textBox6.Text,
                Address = MultiTextNG2,
            };
            NGDB.List["Поставщики"].InsertInfo(provider.NGToString());
            dataGridView1.Rows.Add(new object[]
            {
                0,
                provider.Name,
                provider.FullName,
                provider.CheckingAccount,
                provider.INN,
                provider.Phone,
                provider.E_Mail,
                provider.Address,
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
                    Provider.ProviderST obj = serializer.Deserialize<Provider.ProviderST>(reader);
                    MultiTextNG1 = obj.Name;
                    textBox1.Text = obj.FullName;
                    textBox4.Text = obj.CheckingAccount;
                    textBox2.Text = obj.INN;
                    maskedTextBox1.Text = obj.Phone;
                    textBox6.Text = obj.E_Mail;
                    MultiTextNG2 = obj.Address;
                    if (!isValid(false))
                        list.Add(ij);
                    else
                    {
                        Provider provider = new Provider
                        {
                            Name = MultiTextNG1,
                            FullName = textBox1.Text,
                            CheckingAccount = textBox4.Text,
                            INN = textBox2.Text,
                            Phone = maskedTextBox1.Text,
                            E_Mail = textBox6.Text,
                            Address = MultiTextNG2,
                        };
                        NGDB.List["Поставщики"].InsertInfo(provider.NGToString());
                        dataGridView1.Rows.Add(new object[]
                        {
                            0,
                            provider.Name,
                            provider.FullName,
                            provider.CheckingAccount,
                            provider.INN,
                            provider.Phone,
                            provider.E_Mail,
                            provider.Address,
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
