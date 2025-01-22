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
    public partial class ICabinetEmployee : Form
    {
        public int Index = 0;
        public ICabinetEmployee()
        {
            InitializeComponent();
            LoadRes();
        }
        public void LoadRes()
        {
            NGDB.List["Кабинеты"].PrintInfo(dataGridView1);
            dataGridView1.Rows.Clear();
        }
        public void Clear()
        {
            Index = 0;
            toolTip1.SetToolTip(button4, "");
            button4.BackColor = Color.LightGray;
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (new FormOther(NGDB.List["Сотрудники"]).ShowDialog() == DialogResult.Cancel)
                return;
            toolTip1.SetToolTip(button4, NGOptions.StackObjects.Pop().ToString());
            Index = Convert.ToInt32(NGOptions.StackObjects.Pop());
            button4.BackColor = Color.Yellow;
        }
        public bool isValid(bool flag)
        {
            if (numericUpDown1.Value == 0 ||
                numericUpDown2.Value == 0 ||
                Index == 0)
            {
                if (flag) 
                    MessageBox.Show("Поля должны быть заполнены!");
                return false;
            }
            else if (NGDB.List["Кабинеты"].SelectInfo().Cast<CabinetEmployee>().Any(u =>
                u.Cabinet == numericUpDown2.Value &&
                u.Campus == numericUpDown1.Value))
            {
                if (flag) 
                    MessageBox.Show("Данный кабинет уже существует!");
                return false;
            }
            else if (NGDB.List["Кабинеты"].SelectInfo().Cast<CabinetEmployee>().Any(u =>
                u.Cabinet == numericUpDown2.Value &&
                u.Campus == numericUpDown1.Value &&
                u.Employee.ID == Index))
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
            CabinetEmployee cabinet = new CabinetEmployee
            {
                Employee = NGDB.List["Сотрудники"].SelectInfo().First(u => u.ID == Index) as Employee,
                Campus = (int)numericUpDown1.Value,
                Cabinet = (int)numericUpDown2.Value,
            };
            NGDB.List["Кабинеты"].InsertInfo(cabinet.NGToString());
            dataGridView1.Rows.Add(new object[]
            {
                0,
                cabinet.Campus,
                cabinet.Cabinet,
                cabinet.Employee.Name,
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
                    CabinetEmployee.CabinetEmployeeST obj = serializer.Deserialize<CabinetEmployee.CabinetEmployeeST>(reader);
                    numericUpDown1.Value = obj.Campus;
                    numericUpDown2.Value = obj.Cabinet;
                    Index = obj.EmployeeID;
                    if (!isValid(false))
                        list.Add(ij);
                    else
                    {
                        CabinetEmployee cabinet = new CabinetEmployee
                        {
                            Employee = NGDB.List["Сотрудники"].SelectInfo().First(u => u.ID == Index) as Employee,
                            Campus = (int)numericUpDown1.Value,
                            Cabinet = (int)numericUpDown2.Value,
                        };
                        NGDB.List["Кабинеты"].InsertInfo(cabinet.NGToString());
                        dataGridView1.Rows.Add(new object[]
                        {
                            0,
                            cabinet.Campus,
                            cabinet.Cabinet,
                            cabinet.Employee.Name,
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
