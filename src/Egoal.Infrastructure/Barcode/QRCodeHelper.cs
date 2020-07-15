using QRCoder;
using System.Drawing;
using System.Threading.Tasks;

namespace Egoal.Barcode
{
    public static class QRCodeHelper
    {
        public static Task<string> ToDataURLAsync(string value)
        {
            return Task.Run(() =>
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(value, QRCodeGenerator.ECCLevel.Q);
                Base64QRCode qrCode = new Base64QRCode(qrCodeData);
                var imgType = Base64QRCode.ImageType.Jpeg;
                string qrCodeImageAsBase64 = qrCode.GetGraphic(10, Color.Black, Color.White, true, imgType);
                return $"data:image/{imgType.ToString().ToLower()};base64,{qrCodeImageAsBase64}";
            });
        }
    }
}
