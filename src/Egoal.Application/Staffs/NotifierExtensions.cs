using Egoal.Notifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.Staffs
{
    public static class NotifierExtensions
    {
        public const string ExplainerNoticeGroup = "Explainer";
        public const string CheckerNoticeGroup = "Checker";

        public static async Task NoticeExplainerBeginExplainAsync(this IRealTimeNotifier notifier, object data)
        {
            await notifier.SendToGroupsAsync(new List<string> { ExplainerNoticeGroup }, "BeginExplain", data);
        }

        public static async Task NoticeExplainerCompleteExplainAsync(this IRealTimeNotifier notifier, object data)
        {
            await notifier.SendToGroupsAsync(new List<string> { ExplainerNoticeGroup }, "CompleteExplain", data);
        }

        public static async Task NoticeExplainerCheckInAsync(this IRealTimeNotifier notifier, object data)
        {
            await notifier.SendToGroupsAsync(new List<string> { ExplainerNoticeGroup }, "CheckIn", data);
        }

        public static async Task NoticeCheckerCheckInAsync(this IRealTimeNotifier notifier, object data)
        {
            await notifier.SendToGroupsAsync(new List<string> { CheckerNoticeGroup }, "CheckIn", data);
        }

        public static async Task NoticeCheckerCheckOutAsync(this IRealTimeNotifier notifier, object data)
        {
            await notifier.SendToGroupsAsync(new List<string> { CheckerNoticeGroup }, "CheckOut", data);
        }
    }
}
