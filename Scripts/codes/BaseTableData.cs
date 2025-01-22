using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DipProject
{
    public abstract partial class BaseTable
    {
        public abstract string RUSNameTable { get; }
        public abstract string ENGNameTable { get; }
        public abstract IEnumerable<string> RUSFieldFull { get; }
        public abstract IEnumerable<string> ENGFieldFull { get; }
        public abstract IEnumerable<string> TypesFull { get; }
        public abstract IEnumerable<string> RUSPrintField { get; }
        public IEnumerable<BaseTable> WebTables = Enumerable.Empty<BaseTable>();
    }
    public abstract partial class BaseTable
    {
        public virtual void Load() { }
        public void RemoveInfo(int index)
        {
            NGDB.RemoveInfo(ENGNameTable, "WHERE id = " + index);
        }
        public void RemoveInfo()
        {
            NGDB.RemoveInfo(ENGNameTable, "");
        }
        public virtual void PrintInfo(DataGridView view, IEnumerable<string> names, IEnumerable<object[]> data)
        {
            view.Rows.Clear();
            view.Columns.Clear();
            foreach (var s in names)
                view.Columns.Add(s, s);
            foreach (var s in data)
                view.Rows.Add(s);
            view.Columns[0].Visible = false;
        }
        public abstract void PrintInfo(DataGridView view, IEnumerable<BaseData> list);
        public void PrintInfo(DataGridView view)
        {
            PrintInfo(view, SelectInfo());
        }
        public virtual IEnumerable<BaseData> FilterPrint(IEnumerable<string> str) { return Enumerable.Empty<BaseData>(); }
        public abstract IEnumerable<BaseData> SelectInfo();
        public virtual void ShowFormInsertInfo() { }
        public virtual void ShowFormUpdateInfo(IEnumerable<BaseData> obj) { }
        public void ShowFormRemoveInfo(IEnumerable<int> obj)
        {
            if (MessageBox.Show("Вы точно желаете удалить выделенные записи?", 
                "Сообщение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                foreach (var s in obj)
                    RemoveInfo(s);
        }
        public virtual void Default()
        {
            RemoveInfo();
        }
        public int Count()
        {
            return NGDB.Count(ENGNameTable);
        }
        public virtual void Update() { }
        public string CreateTableString()
        {
            var vs = ENGFieldFull.Zip(TypesFull, (field, types) => field + " " + types);
            return string.Format("CREATE TABLE {0} ({1});", 
                ENGNameTable, vs.First() + " PRIMARY KEY AUTOINCREMENT," + string.Join(",", vs.Skip(1)));
        }
        public void InsertInfo(IEnumerable<string> obj)
        {
            NGDB.InsertInfo(ENGNameTable, string.Join(",", obj.Skip(1)));
        }
        public void UpdateInfo(IEnumerable<string> obj)
        {
            NGDB.UpdateInfo(ENGNameTable, string.Join(",", ENGFieldFull.Skip(1).Zip(obj.Skip(1), 
                (field, objs) => field + "=" + objs)), "WHERE id=" + obj.First());
        }
        public void PrintInfo(DataGridView view, BaseData data)
        {
            PrintInfo(view, data.WebFilter(this));
        }
    }
    public abstract partial class BaseData
    {
        public int ID { get; set; }
    }
    public abstract partial class BaseData
    {
        public abstract IEnumerable<BaseData> WebFilter(BaseTable obj);
        public abstract IEnumerable<string> NGToString();
        public abstract string SerializeJSON();
        public abstract BaseData SetDataRow(DataRow obj);
    }
}
