using System.Security.Cryptography;
using System.Text;
using URLShortener.Data;

namespace URLShortener.Services
{
    public class UrlShortenerService
    {
        public UrlShortenerService()
        {   
        }

        public string? ShortenUrl(string originalUrl, List<UrlMapping> urlMappings)
        {
            if (urlMappings.Exists(x => x.OriginalUrl == originalUrl)) { return urlMappings.Where(x => x.OriginalUrl == originalUrl).First().ShortenedUrl; } // do not readd if it exists return the shortened that already exists
            // Note: really should test the raw uri and ditch any www, http or https, etc. should bring up to discuss as new spec
            string? shortenedUrl = GetShortened(urlMappings, originalUrl);
            if (shortenedUrl == null) { return null; }
            var mapping = new UrlMapping
            {
                OriginalUrl = originalUrl,
                ShortenedUrl = shortenedUrl
            };
            urlMappings.Add(mapping);
            return mapping.ShortenedUrl;
        }


        public async Task<string?> GetOriginalUrl(string short_id, List<UrlMapping> urlMappings)
        {   
            UrlMapping mapping = urlMappings.FirstOrDefault(x => x.ShortenedUrl == short_id);
            return mapping?.OriginalUrl;
        }

        private string GenerateShortCode()
        {
            return Helpers.RandomIdGenerator.GetBase62(6);
            
            // too long
            //string base64String = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(originalUrl))).Trim('=').Replace("/","");
            //return base64String;
        }
        private string? GetShortened(List<UrlMapping> urlMappings, string originalUrl)
        {   
            // note, is possible but very unlikely to generate duplicates so this will keep trying.  Actually a chance that we could get duplicates multiple time but very unlikely
            string? shortenedUrl = GenerateShortCode();
            int i = 0;
            // check if shoretened exists (and is not the same originalUrl)            
            while (urlMappings.Exists(x => x.OriginalUrl == originalUrl && x.ShortenedUrl == shortenedUrl))
            {
                shortenedUrl = GenerateShortCode();
                i++;
                if (i > 9999999) { break; } // just in case though extremely unlikely
            }
            return shortenedUrl;

        }
    }
}
