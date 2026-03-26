using System.Text.RegularExpressions;

namespace Feex.Application.Helpers
{
    public class GenericHelper
    {
        public static string FormatPhoneNumber(string phone, string format = "INTL")
        {
            if (string.IsNullOrEmpty(phone))
                return phone;

            string result = "";
            if (format == "LOCAL")
            {
                if (phone.StartsWith("+234"))
                {
                    result = phone.Replace("+234", "0");
                }
                else if (phone.StartsWith("234"))
                {
                    result = "0" + phone.Remove(0, 3);
                }
                else if (phone.StartsWith("0") && phone.Length == 11)
                {
                    result = phone;
                }
                else if (phone.StartsWith("00") && phone.Length == 12)
                {
                    result = phone.Replace("00", "0");
                }
            }
            else
            {
                if (phone.StartsWith("+"))
                {
                    result = phone.Remove(0, 1);
                }

                result = phone.StartsWith("0") ? $"234{phone.Remove(0, 1)}" : phone;
            }

            return result;
        }

        public static int GenerateRandomInteger(int length = 0)
        {
            var _random = new Random();

            if (length <= 0)
            {
                // If length is not specified or less than or equal to zero, generate any random integer
                return _random.Next();
            }

            // Ensure the length is valid (1 to 9 digits)
            if (length > 9)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Length cannot exceed 9 digits when generating random numbers.");
            }

            // Calculate the minimum and maximum range based on the length
            int minValue = (int)Math.Pow(10, length - 1);
            int maxValue = (int)Math.Pow(10, length) - 1;

            return _random.Next(minValue, maxValue + 1);
        }

        public static string GenerateTxnRef()
        {
            Random rn = new Random();
            var numbers = rn.Next(100000, 1000000);
            var dtString = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            return $"{numbers.ToString()}{dtString}";
        }

        public static string GenerateRandomString(int length)
        {
            var seed = "123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string output = "";
            for (int i = 0; i < length; i++)
            {
                output += seed[new Random().Next(0, seed.Length - 1)];
            }
            return output;
        }

        public static long GenerateRandomNumbers(int length)
        {
            var seed = "123456789";
            string output = "";
            for (int i = 0; i < length; i++)
            {
                output += seed[new Random().Next(0, seed.Length - 1)];
            }
            return Convert.ToInt32(output);
        }

        public static FileInfo ConvertStringToFile(string base64Content, string fileName, string fileExtension)
        {
            try
            {
                if (string.IsNullOrEmpty(base64Content))
                {
                    throw new ArgumentException("File content cannot be null or empty");
                }

                byte[] fileBytes = Convert.FromBase64String(base64Content);
                string tempFilePath = Path.Combine(Path.GetTempPath(), fileName + "." + fileExtension);

                File.WriteAllBytes(tempFilePath, fileBytes);
                return new FileInfo(tempFilePath);
            }
            catch (Exception ex)
            {
                throw new Exception("Error converting string to file", ex);
            }
        }

        /// <summary>
        /// Converts a base64 string to a byte array.
        /// </summary>
        /// <param name="base64String">The base64 encoded string to convert.</param>
        /// <returns>A byte array containing the decoded data.</returns>
        /// <exception cref="ArgumentException">Thrown when the input string is not a valid base64 string.</exception>
        public static byte[] ConvertBase64ToByteArray(string base64String)
        {
            // Check if the string is null or empty
            if (string.IsNullOrEmpty(base64String))
            {
                return new byte[0];
            }

            try
            {
                // Remove potential data URL prefix (e.g., "data:application/pdf;base64,")
                if (base64String.Contains(","))
                {
                    base64String = base64String.Split(',')[1];
                }

                // Remove whitespace if present
                base64String = base64String.Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "");

                // Convert base64 string to byte array
                return Convert.FromBase64String(base64String);
            }
            catch (FormatException)
            {
                throw new ArgumentException("The provided string is not a valid base64 string.", nameof(base64String));
            }
        }

        public static string GenerateApiKey(string env)
        {
            if (env.ToLower() == "development")
            {
                env = "test";
            }
            if (env.ToLower() == "production")
            {
                env = "prod";
            }
            return ($"Feex_sk_{env}_{(Guid.NewGuid()).ToString("N")}").ToLower();
        }

        public static string FormatDate(DateTime date)
        {
            string result = "";
            string day = "";
            string month = "";

            if (date.Day < 10)
            {
                day = $"0{date.Day}";
            }
            else
            {
                day = $"{date.Day}";
            }

            if (date.Month < 10)
            {
                month = $"0{date.Month}";
            }
            else
            {
                month = $"{date.Month}";
            }

            result = $"{day}/{month}/{date.Year}";
            return result;
        }
    }

}
