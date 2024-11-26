using System.Text;
using System.Text.RegularExpressions;


public static class Helpers
{
    public static bool isValidURL(string str)
    {
        string strRegex = @"^((https?|ftp|smtp):\/\/)?(www.)?[a-z0-9]+\.[a-z]+(\/[a-zA-Z0-9#]+\/?)*$";
        Regex re = new Regex(strRegex);
        if (re.IsMatch(str))
            return (true);
        else
            return (false);
    }
    public static class RandomIdGenerator
    {
        private static char[] _base62chars =
            "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"
            .ToCharArray();

        private static Random _random = new Random();

        public static string GetBase62(int length)
        {
            var sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
                sb.Append(_base62chars[_random.Next(62)]);

            return sb.ToString();
        }

        
    }
}

