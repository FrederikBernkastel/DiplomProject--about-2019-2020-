using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DipProject
{
    public partial class Report : Form
    {
        public DataGridView Grid;
        public Report(DataGridView grid)
        {
            InitializeComponent();
            Grid = grid;
            LoadRes();
        }
        public void LoadRes()
        {
            toolTip1.SetToolTip(button3, "Сохранить куда");
            toolTip2.SetToolTip(textBox1, "Путь экспорта");
            toolTip3.SetToolTip(comboBox1, "Имя заголовка");
            foreach (DataGridViewColumn s in Grid.Columns.Cast<DataGridViewColumn>().Skip(1))
                treeView1.Nodes.Add(new TreeNode(s.HeaderText)
                {
                    Checked = true,
                });
            
            comboBox1.SelectedItem = "Не выбрано";
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "excel files (*.xlsx)|*.xlsx";
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            textBox1.Text = saveFileDialog1.FileName + ".xslx";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (isValid())
            {
                ExcelPackage package = new ExcelPackage();
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add(comboBox1.SelectedItem.ToString() == "Не выбрано" ? " " : comboBox1.SelectedItem.ToString());
                int i = 1;
                foreach (TreeNode s in treeView1.Nodes)
                    if (s.Checked)
                    {
                        sheet.Cells[2, i].Value = s.Text;
                        for (int j = 3; j < Grid.Rows.Count + 3; j++)
                        {
                            sheet.Cells[j, i].Value = Grid.Rows[j - 3].Cells[s.Text].Value;
                            sheet.Cells[j, i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);
                        }
                        i++;
                    }
                sheet.Cells[1, 1, 1, i].Merge = true;
                if (comboBox1.SelectedItem.ToString() != "Не выбрано")
                {
                    sheet.Cells[1, 1].Value = comboBox1.SelectedItem.ToString();
                    sheet.Cells[1, 1].Style.Font.Size = 14;
                    sheet.Cells[1, 1].Style.Font.Bold = true;
                }
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
                File.WriteAllBytes(textBox1.Text, package.GetAsByteArray());
                MessageBox.Show("Экспорт Excel-отчёта успешно выполнен!");
                Process.Start(textBox1.Text);
            }
        }
        public bool isValid()
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Необходимо выбрать путь экспорта!");
                return false;
            }
            else if (treeView1.Nodes.Cast<TreeNode>().All(u => !u.Checked))
            {
                MessageBox.Show("Необходимо выбрать хотя бы один столбец!");
                return false;
            }
            else
                return true;
        }
    }
}
