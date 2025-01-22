using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DipProject
{
    [Serializable]
    public class NG_Options
    {
        public bool isSave { get; set; }
        public bool isAdmin { get; set; }
        public string[] List { get; set; }
        public bool isIFFormat { get; set; }
    }
    public static class NGOptions
    {
        public static bool isSave;
        public static bool isIFFormat;
        public static bool isAdmin;
        public static Stack<string> List = new Stack<string>();
        public static Stack<object> StackObjects = new Stack<object>();
        public static void ImportOptions()
        {
            if (File.Exists("options.ng"))
            {
                NG_Options options = NGExtens.Deserialization(new NGExtens.SerializationOptions("options.ng", null)) as NG_Options;
                isSave = options.isSave;
                isAdmin = options.isAdmin;
                List = new Stack<string>(options.List);
                isIFFormat = options.isIFFormat;
            }
            else
                Default();
        }
        public static void ExportOptions()
        {
            NGExtens.Serialization = new NGExtens.SerializationOptions("options.ng", new NG_Options
            {
                isSave = isSave,
                isAdmin = isAdmin,
                List = List.ToArray(),
                isIFFormat = isIFFormat,
            });
        }
        public static void Default()
        {
            List = new Stack<string>(NGDB.List.Select(u => u.ENGNameTable));
            isSave = false;
            isIFFormat = false;
            isAdmin = false;
        }
    }
}
