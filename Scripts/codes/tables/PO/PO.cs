using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DipProject
{
    public partial class PO : BaseData
    {
        public string Name { get; set; }
        public float Cost { get; set; }
        public Device Device { get; set; }
        public string RegistrationKey { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int Gurantee { get; set; }
        public string StatusPO { get; set; }
    }
    public partial class PO : BaseData
    {
        public override BaseData SetDataRow(DataRow obj)
        {
            ID = int.Parse(obj[0].ToString());
            Name = obj[1].ToString();
            Cost = int.Parse(obj[2].ToString());
            Device = NGDB.List["Устройства"]
                .SelectInfo().FirstOrDefault(u => u.ID == int.Parse(obj[3].ToString())) as Device;
            RegistrationKey = obj[4].ToString();
            PurchaseDate = NGExtens.ToDateTime(obj[5].ToString());
            Gurantee = int.Parse(obj[6].ToString());
            StatusPO = obj[7].ToString();
            return this;
        }
        public override IEnumerable<BaseData> WebFilter(BaseTable table)
        {
            if (table is CollectionDevices)
                return table.SelectInfo().Cast<Device>().Where(u => Device != null ? u.ID == Device.ID : false);
            return Enumerable.Empty<BaseData>();
        }
        public override IEnumerable<string> NGToString()
        {
            return new string[]
            {
                ID.ToString(),
                Name.NGToString(),
                Cost.ToString().Replace(",", "."),
                Device != null ? Device.ID.ToString() : "0",
                RegistrationKey.NGToString(),
                (PurchaseDate.Day + "." + PurchaseDate.Month + "." + PurchaseDate.Year).NGToString(),
                Gurantee.ToString(),
                StatusPO.NGToString(),
            };
        }
        public override string SerializeJSON()
        {
            return JsonConvert.SerializeObject(POST.Load(this), Formatting.Indented);
        }
        public struct POST
        {
            [JsonProperty(PropertyName = "Название [string]")]
            public string Name { get; set; }
            [JsonProperty(PropertyName = "Стоимость [float]")]
            public float Cost { get; set; }
            [JsonProperty(PropertyName = "ID устройства [int]")]
            public int DeviceID { get; set; }
            [JsonProperty(PropertyName = "Регистрационный ключ [string]")]
            public string RegistrationKey { get; set; }
            [JsonProperty(PropertyName = "Дата покупки [string]")]
            public string PurchaseDate { get; set; }
            [JsonProperty(PropertyName = "Срок гарантии(мес) [int]")]
            public int Gurantee { get; set; }
            [JsonProperty(PropertyName = "Статус [string]")]
            public string StatusPO { get; set; }
            public static POST Load(PO obj)
            {
                return new POST
                {
                    Name = obj.Name,
                    Cost = obj.Cost,
                    DeviceID = obj.Device != null ? obj.Device.ID : 0,
                    RegistrationKey = obj.RegistrationKey,
                    PurchaseDate = obj.PurchaseDate.ToString(),
                    Gurantee = obj.Gurantee,
                    StatusPO = obj.StatusPO,
                };
            }
        }
    }
}
