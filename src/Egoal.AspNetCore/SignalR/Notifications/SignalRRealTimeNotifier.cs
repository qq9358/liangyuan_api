using Egoal.Notifications;
using Egoal.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.SignalR.Notifications
{
    public class SignalRRealTimeNotifier : IRealTimeNotifier
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public SignalRRealTimeNotifier(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendToClientsAsync(List<string> clients, string method, object data)
        {
            await _hubContext.Clients.Clients(clients).SendAsync(method, data);
        }

        public async Task SendToGroupsAsync(List<string> groups, string method, object data)
        {
            await _hubContext.Clients.Groups(groups).SendAsync(method, data);
        }

        public async Task SendToUsersAsync(List<string> users, string method, object data)
        {
            await _hubContext.Clients.Users(users).SendAsync(method, data);
        }

        public async Task SendToAllAsync(string method, object data)
        {
            await _hubContext.Clients.All.SendAsync(method, data);
        }
    }
}
