using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DipProject
{
    public partial class CollectionCabinetEmployee : BaseTable
    {
        public override string RUSNameTable { get { return "Кабинеты"; } }
        public override string ENGNameTable { get { return "cabinet_employee"; } }
        public override IEnumerable<string> RUSFieldFull { get { return new string[] 
        {
            "ID",
            "Уч. корпус",
            "Кабинет",
            "СотрудникID",
        }; } }
        public override IEnumerable<string> ENGFieldFull { get { return new string[]
        {
            "id",
            "campus",
            "cabinet",
            "employee_id",
        }; } }
        public override IEnumerable<string> TypesFull { get { return new string[]
        {
            "INTEGER",
            "INTEGER",
            "INTEGER",
            "INTEGER",
        }; } }
        public override IEnumerable<string> RUSPrintField { get { return new string[]
        {
            "ID",
            "Уч. корпус",
            "Кабинет",
            "ФИО",
        }; } }
    }
    public partial class CollectionCabinetEmployee : BaseTable
    {
        public override void ShowFormInsertInfo()
        {
            new ICabinetEmployee().ShowDialog();
        }
        public override void ShowFormUpdateInfo(IEnumerable<BaseData> obj)
        {
            new UCabinetEmployee(obj.Cast<CabinetEmployee>()).ShowDialog();
        }
        public override void Default()
        {
            base.Default();
        }
        public override void Update()
        {
            base.Update();
        }
        public override IEnumerable<BaseData> FilterPrint(IEnumerable<string> str)
        {
            var sdf = str.ToArray();
            float tyu;
            int jnb;
            switch (sdf[0])
            {
                case "Сотрудники":
                    return SelectInfo().Cast<CabinetEmployee>().Where(u => u.Employee == null ? false :
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
                case "Устройства":
                    return SelectInfo().Cast<Device>().Where(u =>
                        sdf[1] == "Название" ?
                            sdf[2] == "Равно" ? u.Name.CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.Name.CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? u.Name.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? u.Name.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Содержит" ? u.Name.Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.Name.Contains(sdf[3]) : true :
                        sdf[1] == "Тех. характеристики" ?
                            sdf[2] == "Равно" ? u.Characteristics.CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.Characteristics.CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? u.Characteristics.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? u.Characteristics.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Содержит" ? u.Characteristics.Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.Characteristics.Contains(sdf[3]) : true :
                        sdf[1] == "Тип устройства" ? u.TypeDevice == null ? false :
                            sdf[2] == "Равно" ? u.TypeDevice.Name.CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.TypeDevice.Name.CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? u.TypeDevice.Name.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? u.TypeDevice.Name.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Содержит" ? u.TypeDevice.Name.Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.TypeDevice.Name.Contains(sdf[3]) : true :
                        sdf[1] == "Инвентарный номер" ?
                            sdf[2] == "Равно" ? u.InventoryNumber.CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.InventoryNumber.CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? u.InventoryNumber.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? u.InventoryNumber.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Содержит" ? u.InventoryNumber.Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.InventoryNumber.Contains(sdf[3]) : true :
                        sdf[1] == "Серийный номер" ?
                            sdf[2] == "Равно" ? u.SerialNumber.CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.SerialNumber.CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? u.SerialNumber.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? u.SerialNumber.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Содержит" ? u.SerialNumber.Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.SerialNumber.Contains(sdf[3]) : true :
                        sdf[1] == "Стоимость" ?
                            sdf[2] == "Равно" ? u.Cost.ToString().CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.Cost.ToString().CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? float.TryParse(sdf[3], out tyu) ? u.Cost.CompareTo(float.Parse(sdf[3])) > 0 : u.Cost.ToString().CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? float.TryParse(sdf[3], out tyu) ? u.Cost.CompareTo(float.Parse(sdf[3])) < 0 : u.Cost.ToString().CompareTo(sdf[3]) < 0 :
                            sdf[2] == "Содержит" ? u.Cost.ToString().Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.Cost.ToString().Contains(sdf[3]) : true :
                        sdf[1] == "Дата покупки" ?
                            sdf[2] == "Равно" ? u.PurchaseDate.Date.ToString().CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.PurchaseDate.Date.ToString().CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? NGExtens.isToDateTime(sdf[3]) ? u.PurchaseDate.Date > NGExtens.ToDateTime(sdf[3]).Date : u.PurchaseDate.Date.ToString().CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? NGExtens.isToDateTime(sdf[3]) ? u.PurchaseDate.Date < NGExtens.ToDateTime(sdf[3]).Date : u.PurchaseDate.Date.ToString().CompareTo(sdf[3]) < 0 :
                            sdf[2] == "Содержит" ? u.PurchaseDate.Date.ToString().Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.PurchaseDate.Date.ToString().Contains(sdf[3]) : true :
                        sdf[1] == "Срок гарантии(мес)" ?
                            sdf[2] == "Равно" ? u.Gurantee.ToString().CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.Gurantee.ToString().CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? float.TryParse(sdf[3], out tyu) ? u.Gurantee.CompareTo(float.Parse(sdf[3])) > 0 : u.Gurantee.ToString().CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? float.TryParse(sdf[3], out tyu) ? u.Gurantee.CompareTo(float.Parse(sdf[3])) < 0 : u.Gurantee.ToString().CompareTo(sdf[3]) < 0 :
                            sdf[2] == "Содержит" ? u.Gurantee.ToString().Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.Gurantee.ToString().Contains(sdf[3]) : true :
                        sdf[1] == "Статус" ?
                            sdf[2] == "Равно" ? u.StatusDevice.CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.StatusDevice.CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? u.StatusDevice.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? u.StatusDevice.CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Содержит" ? u.StatusDevice.Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.StatusDevice.Contains(sdf[3]) : true :
                        sdf[1] == "Уч. корпус" ?
                            sdf[2] == "Равно" ? u.Campus.ToString().CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.Campus.ToString().CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? int.TryParse(sdf[3], out jnb) ? u.Campus.CompareTo(int.Parse(sdf[3])) > 0 : u.Campus.ToString().CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? int.TryParse(sdf[3], out jnb) ? u.Campus.CompareTo(int.Parse(sdf[3])) < 0 : u.Campus.ToString().CompareTo(sdf[3]) < 0 :
                            sdf[2] == "Содержит" ? u.Campus.ToString().Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.Campus.ToString().Contains(sdf[3]) : true :
                        sdf[1] == "Кабинет" ?
                            sdf[2] == "Равно" ? u.Cabinet.ToString().CompareTo(sdf[3]) == 0 :
                            sdf[2] == "Не равно" ? u.Cabinet.ToString().CompareTo(sdf[3]) != 0 :
                            sdf[2] == "Больше" ? int.TryParse(sdf[3], out jnb) ? u.Cabinet.CompareTo(int.Parse(sdf[3])) > 0 : u.Cabinet.ToString().CompareTo(sdf[3]) > 0 :
                            sdf[2] == "Меньше" ? int.TryParse(sdf[3], out jnb) ? u.Cabinet.CompareTo(int.Parse(sdf[3])) < 0 : u.Cabinet.ToString().CompareTo(sdf[3]) < 0 :
                            sdf[2] == "Содержит" ? u.Cabinet.ToString().Contains(sdf[3]) :
                            sdf[2] == "Не содержит" ? !u.Cabinet.ToString().Contains(sdf[3]) : true : true).Select(u => u.Campus.ToString() + " " + u.Cabinet.ToString())
                            .Distinct().Intersect(SelectInfo().Cast<CabinetEmployee>().Select(u => u.Campus.ToString() + " " + u.Cabinet.ToString()))
                            .Select(u => SelectInfo().Cast<CabinetEmployee>().First(j => j.Campus == int.Parse(u.Split(' ')[0]) && j.Cabinet == int.Parse(u.Split(' ')[1])));
                default:
                    return Enumerable.Empty<BaseData>();
            }
        }
        public override void Load()
        {
            WebTables = new BaseTable[]
            {
                NGDB.List["Устройства"],
                NGDB.List["Сотрудники"],
            };
        }
        public override void PrintInfo(DataGridView view, IEnumerable<BaseData> list)
        {
            PrintInfo(view, RUSPrintField, list.Cast<CabinetEmployee>().Select(u => new string[] 
            {
                u.ID.ToString(),
                u.Campus.ToString(),
                u.Cabinet.ToString(),
                u.Employee != null ? u.Employee.Name : "",
            }));
        }
        public override IEnumerable<BaseData> SelectInfo()
        {
            foreach (DataRow s in NGDB.SelectInfo(ENGNameTable))
                    yield return new CabinetEmployee().SetDataRow(s);
        }
    }
}
