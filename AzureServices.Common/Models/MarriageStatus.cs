using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureServices.Common.Models
{
    public class MarriageStatus
    {
        public bool IsMarried { get; set; }
        public Person Spouse { get; set; }
    }
}
