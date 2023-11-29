using AzureServices.SignalR.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureServices.SignalR.Hubs
{
    public class AuctionHub : Hub
    {
        public async Task NotifyNewBid(AuctionNotify auction)
        {
            // sends notification to all connected users
            // first param: methodName, second param: message
            await Clients.All.SendAsync("ReceiveNewBid",
                auction);
        }

        public async Task SendMessage(string userName, string message)
        {
            try
            {
                await Clients.All.SendAsync("ReceiveMessage", userName, message);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
