using AzureServices.SignalR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureServices.SignalR.Repositories
{
    public class AuctionMemoryRepo : IAuctionRepo
    {
        private readonly List<Auction> auctions = new List<Auction>();

        public AuctionMemoryRepo()
        {
            auctions.Add(new Auction { Id = 1, ItemName = "Cool refrigerator", CurrentBid = 23 });
            auctions.Add(new Auction { Id = 2, ItemName = "Noisy headphones", CurrentBid = 4 });
            auctions.Add(new Auction { Id = 3, ItemName = "Blinding television", CurrentBid = 143 });
            auctions.Add(new Auction { Id = 4, ItemName = "Sturdy kitchen table", CurrentBid = 12 });
            auctions.Add(new Auction { Id = 5, ItemName = "Creative pencil set", CurrentBid = 3 });
        }

        public IEnumerable<Auction> GetAll()
        {
            return auctions;
        }

        public void NewBid(int auctionId, int newBid)
        {
            var auction = auctions.Single(a => a.Id == auctionId);
            auction.CurrentBid = newBid;
        }
    }
}
