using Egoal.Extensions;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Egoal.SignalR.Hubs
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var groupName = GetGroupName();
            if (!groupName.IsNullOrEmpty())
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var groupName = GetGroupName();
            if (!groupName.IsNullOrEmpty())
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            }

            await base.OnDisconnectedAsync(exception);
        }

        private string GetGroupName()
        {
            var httpContext = Context.GetHttpContext();
            var group = httpContext.Request.Query["Group"];
            if (group.Count > 0)
            {
                return group[0];
            }

            return string.Empty;
        }
    }
}
