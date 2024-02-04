using Microsoft.EntityFrameworkCore;

namespace UrlShorter.Services;
public class UrlServices
    {
        private const int  LenghtOfShortUrl = 9;
        private const string Alphabet = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm0123456789";

        private readonly Random random = new Random();

        private readonly AppDbContext dbContext;
        public async Task<String> GenerateUniqueCode() {
            var codeChars = new char[LenghtOfShortUrl];

            while (true) {
                for (int i = 0; i < codeChars.Length; i++)
                {
                    int rndInd = random.Next(0, Alphabet.Length - 1);

                    codeChars[i] = Alphabet[rndInd];

                }
                var code = new string(codeChars);

                if (!await dbContext.ShortenUrls.AnyAsync(s => s.Code == code)) {
                    return code;
                }
            }
        }

        public static int getLenghtOfShort() {
            return LenghtOfShortUrl;
        }
}

