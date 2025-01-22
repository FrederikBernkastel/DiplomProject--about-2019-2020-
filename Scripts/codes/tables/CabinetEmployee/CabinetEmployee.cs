using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DipProject
{
    public partial class CabinetEmployee : BaseData
    {
        public int Campus { get; set; }
        public int Cabinet { get; set; }
        public Employee Employee { get; set; }
    }
    public partial class CabinetEmployee : BaseData
    {
        public override BaseData SetDataRow(DataRow obj)
        {
            ID = int.Parse(obj[0].ToString());
            Campus = int.Parse(obj[1].ToString());
            Cabinet = int.Parse(obj[2].ToString());
            Employee = NGDB.List["Сотрудники"].SelectInfo()
                .FirstOrDefault(u => u.ID == int.Parse(obj[3].ToString())) as Employee;
            return this;
        }
        public override IEnumerable<BaseData> WebFilter(BaseTable table)
        {
            if (table is CollectionEmployee)
                return table.SelectInfo().Cast<Employee>().Where(u => Employee != null ? u.ID == Employee.ID : false);
            else if (table is CollectionDevices)
                return table.SelectInfo().Cast<Device>().Where(u => u.Campus == Campus && u.Cabinet == Cabinet);
            return Enumerable.Empty<BaseData>();
        }
        public override IEnumerable<string> NGToString()
        {
            return new string[]
            {
                ID.ToString(),
                Campus.ToString(),
                Cabinet.ToString(),
                Employee != null ? Employee.ID.ToString() : "0",
            };
        }
        public override string SerializeJSON()
        {
            return JsonConvert.SerializeObject(CabinetEmployeeST.Load(this), Formatting.Indented);
        }
        public struct CabinetEmployeeST
        {
            [JsonProperty(PropertyName = "Уч. корпус [int]")]
            public int Campus { get; set; }
            [JsonProperty(PropertyName = "Кабинет [int]")]
            public int Cabinet { get; set; }
            [JsonProperty(PropertyName = "ID сотрудника [int]")]
            public int EmployeeID { get; set; }
            public static CabinetEmployeeST Load(CabinetEmployee obj)
            {
                return new CabinetEmployeeST
                {
                    Campus = obj.Campus,
                    Cabinet = obj.Cabinet,
                    EmployeeID = obj.Employee != null ? obj.Employee.ID : 0,
                };
            }
        }
    }
}
