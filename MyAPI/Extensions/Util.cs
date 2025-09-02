using System;
using System.Security.Cryptography;
using System.Text;

namespace MyAPI.Extensions
{
    public static class Util
    {
        /// <summary>
        /// คำนวณอายุจากวันเกิด
        /// </summary>
        /// <param name="dateTimeOffset"></param>
        /// <returns></returns>
        public static int GetCurrentAge(DateTimeOffset dateTimeOffset)
        {
            var today = DateTime.UtcNow;
            var age = today.Year - dateTimeOffset.Year;
            if (today < dateTimeOffset.AddYears(age))
                age--;
            return age;
        }

        public static string ComputeMD5Hash(string input)
        {
            // สร้าง instance ของ MD5
            using (MD5 md5 = MD5.Create())
            {
                // แปลง input string เป็น byte array และคำนวณ hash
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // แปลง byte array เป็น string ที่แสดงผลในรูปแบบ hexadecimal
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

    }
}
