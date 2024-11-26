using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.RegularExpressions;
using System.Web;
using URLShortener.Data;
using URLShortener.Services;


namespace URLShortener.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UrlShortenerController : Controller
    {
        
        private readonly UrlShortenerService _urlShortenerService;
        
        private static List<UrlMapping> _urlMappings = new List<UrlMapping>();
        public UrlShortenerController(UrlShortenerService urlShortenerService)
        {
            _urlShortenerService = urlShortenerService;
        }

        

        [HttpPost("api/{originalUrl}")]
        public async Task<ActionResult<string>> ShortenUrl(string originalUrl)
        {
            // Validate and shorten decodedUrl
            string decodedUrl = HttpUtility.UrlDecode(originalUrl);
            if (!Helpers.isValidURL(decodedUrl)) { return BadRequest(); }// or we could return something like "<ERRORCODE>:Bad Url"; 
            string? shortenedUrl = _urlShortenerService.ShortenUrl(decodedUrl, _urlMappings);
            if (shortenedUrl == null) { return BadRequest(); } // if an error occurs in _urlShortenerService.ShortenUrl

            // create the return get url using originalUrl 
            string currentUrl = UriHelper.GetDisplayUrl(HttpContext.Request);
            string returnValue = currentUrl.Replace(originalUrl, shortenedUrl);
            // 
            return $"{returnValue}";
        }

        [HttpGet("api/{short_id}")]
        public async Task<ActionResult<string>> ReturnLongUrl(string short_id)
        {
            string? originalUrl = await _urlShortenerService.GetOriginalUrl(short_id, _urlMappings);
            return originalUrl == null ? NotFound() : originalUrl;            
        }

    }

}
