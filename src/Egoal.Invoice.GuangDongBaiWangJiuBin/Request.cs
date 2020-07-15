using System.Text;

namespace Egoal.Invoice.GuangDongBaiWangJiuBin
{
    public class Request
    {
        public string Id { get; set; }
        public string Comment { get; set; }
        public IRequestBody Body { get; set; }

        public string ToXml(Encoding encoding, bool encrypt = true)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append($"<?xml version=\"1.0\" encoding=\"gbk\"?>");
            xml.Append($"<business id=\"{Id}\" comment=\"{Comment}\">");
            xml.Append(Body.ToXml(encoding, encrypt));
            xml.Append("</business>");

            return xml.ToString();
        }
    }
}
