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
    public partial class AU : Form
    {
        public AU()
        {
            InitializeComponent();
            checkBox1.CheckedChanged += CheckBox1_CheckedChanged;
            textBox2.UseSystemPasswordChar = true;
            toolTip1.SetToolTip(checkBox1, "Вкл/Выкл Мнемоника");
            Load += AU_Load;
        }
        private void AU_Load(object sender, EventArgs e)
        {
            if (NGOptions.isSave)
            {
                Hide();
                if (new General().ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                {
                    Show();
                    NGOptions.isSave = false;
                }
                else
                    Application.Exit();
            }
        }
        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = !textBox2.UseSystemPasswordChar;
        }
        public void Clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (isValid())
            {
                if (textBox1.Text == "admin" && textBox2.Text == "admin")
                {
                    NGOptions.isAdmin = true;
                    Clear();
                    Hide();
                    if (new General().ShowDialog() == DialogResult.Yes)
                    {
                        Show();
                        NGOptions.isSave = false;
                    }
                    else
                        Application.Exit();
                }
                else
                {
                    NGOptions.isAdmin = false;
                    Clear();
                    Hide();
                    if (new General().ShowDialog() == DialogResult.Yes)
                    {
                        Show();
                        NGOptions.isSave = false;
                    }
                    else
                        Application.Exit();
                }
            }
        }
        public bool isValid()
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Поля должны быть заполнены!");
                return false;
            }
            else if (
                textBox1.Text != "admin" && textBox2.Text != "admin" &&
                textBox1.Text != "user" && textBox2.Text != "user")
            {
                MessageBox.Show("Неправильный логин или пароль!");
                return false;
            }
            else
                return true;
        }
    }
}
