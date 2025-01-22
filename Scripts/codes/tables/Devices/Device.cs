using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DipProject
{
    public partial class Device : BaseData
    {
        public string Name { get; set; }
        public string Characteristics { get; set; }
        public TypeDevice TypeDevice { get; set; }
        public string InventoryNumber { get; set; }
        public string SerialNumber { get; set; }
        public float Cost { get; set; }
        public Employee Employee { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int Gurantee { get; set; }
        public string StatusDevice { get; set; }
        public Provider Provider { get; set; }
        public int Campus { get; set; }
        public int Cabinet { get; set; }
    }
    public partial class Device : BaseData
    {
        public override BaseData SetDataRow(DataRow obj)
        {
            ID = int.Parse(obj[0].ToString());
            Name = obj[1].ToString();
            Characteristics = obj[2].ToString();
            TypeDevice = NGDB.List["Типы устройств"]
                .SelectInfo().FirstOrDefault(u => u.ID == int.Parse(obj[3].ToString())) as TypeDevice;
            InventoryNumber = obj[4].ToString();
            SerialNumber = obj[5].ToString();
            Cost = float.Parse(obj[6].ToString());
            Employee = NGDB.List["Сотрудники"]
                .SelectInfo().FirstOrDefault(u => u.ID == int.Parse(obj[7].ToString())) as Employee;
            PurchaseDate = NGExtens.ToDateTime(obj[8].ToString());
            Gurantee = int.Parse(obj[9].ToString());
            StatusDevice = obj[10].ToString();
            Provider = NGDB.List["Поставщики"]
                .SelectInfo().FirstOrDefault(u => u.ID == int.Parse(obj[11].ToString())) as Provider;
            Campus = int.Parse(obj[12].ToString());
            Cabinet = int.Parse(obj[13].ToString());
            return this;
        }
        public override IEnumerable<BaseData> WebFilter(BaseTable table)
        {
            if (table is CollectionPO)
                return table.SelectInfo().Cast<PO>().Where(u => u.Device != null ? u.Device.ID == ID : false);
            else if (table is CollectionEmployee)
                return table.SelectInfo().Cast<Employee>().Where(u => Employee != null ? u.ID == Employee.ID : false);
            else if (table is CollectionSuppliers)
                return table.SelectInfo().Cast<Provider>().Where(u => Provider != null ? u.ID == Provider.ID : false);
            return Enumerable.Empty<BaseData>();
        }
        public override IEnumerable<string> NGToString()
        {
            return new string[]
            {
                ID.ToString(),
                Name.NGToString(),
                Characteristics.NGToString(),
                TypeDevice != null ? TypeDevice.ID.ToString() : "0",
                InventoryNumber.NGToString(),
                SerialNumber.NGToString(),
                Cost.ToString().Replace(",", "."),
                Employee != null ? Employee.ID.ToString() : "0",
                (PurchaseDate.Day + "." + PurchaseDate.Month + "." + PurchaseDate.Year).NGToString(),
                Gurantee.ToString(),
                StatusDevice.NGToString(),
                Provider != null ? Provider.ID.ToString() : "0",
                Campus.ToString(),
                Cabinet.ToString(),
            };
        }
        public override string SerializeJSON()
        {
            return JsonConvert.SerializeObject(DeviceST.Load(this), Formatting.Indented);
        }
        public struct DeviceST
        {
            [JsonProperty(PropertyName = "Название [string]")]
            public string Name { get; set; }
            [JsonProperty(PropertyName = "Тех. характеристики [string]")]
            public string Characteristics { get; set; }
            [JsonProperty(PropertyName = "ID типа устройства [int]")]
            public int TypeDeviceID { get; set; }
            [JsonProperty(PropertyName = "Инвентарный номер [string]")]
            public string InventoryNumber { get; set; }
            [JsonProperty(PropertyName = "Серийный номер [string]")]
            public string SerialNumber { get; set; }
            [JsonProperty(PropertyName = "Стоимость [float]")]
            public float Cost { get; set; }
            [JsonProperty(PropertyName = "ID сотрудника [int]")]
            public int EmployeeID { get; set; }
            [JsonProperty(PropertyName = "Дата покупки [string]")]
            public string PurchaseDate { get; set; }
            [JsonProperty(PropertyName = "Срок гарантии(мес) [int]")]
            public int Gurantee { get; set; }
            [JsonProperty(PropertyName = "Статус [string]")]
            public string StatusDevice { get; set; }
            [JsonProperty(PropertyName = "ID поставщика [int]")]
            public int ProviderID { get; set; }
            [JsonProperty(PropertyName = "Уч. корпус [int]")]
            public int Campus { get; set; }
            [JsonProperty(PropertyName = "Кабинет [int]")]
            public int Cabinet { get; set; }
            public static DeviceST Load(Device obj)
            {
                return new DeviceST
                {
                    Name = obj.Name,
                    Characteristics = obj.Characteristics,
                    TypeDeviceID = obj.TypeDevice != null ? obj.TypeDevice.ID : 0,
                    InventoryNumber = obj.InventoryNumber,
                    SerialNumber = obj.SerialNumber,
                    Cost = obj.Cost,
                    EmployeeID = obj.Employee != null ? obj.Employee.ID : 0,
                    PurchaseDate = obj.PurchaseDate.ToString(),
                    Gurantee = obj.Gurantee,
                    StatusDevice = obj.StatusDevice,
                    ProviderID = obj.Provider != null ? obj.Provider.ID : 0,
                    Campus = obj.Campus,
                    Cabinet = obj.Cabinet,
                };
            }
        }
    }
}
