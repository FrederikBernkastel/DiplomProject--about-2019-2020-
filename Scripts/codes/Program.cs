using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DipProject
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                NGOptions.ImportOptions();
                NGDB.Load();
                foreach (var s in NGDB.List)
                    s.Load();
                Application.Run(new AU());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            NGExit();
        }
        public static void NGExit()
        {
            NGOptions.ExportOptions();
            NGDB.Dispose();
        }
    }
}
