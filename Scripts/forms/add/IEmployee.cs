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
    public partial class IEmployee : Form
    {
        public IEmployee()
        {
            InitializeComponent();
            LoadRes();
        }
        public void LoadRes()
        {
            NGDB.List["Сотрудники"].PrintInfo(dataGridView1);
            dataGridView1.Rows.Clear();
        }
        public void Clear()
        {
            textBox2.Text = "";
            textBox1.Text = "";
        }
        public bool isValid(bool flag)
        {
            if (textBox1.Text == "" ||
                textBox2.Text == "")
            {
                if (flag) 
                    MessageBox.Show("Поля должны быть заполнены!");
                return false;
            }
            else if (NGDB.List["Сотрудники"].SelectInfo().Cast<Employee>().Any(u =>
                u.Name == textBox2.Text &&
                u.Position == textBox1.Text))
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
            Employee employee = new Employee
            {
                Name = textBox2.Text,
                Position = textBox1.Text,
            };
            NGDB.List["Сотрудники"].InsertInfo(employee.NGToString());
            dataGridView1.Rows.Add(new object[]
            {
                0,
                employee.Name,
                employee.Position,
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
                    Employee.EmployeeST obj = serializer.Deserialize<Employee.EmployeeST>(reader);
                    textBox2.Text = obj.Name;
                    textBox1.Text = obj.Position;
                    if (!isValid(false))
                        list.Add(ij);
                    else
                    {
                        Employee employee = new Employee
                        {
                            Name = textBox2.Text,
                            Position = textBox1.Text,
                        };
                        NGDB.List["Сотрудники"].InsertInfo(employee.NGToString());
                        dataGridView1.Rows.Add(new object[]
                        {
                            0,
                            employee.Name,
                            employee.Position,
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
