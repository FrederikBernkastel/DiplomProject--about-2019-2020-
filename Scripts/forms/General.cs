using Newtonsoft.Json;
using OfficeOpenXml;
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
    public partial class General : Form
    {
        public BaseTable Table;
        public BaseTable TableWeb;
        public General()
        {
            InitializeComponent(); 
            dataGridView1.CellClick += dataGridView1_CellClick;
            tabControl1.Selected += TabControl1_Selected;
            tabControl2.Selected += TabControl2_Selected;
            FormClosing += General_FormClosing;
            LoadRes();
            UpdateResGrid1();
        }
        private void General_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.WindowsShutDown)
                return;
            if (DialogResult == DialogResult.Cancel)
            {
                switch (MessageBox.Show("Действительно хотите выйти?", "Сообщение", MessageBoxButtons.YesNo))
                {
                    case DialogResult.No:
                        e.Cancel = true;
                        break;
                    default:
                        break;
                }
            }
        }
        public void LoadRes()
        {
            menuStrip1.Items.Clear();
            ToolStripMenuItem item1 = new ToolStripMenuItem("Файл");
            ToolStripMenuItem item14 = new ToolStripMenuItem("Обновить базу");
            ToolStripMenuItem item11 = new ToolStripMenuItem("Настройки");
            ToolStripMenuItem item12 = new ToolStripMenuItem("Сменить пользователя");
            ToolStripMenuItem item13 = new ToolStripMenuItem("Выйти");
            item11.Click += Item11_Click;
            item12.Click += Item12_Click;
            item13.Click += Item13_Click;
            item14.Click += item14_Click;
            item1.DropDownItems.Add(item14);
            item1.DropDownItems.Add(item11);
            item1.DropDownItems.Add(item12);
            item1.DropDownItems.Add(item13);
            menuStrip1.Items.Add(item1);
            tabControl1.TabPages.Clear();
            foreach (var s in NGOptions.List)
                tabControl1.TabPages.Add(NGDB.List[s].RUSNameTable);
            toolStrip1.Items.Clear();
            if (NGOptions.isAdmin)
            {
                MessageBox.Show("Добро пожаловать, admin!");
                label2.Text = "admin";
                ToolStripButton but1 = new ToolStripButton("Добавить");
                ToolStripButton but2 = new ToolStripButton("Изменить");
                ToolStripButton but3 = new ToolStripButton("Удалить");
                ToolStripButton but4 = new ToolStripButton("Фильтр по основной таблице");
                ToolStripButton but5 = new ToolStripButton("Сбросить фильтр");
                ToolStripButton but6 = new ToolStripButton("Экспорт в JSON");
                ToolStripButton but7 = new ToolStripButton("Оформление отчёта по основной таблице");
                ToolStripButton but8 = new ToolStripButton("Оформление отчёта по связующей таблице");
                ToolStripButton but9 = new ToolStripButton("Фильтр по связующей таблице");
                but1.Click += but1_Click;
                but2.Click += but2_Click;
                but3.Click += but3_Click;
                but4.Click += But4_Click;
                but5.Click += But5_Click;
                but6.Click += But6_Click;
                but7.Click += but7_Click;
                but8.Click += but8_Click;
                but9.Click += but9_Click;
                toolStrip1.Items.Add(but1);
                toolStrip1.Items.Add(but2);
                toolStrip1.Items.Add(but3);
                toolStrip1.Items.Add(but4);
                toolStrip1.Items.Add(but5);
                toolStrip1.Items.Add(but6);
                toolStrip1.Items.Add(but7);
                toolStrip1.Items.Add(but8);
                toolStrip1.Items.Add(but9);
                ToolStripMenuItem item3 = new ToolStripMenuItem("БД");
                ToolStripMenuItem item31 = new ToolStripMenuItem("Экспортировать БД");
                ToolStripMenuItem item32 = new ToolStripMenuItem("Импортировать БД");
                item31.Click += item15_Click;
                item32.Click += item16_Click;
                item3.DropDownItems.Add(item31);
                item3.DropDownItems.Add(item32);
                menuStrip1.Items.Add(item3);
            }
            else
            {
                MessageBox.Show("Добро пожаловать, user!");
                label2.Text = "user";
                ToolStripButton but1 = new ToolStripButton("Фильтр по основной таблице");
                ToolStripButton but2 = new ToolStripButton("Сбросить фильтр");
                ToolStripButton but4 = new ToolStripButton("Оформление отчёта по основной таблице");
                ToolStripButton but5 = new ToolStripButton("Оформление отчёта по связующей таблице");
                ToolStripButton but6 = new ToolStripButton("Фильтр по связующей таблице");
                but1.Click += But4_Click;
                but2.Click += But5_Click;
                but4.Click += but7_Click;
                but5.Click += but8_Click;
                but6.Click += but9_Click;
                toolStrip1.Items.Add(but1);
                toolStrip1.Items.Add(but2);
                toolStrip1.Items.Add(but4);
                toolStrip1.Items.Add(but5);
                toolStrip1.Items.Add(but6);
            }
            timer1.Enabled = true;
            timer1.Interval = 1000;
            label3.Text = "Готово";
            Table = NGDB.List[NGOptions.List.Peek()];
        }
        private void but9_Click(object sender, EventArgs e)
        {
            if (Table.WebTables.Count() == 0)
                return;
            if (new FilterOther(Table).ShowDialog() == DialogResult.Cancel)
                return;
            try
            {
                string[][] vok = NGOptions.StackObjects.Pop() as string[][];
                string[] dtn = NGOptions.StackObjects.Pop() as string[];
                IEnumerable<BaseData> SD = Enumerable.Empty<BaseData>();
                if (dtn.Length == 0)
                {
                    if (vok.Length != 0)
                        SD = Table.FilterPrint(vok[0]);
                }
                else
                {
                    SD = Table.FilterPrint(vok[0]);
                    for (int i = 1; i < vok.Length; i++)
                    {
                        if (dtn[i - 1] == "И")
                        {
                            var gig = Table.FilterPrint(vok[i]);
                            SD = SD.Where(u => gig.Select(j => j.ID).Contains(u.ID));
                        }
                        else if (dtn[i - 1] == "ИЛИ")
                            SD = SD.Union(Table.FilterPrint(vok[i])).GroupBy(u => u.ID).Select(u => u.First());
                    }
                }
                Table.PrintInfo(dataGridView1, SD);
                label5.Text = "Кол-во записей: " + dataGridView1.Rows.Count.ToString();
                UpdateResGrid2();
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при фильтровании БД!");
            }
        }
        private void but8_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Columns.Count != 0)
                new Report(dataGridView2).ShowDialog();
        }
        private void but7_Click(object sender, EventArgs e)
        {
            new Report(dataGridView1).ShowDialog();
        }
        private void item16_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.Filter = "sqlite files (*.sqlite)|*.sqlite";
                if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                File.Copy(openFileDialog1.FileName, "db.sqlite", true);
                MessageBox.Show("Импорт выполнен успешно!");
                UpdateResGrid1();
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при импорте БД!");
            }
        }
        private void item15_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.Filter = "sqlite files (*.sqlite)|*.sqlite";
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                File.Copy("db.sqlite", saveFileDialog1.FileName + ".sqlite", true);
                MessageBox.Show("Экспорт выполнен успешно!");
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при экспорте БД!");
            }
        }
        private void item14_Click(object sender, EventArgs e)
        {
            try
            {
                new NGMessage(() => NGDB.Update()).ShowDialog();
                MessageBox.Show("Обновление прошло успешно!");
                UpdateResGrid1();
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при обновлении БД!");
            }
        }
        private void But4_Click(object sender, EventArgs e)
        {
            try
            {
                if (new Filter(dataGridView1).ShowDialog() == DialogResult.Cancel)
                    return;
                string[][] vok = NGOptions.StackObjects.Pop() as string[][];
                string[] dtn = NGOptions.StackObjects.Pop() as string[];
                IEnumerable<DataGridViewRow> SD = Enumerable.Empty<DataGridViewRow>();
                if (dtn.Length == 0)
                {
                    if (vok.Length != 0)
                        SD = NGExtens.GetRowsFilter(dataGridView1, vok[0]);
                }
                else
                {
                    SD = NGExtens.GetRowsFilter(dataGridView1, vok[0]);
                    for (int i = 1; i < vok.Length; i++)
                        SD = NGExtens.GetManyRowsFilter(SD, NGExtens.GetRowsFilter(dataGridView1, vok[i]), dtn[i - 1]);
                }
                NGExtens.GridUpdate(dataGridView1, SD.ToArray());
                label5.Text = "Кол-во записей: " + dataGridView1.Rows.Count.ToString();
                UpdateResGrid2();
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при фильтровании БД!");
            }
        }
        private void But5_Click(object sender, EventArgs e)
        {
            UpdateResGrid1();
        }
        private void But6_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.Filter = "json files (*.json)|*.json";
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                using (StreamWriter writer = new StreamWriter(saveFileDialog1.FileName + ".json", false, Encoding.Default))
                {
                    foreach (var s in dataGridView1.Rows.Cast<DataGridViewRow>().Select(u => Table.SelectInfo().First(b => b.ID == Convert.ToInt32(u.Cells[0].Value)).SerializeJSON()))
                        writer.WriteLine(s);
                }
                MessageBox.Show("Экспорт выполнен успешно!");
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при экспорте в JSON!");
            }
        }
        private void but3_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0)
                    MessageBox.Show("Выделите строку для удаления!");
                else
                {
                    Table.ShowFormRemoveInfo(dataGridView1.SelectedRows.Cast<DataGridViewRow>()
                        .Select(u => Convert.ToInt32(u.Cells[0].Value)).ToArray());
                    UpdateResGrid1();
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при удалении данных!");
            }
        }
        private void but2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0)
                    MessageBox.Show("Выделите строку для изменения!");
                else
                {
                    Table.ShowFormUpdateInfo(dataGridView1.SelectedRows.Cast<DataGridViewRow>()
                        .Select(u => Table.SelectInfo().First(j => j.ID == Convert.ToInt32(u.Cells[0].Value))));
                    UpdateResGrid1();
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при изменении данных!");
            }
        }
        private void but1_Click(object sender, EventArgs e)
        {
            Table.ShowFormInsertInfo();
            UpdateResGrid1();
        }
        public void UpdateResGrid1()
        {
            label4.Text = Table.RUSNameTable;
            Table.PrintInfo(dataGridView1);
            label5.Text = "Кол-во записей: " + dataGridView1.Rows.Count.ToString();
            tabControl2.TabPages.Clear();
            foreach (var s in Table.WebTables)
                tabControl2.TabPages.Add(s.RUSNameTable);
            TableWeb = Table.WebTables.Count() != 0 ? Table.WebTables.First() : null;
            UpdateResGrid2();
        }
        public void UpdateResGrid2()
        {
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();
            if (TableWeb != null)
            {
                if (dataGridView1.SelectedRows.Count != 0)
                    TableWeb.PrintInfo(dataGridView2, Table.SelectInfo().First(u => u.ID == Convert.ToInt32(dataGridView1.SelectedRows[dataGridView1.SelectedRows.Count - 1].Cells[0].Value)));
                label6.Text = TableWeb.RUSNameTable;
            }
            else
                label6.Text = "Связующая таблица";
            label7.Text = "Кол-во записей: " + dataGridView2.Rows.Count.ToString();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            UpdateResGrid2();
        }
        private void TabControl1_Selected(object sender, TabControlEventArgs e)
        {
            Table = NGDB.List[e.TabPage.Text];
            UpdateResGrid1();
        }
        private void TabControl2_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage != null)
            {
                TableWeb = NGDB.List[e.TabPage.Text];
                UpdateResGrid2();
            }
        }
        private void Item12_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы точно желаете сменить пользователя?", "Сообщение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DialogResult = DialogResult.Yes;
                Close();
            }
        }
        private void Item11_Click(object sender, EventArgs e)
        {
            new Options().ShowDialog();
            UpdateResGrid1();
        }
        private void Item13_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToString();
        }
    }
}
