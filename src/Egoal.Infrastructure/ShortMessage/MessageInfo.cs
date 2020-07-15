using System.Collections.Generic;

namespace Egoal.ShortMessage
{
    public class MessageInfo
    {
        public string Mobile { get; set; }
        public string Content { get; set; }
        public string TemplateCode { get; set; }
        public Dictionary<string, string> TemplateParams { get; set; }
        public string SignName { get; set; }
    }
}
