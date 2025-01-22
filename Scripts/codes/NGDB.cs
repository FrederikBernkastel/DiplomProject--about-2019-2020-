using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;

namespace DipProject
{
    public partial class NGListBaseTable
    {
        public IEnumerable<BaseTable> List { get; set; }
    }
    public partial class NGListBaseTable : IEnumerable<BaseTable>
    {
        public BaseTable this[string obj]
        {
            get
            {
                return List.FirstOrDefault(u => u.RUSNameTable == obj || u.ENGNameTable == obj);
            }
        }
        public IEnumerator<BaseTable> GetEnumerator()
        {
            return List.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
    public static partial class NGDB
    {
        public static SQLiteConnection Connection = new SQLiteConnection("Data Source=db.sqlite;Version=3;");
        public static NGListBaseTable List = new NGListBaseTable
        {
            List = new BaseTable[]
            {
                new CollectionDevices(),
                new CollectionEmployee(),
                new CollectionPO(),
                new CollectionTypeDevice(),
                new CollectionSuppliers(),
                new CollectionCabinetEmployee(),
            },
        };
    }
    public static partial class NGDB
    {
        public static void Load()
        {
            bool life = false;
            if (File.Exists("db.sqlite"))
                life = true;
            Connection.Open();
            if (!life)
                CreateDB();
        }
        public static void Update()
        {
            foreach (var s in List)
                s.Update();
        }
        public static void Dispose()
        {
            Connection.Close();
        }
        public static void InsertInfo(string table_name, string values)
        {
            new SQLiteCommand("INSERT INTO " + table_name + " VALUES(NULL, " + values + ");", Connection).ExecuteNonQuery();
        }
        public static int Count(string table_name)
        {
            return int.Parse(new SQLiteCommand("SELECT COUNT(*) FROM " + table_name + ";", Connection).ExecuteScalar().ToString());
        }
        public static DataRowCollection SelectInfo(string table_name)
        {
            DataTable table = new DataTable();
            new SQLiteDataAdapter("SELECT * FROM " + table_name + ";", Connection).Fill(table);
            return table.Rows;
        }
        public static void UpdateInfo(string table_name, string values, string wherE)
        {
            new SQLiteCommand("UPDATE " + table_name + " SET " + values + " " + wherE + ";", Connection).ExecuteNonQuery();
        }
        public static void RemoveInfo(string table_name, string wherE)
        {
            new SQLiteCommand("DELETE FROM " + table_name + " " + wherE + ";", Connection).ExecuteNonQuery();
        }
        public static void CreateDB()
        {
            foreach (var s in List)
            {
                new SQLiteCommand(s.CreateTableString(), Connection).ExecuteNonQuery();
                s.Default();
            }
        }
    }
    public static class NGExtens
    {
        public static DateTime ToDateTime(string str)
        {
            string[] temp2 = str.Split(' ')[0].Split('.');
            return new DateTime(
                int.Parse(temp2[2]),
                int.Parse(temp2[1]),
                int.Parse(temp2[0]));
        }
        public static bool isToDateTime(string str)
        {
            try
            {
                var temp = ToDateTime(str);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static IEnumerable<DataGridViewRow> GetManyRowsFilter(IEnumerable<DataGridViewRow> rows1, IEnumerable<DataGridViewRow> rows2, string operand)
        {
            switch (operand)
            {
                case "И":
                    return rows1.Intersect(rows2);
                case "ИЛИ":
                    return rows1.Union(rows2).Distinct();
                default:
                    return Enumerable.Empty<DataGridViewRow>();
            }
        }
        public static void GridUpdate(DataGridView grid, DataGridViewRow[] rows)
        {
            foreach (var s in rows)
                grid.Rows.Add(new Func<DataGridViewCellCollection, object[]>(u =>
                {
                    object[] vs = new object[u.Count];
                    for (int i = 0; i < vs.Length; i++)
                        vs[i] = u[i].Value;
                    return vs;
                }).Invoke(s.Cells));
            for (int i = 0, j = grid.Rows.Count; i < j - rows.Count(); i++)
                grid.Rows.RemoveAt(0);
        }
        public static IEnumerable<DataGridViewRow> GetRowsFilter(DataGridView grid, string[] pattern)
        {
            Func<DataGridViewRow, bool> FINC = default(Func<DataGridViewRow, bool>);
            switch (pattern[1])
            {
                case "Равно":
                    FINC = new Func<DataGridViewRow, bool>(u =>
                        u.Cells[grid.Columns[pattern[0]].Index].Value.ToString().CompareTo(pattern[2]) == 0);
                    break;
                case "Не равно":
                    FINC = new Func<DataGridViewRow, bool>(u =>
                        u.Cells[grid.Columns[pattern[0]].Index].Value.ToString().CompareTo(pattern[2]) != 0);
                    break;
                case "Больше":
                    FINC = new Func<DataGridViewRow, bool>(u =>
                    {
                        string vars = u.Cells[grid.Columns[pattern[0]].Index].Value.ToString();
                        float im;
                        return
                            float.TryParse(vars, out im) && float.TryParse(pattern[2], out im) ? float.Parse(vars).CompareTo(float.Parse(pattern[2])) > 0 :
                            isToDateTime(vars) && isToDateTime(pattern[2]) ? ToDateTime(vars) > ToDateTime(pattern[2]) : vars.CompareTo(pattern[2]) > 0;
                    });
                    break;
                case "Меньше":
                    FINC = new Func<DataGridViewRow, bool>(u =>
                    {
                        string vars = u.Cells[grid.Columns[pattern[0]].Index].Value.ToString();
                        float im;
                        return
                            float.TryParse(vars, out im) && float.TryParse(pattern[2], out im) ? float.Parse(vars).CompareTo(float.Parse(pattern[2])) < 0 :
                            isToDateTime(vars) && isToDateTime(pattern[2]) ? ToDateTime(vars) < ToDateTime(pattern[2]) : vars.CompareTo(pattern[2]) < 0;
                    });
                    break;
                case "Содержит":
                    FINC = new Func<DataGridViewRow, bool>(u =>
                        u.Cells[grid.Columns[pattern[0]].Index].Value.ToString().Contains(pattern[2]));
                    break;
                case "Не содержит":
                    FINC = new Func<DataGridViewRow, bool>(u =>
                        !u.Cells[grid.Columns[pattern[0]].Index].Value.ToString().Contains(pattern[2]));
                    break;
            }
            return grid.Rows.Cast<DataGridViewRow>().Where(FINC);
        }
        public static string NGToString(this string str)
        {
            return "'" + str + "'";
        }
        public static string GetMD5Hash(string text)
        {
            using (var hashAlg = MD5.Create())
            {
                byte[] hash = hashAlg.ComputeHash(Encoding.UTF8.GetBytes(text));
                var builder = new StringBuilder(hash.Length * 2);
                for (int i = 0; i < hash.Length; i++)
                    builder.Append(hash[i].ToString("X2"));
                return builder.ToString();
            }
        }
        public static SerializationOptions Serialization
        {
            set
            {
                using (FileStream fs = new FileStream(value.Path, FileMode.OpenOrCreate))
                {
                    new BinaryFormatter().Serialize(fs, value.Objects);
                }
            }
        }
        public static object Deserialization(SerializationOptions options)
        {
            object obj;
            using (FileStream fs = new FileStream(options.Path, FileMode.OpenOrCreate))
            {
                obj = new BinaryFormatter().Deserialize(fs);
            }
            return obj;
        }
        public struct SerializationOptions
        {
            public string Path;
            public object Objects;
            public SerializationOptions(string path, object objects)
            {
                Path = path;
                Objects = objects;
            }
        }
    }
}
