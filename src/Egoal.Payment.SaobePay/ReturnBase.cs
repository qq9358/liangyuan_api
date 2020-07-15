using Egoal.Cryptography;

namespace Egoal.Payment.SaobePay
{
    public class ReturnBase
    {
        public string return_code { get; set; }
        public string return_msg { get; set; }
        public string key_sign { get; set; }

        public bool CheckSign()
        {
            if (return_code != "01")
            {
                return true;
            }

            var s = ToUrl();
            var sign = MD5Helper.Encrypt(s);

            return key_sign == sign;
        }

        public bool CheckSign(string key)
        {
            var s = $"{ToUrl()}&access_token={key}";
            var sign = MD5Helper.Encrypt(s);

            return key_sign == sign;
        }

        protected virtual string ToUrl()
        {
            return $"return_code={return_code}&return_msg={return_msg}";
        }
    }
}
