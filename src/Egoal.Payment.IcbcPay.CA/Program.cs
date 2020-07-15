using infosecapiLib;
using System;
using System.Text;

namespace Egoal.Payment.IcbcPay.CA
{
    class Program
    {
        static void Main(string[] args)
        {
            infosec ca = new infosec();

            var data = Encoding.UTF8.GetString(Convert.FromBase64String(args[0]));
            var sign = ca.sign_SHA1(data, args[1], args[2]);

            Console.Write(sign);
        }
    }
}
