using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Egoal.Extensions
{
    public static class StreamExtensions
    {
        public static async Task<string> ReadAsStringAsync(this Stream stream)
        {
            return await stream.ReadAsStringAsync(Encoding.UTF8);
        }

        public static async Task<string> ReadAsStringAsync(this Stream stream, Encoding encoding)
        {
            using (StreamReader streamReader = new StreamReader(stream, encoding))
            {
                return await streamReader.ReadToEndAsync();
            }
        }
    }
}
