using System;
using System.Security.Cryptography;
using System.Text;

public class URLHelper
{
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const int Base = 62;
    public static string baseURL = "https://tinyurl.azurewebsites.net/";

    public static bool validate(string url)
    {
        Uri uriResult;
        bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

        if (!result) throw new Exception("URL malformed");
        return true;

    }

    public static string Shorten(string url)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] hashBytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(url));
            ulong hashValue = BitConverter.ToUInt64(hashBytes, 0);
            StringBuilder shortUrlBuilder = new StringBuilder();

            do
            {
                shortUrlBuilder.Append(Alphabet[(int)(hashValue % Base)]);
                hashValue /= Base;
            } while (hashValue > 0);

            return shortUrlBuilder.ToString();
        }
    }
}
