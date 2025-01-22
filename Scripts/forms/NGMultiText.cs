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
    public partial class NGMultiText : Form
    {
        public NGMultiText()
        {
            InitializeComponent();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            NGOptions.StackObjects.Push(textBox1.Text);
            DialogResult = DialogResult.Yes;
            Close();
        }
    }
}
