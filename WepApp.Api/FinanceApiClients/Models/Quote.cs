using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClients.Models
{
    public class Quote
    {
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
