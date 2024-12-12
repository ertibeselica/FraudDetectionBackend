using FraudDetection.Models;
using Microsoft.AspNetCore.SignalR;

namespace FraudDetection.Api
{
    public class TransactionHub : Hub
    {
        public async Task SendTransactionUpdate(Transaction transaction)
        {
            await Clients.All.SendAsync("ReceiveTransaction", transaction);
        }

        public async Task SendFraudAlert(Transaction transaction)
        {
            await Clients.All.SendAsync("ReceiveFraudAlert", transaction);
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            // Add logging here
            Console.WriteLine("Client connected");
        }
    }
}
