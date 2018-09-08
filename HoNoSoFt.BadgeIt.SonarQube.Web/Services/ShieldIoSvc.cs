using HoNoSoFt.BadgeIt.SonarQube.Web.Configurations;
using HoNoSoFt.BadgeIt.SonarQube.Web.Models;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

namespace HoNoSoFt.BadgeIt.SonarQube.Web.Services
{
    public class ShieldIoSvc : IShieldIoSvc
    {
        private readonly HttpClient _shieldIoClient;

        public ShieldIoSvc(IHttpClientFactory clientFactory, IOptions<BadgeApiConfig> badgeApiConfig)
        {
            _shieldIoClient = clientFactory.CreateClient("shield.io");
            _shieldIoClient.BaseAddress = badgeApiConfig.Value.BaseUri;
        }

        public Task<string> GetSvgBadgeAsync(string label, string value, ShieldsColors color)
        {
            var valueEncoded = System.Net.WebUtility.UrlEncode(value);
            var labelEncoded = System.Net.WebUtility.UrlEncode(label);

            // This could potentially become more generic (configuration file). Only issue are the colors.
            return _shieldIoClient.GetStringAsync($"label-{valueEncoded}-{color.ToString().ToLowerInvariant()}.svg?longCache=true&style=popout&label={labelEncoded}");
        }
    }
}