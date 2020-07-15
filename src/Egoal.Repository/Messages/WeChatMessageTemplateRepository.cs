using Egoal.EntityFrameworkCore;
using Egoal.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.Messages
{
    public class WeChatMessageTemplateRepository : EfCoreRepositoryBase<WeChatMessageTemplate>, IWeChatMessageTemplateRepository
    {
        private static Dictionary<string, string> templateIdCaches = new Dictionary<string, string>();

        public WeChatMessageTemplateRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public async Task<string> GetTemplateIdAsync(string shortTemplateId)
        {
            if (!templateIdCaches.TryGetValue(shortTemplateId, out string templateId))
            {
                templateId = (await FirstOrDefaultAsync(t => t.ShortTemplateId == shortTemplateId))?.TemplateId;

                if (!templateId.IsNullOrEmpty())
                {
                    try
                    {
                        templateIdCaches.Add(shortTemplateId, templateId);
                    }
                    catch { }
                }
            }

            return templateId;
        }
    }
}
