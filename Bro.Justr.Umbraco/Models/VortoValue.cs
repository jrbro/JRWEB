using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bro.Justr.Umbraco.Models
{
    public class VortoValue
    {
        [JsonProperty("values")]
        public IDictionary<string, object> Values { get; set; }

        [JsonProperty("dtdGuid")]
        public Guid DtdGuid { get; set; }
    }
}
