using System.Text;

namespace Egoal.Invoice.GuangDongBaiWangJiuBin
{
    public interface IRequestBody
    {
        string ToXml(Encoding encoding, bool encrypt);
    }
}
