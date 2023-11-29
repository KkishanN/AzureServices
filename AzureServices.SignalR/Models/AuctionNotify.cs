using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureServices.SignalR.Models
{
    public class AuctionNotify
    {
        public int AuctionId { get; set; }
        public int NewBid { get; set; }
    }
}
