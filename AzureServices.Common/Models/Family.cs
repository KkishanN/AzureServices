using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureServices.Common.Models
{
    public class Family
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string FamilyName { get; set; }
        public string  Caste { get; set; }
        public string Category { get; set; }
        public string Father { get; set; }
        public string Mother { get; set; }
        public string Children { get; set; }
        public string ResidentialAddresss { get; set; }
    }
}
