using System;
using System.Linq;

namespace Tests.Helpers
{
    public static class CommonHelper
    {
        public static readonly Random _random = new Random();

        /// <summary>
        /// 生成指定长度范围的随机字符串（字母数字）
        /// </summary>
        public static string GenerateRandomString(int minLength, int maxLength)
        {
            int length = _random.Next(minLength, maxLength + 1);
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// 生成随机的中国大陆手机号
        /// </summary>
        public static string GenerateRandomPhone()
        {
            string[] prefixes = { "138", "139", "150", "158", "188", "189", "177", "199" };
            string prefix = prefixes[_random.Next(prefixes.Length)];
            string suffix = _random.Next(10000000, 100000000).ToString();
            return prefix + suffix;
        }

        /// <summary>
        /// 生成随机邮箱
        /// </summary>
        public static string GenerateRandomEmail()
        {
            string prefix = GenerateRandomString(5, 8).ToLower();
            return $"{prefix}@example.com";
        }
        /// <summary>
        /// 生成随机长度的数
        /// </summary>
        public static string RandomString(int minLength, int maxLength)
        {
            // 随机决定本次生成的长度
            int length = _random.Next(minLength, maxLength + 1);
            // 允许出现的字符池（包含大小写字母和数字）
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
        public static int RandomBetween(int m, int n)
        {
            if (m > n) (m, n) = (n, m); // 如果 m > n，交换确保区间正确
            Random rand = new Random();
            int randomValue = rand.Next(m, n + 1);
            return randomValue; // 直接返回字符串
        }
    }
}
