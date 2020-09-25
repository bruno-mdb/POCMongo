using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Demo.Hubs
{
    public class LoggerHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("msg",message);
        }
    }
}
