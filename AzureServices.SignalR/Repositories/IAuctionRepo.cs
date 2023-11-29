using AzureServices.SignalR.Models;
using System.Collections.Generic;

namespace AzureServices.SignalR.Repositories
{
    public interface IAuctionRepo
    {
        IEnumerable<Auction> GetAll();
        void NewBid(int auctionId, int newBid);
    }
}
