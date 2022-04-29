using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using WebApp.Models;


namespace WebApp.Hubs
{
    public class MyHub:Hub
    {

        private static int Count;

        public override Task OnConnectedAsync()
        {
            Count++;
            base.OnConnectedAsync();
            this.Clients.All.SendAsync("updateCount", Count);
            return Task.CompletedTask;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Count--;
            base.OnDisconnectedAsync(exception);
            this.Clients.All.SendAsync("updateCount", Count);
            return Task.CompletedTask;
        }

    }
}
