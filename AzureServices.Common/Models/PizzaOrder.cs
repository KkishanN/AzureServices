using System;
using System.Collections.Generic;
using System.Text;

namespace AzureServices.Common.Models
{
    public class PizzaOrder
    {
        public string CustomerName { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
    }
}
