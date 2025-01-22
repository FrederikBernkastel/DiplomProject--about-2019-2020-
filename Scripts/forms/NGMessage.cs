using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DipProject
{
    public partial class NGMessage : Form
    {
        public Action Action;
        public NGMessage(Action action)
        {
            InitializeComponent();
            Action = action;
            Shown += NGMessage_Shown;
            timer1.Interval = 1000;
        }
        private void NGMessage_Shown(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            Action();
            Close();
        }
    }
}
