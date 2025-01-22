using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DipProject
{
    public partial class Employee : BaseData
    {
        public string Name { get; set; }
        public string Position { get; set; }
    }
    public partial class Employee : BaseData
    {
        public override BaseData SetDataRow(DataRow obj)
        {
            ID = int.Parse(obj[0].ToString());
            Name = obj[1].ToString();
            Position = obj[2].ToString();
            return this;
        }
        public override IEnumerable<BaseData> WebFilter(BaseTable table)
        {
            if (table is CollectionDevices)
                return table.SelectInfo().Cast<Device>().Where(u => u.Employee != null ? u.Employee.ID == ID : false);
            else if (table is CollectionCabinetEmployee)
                return table.SelectInfo().Cast<CabinetEmployee>().Where(u => u.Employee != null ? u.Employee.ID == ID : false);
            return Enumerable.Empty<BaseData>();
        }
        public override IEnumerable<string> NGToString()
        {
            return new string[]
            {
                ID.ToString(),
                Name.NGToString(),
                Position.NGToString(),
            };
        }
        public override string SerializeJSON()
        {
            return JsonConvert.SerializeObject(EmployeeST.Load(this), Formatting.Indented);
        }
        public struct EmployeeST
        {
            [JsonProperty(PropertyName = "ФИО [string]")]
            public string Name { get; set; }
            [JsonProperty(PropertyName = "Должность [string]")]
            public string Position { get; set; }
            public static EmployeeST Load(Employee obj)
            {
                return new EmployeeST
                {
                    Name = obj.Name,
                    Position = obj.Position,
                };
            }
        }
    }
}
