using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureServices.SignalR.Models
{
    public class Auction
    {
        public int Id { get; set; }
        public string ItemName { get; set; } = "";
        public int CurrentBid { get; set; }
    }
}
