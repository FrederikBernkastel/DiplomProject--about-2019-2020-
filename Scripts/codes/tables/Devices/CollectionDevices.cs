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
    public partial class CollectionDevices : BaseTable
    {
        public override string RUSNameTable { get { return "Устройства"; } }
        public override string ENGNameTable { get { return "devices"; } }
        public override IEnumerable<string> RUSFieldFull { get { return new string[]
        {
            "ID",
            "Название",
            "Тех. характеристики",
            "ТипУстройстваID",
            "Инвентарный номер",
            "Серийный номер",
            "Стоимость",
            "СотрудникID",
            "Дата покупки",
            "Гарантия(мес)",
            "Статус",
            "ПоставщикID",
            "Уч. корпус",
            "Кабинет",
        }; } }
        public override IEnumerable<string> ENGFieldFull { get { return new string[]
        {
            "id",
            "_name",
            "characteristics",
            "type_device_id",
            "inventory_number",
            "serial_number",
            "cost",
            "employee_id",
            "purchase_date",
            "gurantee",
            "status_device",
            "provider_id",
            "campus",
            "cabinet",
        }; } }
        public override IEnumerable<string> TypesFull { get { return new string[]
        {
            "INTEGER",
            "TEXT",
            "TEXT",
            "INTEGER",
            "TEXT",
            "TEXT",
            "REAL",
            "INTEGER",
            "TEXT",
            "INTEGER",
            "TEXT",
            "INTEGER",
            "INTEGER",
            "INTEGER",
        }; } }
        public override IEnumerable<string> RUSPrintField { get { return new string[]
        {
            "ID",
            "Название",
            "Тех. характеристики",
            "Тип устройства",
            "Инвентарный номер",
            "Серийный номер",
            "Стоимость",
            "Дата покупки",
            "Гарантия",
            "Срок гарантии(мес)",
            "Статус",
            "Уч. корпус",
            "Кабинет",
        }; } }
    }
    public partial class CollectionDevices : BaseTable
    {
        public override void ShowFormInsertInfo()
        {
            new IDevices().ShowDialog();
        }
        public override void ShowFormUpdateInfo(IEnumerable<BaseData> indexes)
        {
            new UDevices(indexes.Cast<Device>()).ShowDialog();
        }
        public override void Default()
        {
            base.Default();
        }
        public override IEnumerable<BaseData> FilterPrint(IEnumerable<string> str)
        {
            var sdf = str.ToArray();
            float tyu;
            switch (sdf[0])
            {
                case "ПО":
                    return NGDB.List[sdf[0]].SelectInfo().Cast<PO>().Where(u =>
                        sdf[1] == "Название" ? 
                            sdf[2] == "Равно" ? u.Name.CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.Name.CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? u.Name.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? u.Name.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Содержит" ? u.Name.Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.Name.Contains(sdf[3]) : true :
                        sdf[1] == "Стоимость" ? 
                            sdf[2] == "Равно" ? u.Cost.ToString().CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.Cost.ToString().CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? float.TryParse(sdf[3], out tyu) ? u.Cost.CompareTo(float.Parse(sdf[3])) > 0 : u.Cost.ToString().CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? float.TryParse(sdf[3], out tyu) ? u.Cost.CompareTo(float.Parse(sdf[3])) < 0 : u.Cost.ToString().CompareTo(sdf[3]) < 0 :
                            sdf[2] == "Содержит" ? u.Cost.ToString().Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.Cost.ToString().Contains(sdf[3]) : true :
                        sdf[1] == "Регистрационный ключ" ? 
                            sdf[2] == "Равно" ? u.RegistrationKey.CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.RegistrationKey.CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? u.RegistrationKey.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? u.RegistrationKey.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Содержит" ? u.RegistrationKey.Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.RegistrationKey.Contains(sdf[3]) : true :
                        sdf[1] == "Дата покупки" ? 
                            sdf[2] == "Равно" ? u.PurchaseDate.Date.ToString().CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.PurchaseDate.Date.ToString().CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? NGExtens.isToDateTime(sdf[3]) ? u.PurchaseDate.Date > NGExtens.ToDateTime(sdf[3]).Date : u.PurchaseDate.Date.ToString().CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? NGExtens.isToDateTime(sdf[3]) ? u.PurchaseDate.Date < NGExtens.ToDateTime(sdf[3]).Date : u.PurchaseDate.Date.ToString().CompareTo(sdf[3]) < 0 :
                            sdf[2] == "Содержит" ? u.PurchaseDate.Date.ToString().Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.PurchaseDate.Date.ToString().Contains(sdf[3]) : true :
                        sdf[1] == "Гарантия(мес)" ? 
                            sdf[2] == "Равно" ? u.Gurantee.ToString().CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.Gurantee.ToString().CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? float.TryParse(sdf[3], out tyu) ? u.Gurantee.CompareTo(float.Parse(sdf[3])) > 0 : u.Gurantee.ToString().CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? float.TryParse(sdf[3], out tyu) ? u.Gurantee.CompareTo(float.Parse(sdf[3])) < 0 : u.Gurantee.ToString().CompareTo(sdf[3]) < 0 :
                            sdf[2] == "Содержит" ? u.Gurantee.ToString().Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.Gurantee.ToString().Contains(sdf[3]) : true :
                        sdf[1] == "Статус" ? 
                            sdf[2] == "Равно" ? u.StatusPO.CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.StatusPO.CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? u.StatusPO.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? u.StatusPO.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Содержит" ? u.StatusPO.Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.StatusPO.Contains(sdf[3]) : true :
                        sdf[1] == "Правовой статус" ? 
                            sdf[2] == "Равно" ? sdf[3] == "Лицензионное" ? u.Cost != 0 : sdf[3] == "Нелицензионное" ? u.Cost == 0 : true :
                            sdf[2] == "Не равно" ? sdf[3] == "Лицензионное" ? u.Cost == 0 : sdf[3] == "Нелицензионное" ? u.Cost != 0 : true :
                            sdf[2] == "Больше" ? true :
                            sdf[2] == "Меньше" ? true :
                            sdf[2] == "Содержит" ? true :
                            sdf[2] == "Не содержит" ? true : true : true).Where(u => u.Device != null).Select(u => u.Device);
                case "Сотрудники":
                    return SelectInfo().Cast<Device>().Where(u => u.Employee == null ? false :
                        sdf[1] == "ФИО" ?
                            sdf[2] == "Равно" ? u.Employee.Name.CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.Employee.Name.CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? u.Employee.Name.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? u.Employee.Name.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Содержит" ? u.Employee.Name.Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.Employee.Name.Contains(sdf[3]) : true :
                        sdf[1] == "Должность" ?
                            sdf[2] == "Равно" ? u.Employee.Position.CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.Employee.Position.CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? u.Employee.Position.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? u.Employee.Position.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Содержит" ? u.Employee.Position.Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.Employee.Position.Contains(sdf[3]) : true : true);    
                case "Поставщики":
                    return SelectInfo().Cast<Device>().Where(u => u.Provider == null ? false :
                        sdf[1] == "Название фирмы" ?
                            sdf[2] == "Равно" ? u.Provider.Name.CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.Provider.Name.CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? u.Provider.Name.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? u.Provider.Name.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Содержит" ? u.Provider.Name.Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.Provider.Name.Contains(sdf[3]) : true :
                        sdf[1] == "ФИО директора" ?
                            sdf[2] == "Равно" ? u.Provider.FullName.CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.Provider.FullName.CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? u.Provider.FullName.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? u.Provider.FullName.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Содержит" ? u.Provider.FullName.Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.Provider.FullName.Contains(sdf[3]) : true :
                        sdf[1] == "Расчётный счёт" ?
                            sdf[2] == "Равно" ? u.Provider.CheckingAccount.CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.Provider.CheckingAccount.CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? u.Provider.CheckingAccount.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? u.Provider.CheckingAccount.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Содержит" ? u.Provider.CheckingAccount.Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.Provider.CheckingAccount.Contains(sdf[3]) : true :
                        sdf[1] == "ИНН" ?
                            sdf[2] == "Равно" ? u.Provider.INN.CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.Provider.INN.CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? u.Provider.INN.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? u.Provider.INN.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Содержит" ? u.Provider.INN.Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.Provider.INN.Contains(sdf[3]) : true :
                        sdf[1] == "Номер телефона" ?
                            sdf[2] == "Равно" ? u.Provider.Phone.CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.Provider.Phone.CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? u.Provider.Phone.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? u.Provider.Phone.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Содержит" ? u.Provider.Phone.Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.Provider.Phone.Contains(sdf[3]) : true :
                        sdf[1] == "E-Mail" ?
                            sdf[2] == "Равно" ? u.Provider.E_Mail.CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.Provider.E_Mail.CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? u.Provider.E_Mail.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? u.Provider.E_Mail.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Содержит" ? u.Provider.E_Mail.Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.Provider.E_Mail.Contains(sdf[3]) : true :
                        sdf[1] == "Адрес" ?
                            sdf[2] == "Равно" ? u.Provider.Address.CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.Provider.Address.CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? u.Provider.Address.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? u.Provider.Address.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Содержит" ? u.Provider.Address.Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.Provider.Address.Contains(sdf[3]) : true : true);
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
                    if (NGExtens.ToDateTime(s[7].ToString()).AddMonths(int.Parse(s[9].ToString())).Date < DateTime.Now.Date)
                        view.Rows[view.Rows.Count - 1].Cells["Гарантия"].Style.BackColor = Color.OrangeRed;
                    else
                        view.Rows[view.Rows.Count - 1].Cells["Гарантия"].Style.BackColor = Color.Yellow;
                }
            else
                foreach (var s in data)
                    view.Rows.Add(s);
            view.Columns[0].Visible = false;
        }
        public override void Load()
        {
            WebTables = new BaseTable[]
            {
                NGDB.List["Сотрудники"],
                NGDB.List["Поставщики"],
                NGDB.List["ПО"],
            };
        }
        public override void PrintInfo(DataGridView view, IEnumerable<BaseData> list)
        {
            PrintInfo(view, RUSPrintField, list.Cast<Device>().Select(u => new string[]
            {
                u.ID.ToString(),
                u.Name,
                u.Characteristics,
                u.TypeDevice != null ? u.TypeDevice.Name : "",
                u.InventoryNumber,
                u.SerialNumber,
                u.Cost.ToString("0.00"),
                u.PurchaseDate.Day.ToString("00") + "." + u.PurchaseDate.Month.ToString("00") + "." + u.PurchaseDate.Year,
                u.PurchaseDate.AddMonths(u.Gurantee).Date < DateTime.Now.Date ? "Срок истёк" : "На гарантии",
                u.Gurantee.ToString(),
                u.StatusDevice,
                u.Campus.ToString(),
                u.Cabinet.ToString(),
            }));
        }
        public override IEnumerable<BaseData> SelectInfo()
        {
            foreach (DataRow s in NGDB.SelectInfo(ENGNameTable))
                yield return new Device().SetDataRow(s);
        }
    }
}
