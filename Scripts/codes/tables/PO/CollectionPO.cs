using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DipProject
{
    public partial class CollectionPO : BaseTable
    {
        public override string RUSNameTable { get { return "ПО"; } }
        public override string ENGNameTable { get { return "po"; } }
        public override IEnumerable<string> RUSFieldFull { get { return new string[]
        {
            "ID",
            "Название",
            "Стоимость",
            "УстройствоID",
            "Регистрационный ключ",
            "Дата покупки",
            "Гарантия(мес)",
            "Статус",
        }; } }
        public override IEnumerable<string> ENGFieldFull { get { return new string[]
        {
            "id",
            "_name",
            "cost",
            "device_id",
            "registration_key",
            "purchase_date",
            "gurantee",
            "status_po",
        }; } }
        public override IEnumerable<string> TypesFull { get { return new string[]
        {
            "INTEGER",
            "TEXT",
            "REAL",
            "INTEGER",
            "TEXT",
            "TEXT",
            "INTEGER",
            "TEXT",
        }; } }
        public override IEnumerable<string> RUSPrintField { get { return new string[]
        {
            "ID",
            "Название",
            "Стоимость",
            "Регистрационный ключ",
            "Дата покупки",
            "Гарантия(мес)",
            "Статус",
            "Правовой статус",
        }; } }
    }
    public partial class CollectionPO : BaseTable
    {
        public override void ShowFormInsertInfo()
        {
            new IPO().ShowDialog();
        }
        public override void ShowFormUpdateInfo(IEnumerable<BaseData> indexes)
        {
            new UPO(indexes.Cast<PO>()).ShowDialog();
        }
        public override void Default()
        {
            base.Default();
        }
        public override IEnumerable<BaseData> FilterPrint(IEnumerable<string> str)
        {
            var sdf = str.ToArray();
            float tyu;
            int jnb;
            switch (sdf[0])
            {
                case "Устройства":
                    return SelectInfo().Cast<PO>().Where(u => u.Device == null ? false :
                        sdf[1] == "Название" ?
                            sdf[2] == "Равно" ? u.Device.Name.CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.Device.Name.CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? u.Device.Name.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? u.Device.Name.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Содержит" ? u.Device.Name.Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.Device.Name.Contains(sdf[3]) : true :
                        sdf[1] == "Тех. характеристики" ?
                            sdf[2] == "Равно" ? u.Device.Characteristics.CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.Device.Characteristics.CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? u.Device.Characteristics.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? u.Device.Characteristics.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Содержит" ? u.Device.Characteristics.Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.Device.Characteristics.Contains(sdf[3]) : true :
                        sdf[1] == "Тип устройства" ?
                            sdf[2] == "Равно" ? u.Device.TypeDevice.Name.CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.Device.TypeDevice.Name.CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? u.Device.TypeDevice.Name.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? u.Device.TypeDevice.Name.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Содержит" ? u.Device.TypeDevice.Name.Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.Device.TypeDevice.Name.Contains(sdf[3]) : true :
                        sdf[1] == "Инвентарный номер" ?
                            sdf[2] == "Равно" ? u.Device.InventoryNumber.CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.Device.InventoryNumber.CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? u.Device.InventoryNumber.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? u.Device.InventoryNumber.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Содержит" ? u.Device.InventoryNumber.Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.Device.InventoryNumber.Contains(sdf[3]) : true :
                        sdf[1] == "Серийный номер" ?
                            sdf[2] == "Равно" ? u.Device.SerialNumber.CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.Device.SerialNumber.CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? u.Device.SerialNumber.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? u.Device.SerialNumber.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Содержит" ? u.Device.SerialNumber.Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.Device.SerialNumber.Contains(sdf[3]) : true :
                        sdf[1] == "Стоимость" ?
                            sdf[2] == "Равно" ? u.Device.Cost.ToString().CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.Device.Cost.ToString().CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? float.TryParse(sdf[3], out tyu) ? u.Device.Cost.CompareTo(float.Parse(sdf[3])) > 0 : u.Device.Cost.ToString().CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? float.TryParse(sdf[3], out tyu) ? u.Device.Cost.CompareTo(float.Parse(sdf[3])) < 0 : u.Device.Cost.ToString().CompareTo(sdf[3]) < 0 :
                            sdf[2] == "Содержит" ? u.Device.Cost.ToString().Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.Device.Cost.ToString().Contains(sdf[3]) : true :
                        sdf[1] == "Дата покупки" ?
                            sdf[2] == "Равно" ? u.Device.PurchaseDate.Date.ToString().CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.Device.PurchaseDate.Date.ToString().CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? NGExtens.isToDateTime(sdf[3]) ? u.Device.PurchaseDate.Date > NGExtens.ToDateTime(sdf[3]).Date : u.Device.PurchaseDate.Date.ToString().CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? NGExtens.isToDateTime(sdf[3]) ? u.Device.PurchaseDate.Date < NGExtens.ToDateTime(sdf[3]).Date : u.Device.PurchaseDate.Date.ToString().CompareTo(sdf[3]) < 0 :
                            sdf[2] == "Содержит" ? u.Device.PurchaseDate.Date.ToString().Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.Device.PurchaseDate.Date.ToString().Contains(sdf[3]) : true :
                        sdf[1] == "Срок гарантии(мес)" ?
                            sdf[2] == "Равно" ? u.Device.Gurantee.ToString().CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.Device.Gurantee.ToString().CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? float.TryParse(sdf[3], out tyu) ? u.Device.Gurantee.CompareTo(float.Parse(sdf[3])) > 0 : u.Device.Gurantee.ToString().CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? float.TryParse(sdf[3], out tyu) ? u.Device.Gurantee.CompareTo(float.Parse(sdf[3])) < 0 : u.Device.Gurantee.ToString().CompareTo(sdf[3]) < 0 :
                            sdf[2] == "Содержит" ? u.Device.Gurantee.ToString().Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.Device.Gurantee.ToString().Contains(sdf[3]) : true :
                        sdf[1] == "Статус" ?
                            sdf[2] == "Равно" ? u.Device.StatusDevice.CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.Device.StatusDevice.CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? u.Device.StatusDevice.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? u.Device.StatusDevice.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Содержит" ? u.Device.StatusDevice.Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.Device.StatusDevice.Contains(sdf[3]) : true :
                        sdf[1] == "Уч. корпус" ?
                            sdf[2] == "Равно" ? u.Device.Campus.ToString().CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.Device.Campus.ToString().CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? int.TryParse(sdf[3], out jnb) ? u.Device.Campus.CompareTo(int.Parse(sdf[3])) > 0 : u.Device.Campus.ToString().CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? int.TryParse(sdf[3], out jnb) ? u.Device.Campus.CompareTo(int.Parse(sdf[3])) < 0 : u.Device.Campus.ToString().CompareTo(sdf[3]) < 0 :
                            sdf[2] == "Содержит" ? u.Device.Campus.ToString().Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.Device.Campus.ToString().Contains(sdf[3]) : true :
                        sdf[1] == "Кабинет" ?
                            sdf[2] == "Равно" ? u.Device.Cabinet.ToString().CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.Device.Cabinet.ToString().CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? int.TryParse(sdf[3], out jnb) ? u.Device.Cabinet.CompareTo(int.Parse(sdf[3])) > 0 : u.Device.Cabinet.ToString().CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? int.TryParse(sdf[3], out jnb) ? u.Device.Cabinet.CompareTo(int.Parse(sdf[3])) < 0 : u.Device.Cabinet.ToString().CompareTo(sdf[3]) < 0 :
                            sdf[2] == "Содержит" ? u.Device.Cabinet.ToString().Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.Device.Cabinet.ToString().Contains(sdf[3]) : true : true);
                default:
                    return Enumerable.Empty<BaseData>();
            }
        }
        public override void PrintInfo(DataGridView view, IEnumerable<string> names, IEnumerable<object[]> data)
        {
            view.Rows.Clear();
            view.Columns.Clear();
            foreach (var s in names)
                view.Columns.Add(s, s);
            if (NGOptions.isIFFormat)
                foreach (var s in data)
                {
                    view.Rows.Add(s);
                    if (s[6].ToString() == "Срок истёк")
                        view.Rows[view.Rows.Count - 1].Cells["Статус"].Style.BackColor = Color.OrangeRed;
                    else if (s[6].ToString() == "Действительно")
                        view.Rows[view.Rows.Count - 1].Cells["Статус"].Style.BackColor = Color.Yellow;
                }
            else
                foreach (var s in data)
                    view.Rows.Add(s);
            view.Columns[0].Visible = false;
        }
        public override void Update()
        {
            foreach (var s in SelectInfo().Cast<PO>().Where(u => u.Gurantee != 0 && u.PurchaseDate.AddMonths(u.Gurantee) < DateTime.Now))
            {
                s.StatusPO = "Срок истёк";
                UpdateInfo(s.NGToString());
            }
        }
        public override void Load()
        {
            WebTables = new BaseTable[]
            {
                NGDB.List["Устройства"],
            };
        }
        public override void PrintInfo(DataGridView view, IEnumerable<BaseData> list)
        {
            PrintInfo(view, RUSPrintField, list.Cast<PO>().Select(u => new string[] 
            {
                u.ID.ToString(),
                u.Name,
                u.Cost.ToString("0.00"),
                u.RegistrationKey,
                u.PurchaseDate.Day.ToString("00") + "." + u.PurchaseDate.Month.ToString("00") + "." + u.PurchaseDate.Year,
                u.Gurantee.ToString(),
                u.StatusPO,
                u.Cost == 0 ? "Нелицензионное" : "Лицензионное",
            }));
        }
        public override IEnumerable<BaseData> SelectInfo()
        {
            foreach (DataRow s in NGDB.SelectInfo(ENGNameTable))
                yield return new PO().SetDataRow(s);
        }
    }
}
