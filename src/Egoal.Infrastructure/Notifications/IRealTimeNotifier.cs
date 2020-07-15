using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.Notifications
{
    public interface IRealTimeNotifier
    {
        Task SendToClientsAsync(List<string> clients, string method, object data);
        Task SendToGroupsAsync(List<string> groups, string method, object data);
        Task SendToUsersAsync(List<string> users, string method, object data);
        Task SendToAllAsync(string method, object data);
    }
}
