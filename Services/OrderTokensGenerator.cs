using System.Security.Cryptography;

namespace Delwings.Services
{
    public class OrderTokensGenerator
    {
        public int GenerateReceiverNumber()
        {
            Random rng = new Random();
            int number = rng.Next(1000000, 9999999);

            return number;
        }

        public string GenerateTrackId()
        {
            byte[] hashData = new byte[10];
            RandomNumberGenerator.Fill(hashData);
            string trackId = Convert.ToHexString(SHA256.HashData(hashData)).Substring(0, 10);

            return trackId;
        }
    }
}
