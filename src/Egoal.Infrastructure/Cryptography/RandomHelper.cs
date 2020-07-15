using System;
using System.Text;

namespace Egoal.Cryptography
{
    public class RandomHelper
    {
        public static string CreateRandomNumber(int length = 6)
        {
            StringBuilder randomNumber = new StringBuilder();
            Random random = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < length; i++)
            {
                randomNumber.Append(random.Next(0, 9));
            }
            return randomNumber.ToString();
        }

        public static int CreateRandomNumber(int minValue, int maxValue)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            return random.Next(minValue, maxValue);
        }
    }
}
