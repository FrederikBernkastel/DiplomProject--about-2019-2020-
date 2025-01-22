using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DipProject
{
    public partial class TypeDevice : BaseData
    {
        public string Name { get; set; }
    }
    public partial class TypeDevice : BaseData
    {
        public override BaseData SetDataRow(DataRow obj)
        {
            ID = int.Parse(obj[0].ToString());
            Name = obj[1].ToString();
            return this;
        }
        public override IEnumerable<BaseData> WebFilter(BaseTable table)
        {
            if (table is CollectionDevices)
                return table.SelectInfo().Cast<Device>().Where(u => u.TypeDevice != null ? u.TypeDevice.ID == ID : false);
            return Enumerable.Empty<BaseData>();
        }
        public override IEnumerable<string> NGToString()
        {
            return new string[]
            {
                ID.ToString(),
                Name.NGToString(),
            };
        }
        public override string SerializeJSON()
        {
            return JsonConvert.SerializeObject(TypeDeviceST.Load(this), Formatting.Indented);
        }
        public struct TypeDeviceST
        {
            [JsonProperty(PropertyName = "Тип [string]")]
            public string Name { get; set; }
            public static TypeDeviceST Load(TypeDevice obj)
            {
                return new TypeDeviceST
                {
                    Name = obj.Name,
                };
            }
        }
    }
}
