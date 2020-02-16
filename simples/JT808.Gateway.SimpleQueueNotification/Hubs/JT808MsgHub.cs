using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;


namespace JT808.Gateway.SimpleQueueNotification.Hubs
{
    public class JT808MsgHub : Hub
    {
        public Task ReceiveMessage(string token, string msg)
		{
			Clients.Caller.SendAsync("ReceiveMessage", token, "Heartbeat");
			return Task.CompletedTask;
		}
	}
}
