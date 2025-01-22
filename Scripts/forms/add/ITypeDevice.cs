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
    public partial class ITypeDevice : Form
    {
        public ITypeDevice()
        {
            InitializeComponent();
            LoadRes();
        }
        public void LoadRes()
        {
            NGDB.List["Типы устройств"].PrintInfo(dataGridView1);
            dataGridView1.Rows.Clear();
        }
        public void Clear()
        {
            textBox1.Text = "";
        }
        public bool isValid(bool flag)
        {
            if (textBox1.Text == "")
            {
                if (flag) 
                    MessageBox.Show("Поля должны быть заполнены!");
                return false;
            }
            else if (NGDB.List["Типы устройств"].SelectInfo().Cast<TypeDevice>().Any(u => u.Name == textBox1.Text))
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
            TypeDevice type_device = new TypeDevice
            {
                Name = textBox1.Text,
            };
            NGDB.List["Типы устройств"].InsertInfo(type_device.NGToString());
            dataGridView1.Rows.Add(new object[]
            {
                0,
                type_device.Name,
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
                    TypeDevice.TypeDeviceST obj = serializer.Deserialize<TypeDevice.TypeDeviceST>(reader);
                    textBox1.Text = obj.Name;
                    if (!isValid(false))
                        list.Add(ij);
                    else
                    {
                        TypeDevice type_device = new TypeDevice
                        {
                            Name = textBox1.Text,
                        };
                        NGDB.List["Типы устройств"].InsertInfo(type_device.NGToString());
                        dataGridView1.Rows.Add(new object[]
                        {
                            0,
                            type_device.Name,
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
