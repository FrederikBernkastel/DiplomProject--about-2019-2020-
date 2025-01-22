using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DipProject
{
    public partial class Provider : BaseData
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string CheckingAccount { get; set; }
        public string INN { get; set; }
        public string Phone { get; set; }
        public string E_Mail { get; set; }
        public string Address { get; set; }
    }
    public partial class Provider : BaseData
    {
        public override BaseData SetDataRow(DataRow obj)
        {
            ID = int.Parse(obj[0].ToString());
            Name = obj[1].ToString();
            FullName = obj[2].ToString();
            CheckingAccount = obj[3].ToString();
            INN = obj[4].ToString();
            Phone = obj[5].ToString();
            E_Mail = obj[6].ToString();
            Address = obj[7].ToString();
            return this;
        }
        public override IEnumerable<BaseData> WebFilter(BaseTable table)
        {
            if (table is CollectionDevices)
                return table.SelectInfo().Cast<Device>().Where(u => u.Provider != null ? u.Provider.ID == ID : false);
            return Enumerable.Empty<BaseData>();
        }
        public override IEnumerable<string> NGToString()
        {
            return new string[]
            {
                ID.ToString(),
                Name.NGToString(),
                FullName.NGToString(),
                CheckingAccount.NGToString(),
                INN.NGToString(),
                Phone.NGToString(),
                E_Mail.NGToString(),
                Address.NGToString(),
            };
        }
        public override string SerializeJSON()
        {
            return JsonConvert.SerializeObject(ProviderST.Load(this), Formatting.Indented);
        }
        public struct ProviderST
        {
            [JsonProperty(PropertyName = "Название фирмы [string]")]
            public string Name { get; set; }
            [JsonProperty(PropertyName = "ФИО директора [string]")]
            public string FullName { get; set; }
            [JsonProperty(PropertyName = "Расчётный счёт [string]")]
            public string CheckingAccount { get; set; }
            [JsonProperty(PropertyName = "ИНН [string]")]
            public string INN { get; set; }
            [JsonProperty(PropertyName = "Телефон [string]")]
            public string Phone { get; set; }
            [JsonProperty(PropertyName = "Почтовый адрес [string]")]
            public string E_Mail { get; set; }
            [JsonProperty(PropertyName = "Адрес [string]")]
            public string Address { get; set; }
            public static ProviderST Load(Provider obj)
            {
                return new ProviderST
                {
                    Name = obj.Name,
                    FullName = obj.FullName,
                    CheckingAccount = obj.CheckingAccount,
                    INN = obj.INN,
                    Phone = obj.Phone,
                    E_Mail = obj.E_Mail,
                    Address = obj.Address,
                };
            }
        }
    }
}
