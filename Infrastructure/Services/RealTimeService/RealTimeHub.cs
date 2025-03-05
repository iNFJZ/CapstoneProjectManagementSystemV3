using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
namespace Infrastructure.Services
{
    public class RealTimeHub : Hub
    {
        private readonly IHubContext<RealTimeHub> _hubContext;

        public RealTimeHub(IHubContext<RealTimeHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task SendMessage(string user, string message)
        {
            // Broadcast the message to all connected clients
            if(_hubContext.Clients != null)
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendMessageWithAdditionalData(string user, string message , string additionalData)
        {
            // Broadcast the message to all connected clients
            if (_hubContext.Clients != null)
                await _hubContext.Clients.All.SendAsync("ReceiveMessageWithAdditionalData", user, message , additionalData);
        }
    }
}
