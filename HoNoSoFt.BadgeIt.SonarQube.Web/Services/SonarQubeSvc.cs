using HoNoSoFt.BadgeIt.SonarQube.Web.Configurations;
using HoNoSoFt.BadgeIt.SonarQube.Web.Models;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HoNoSoFt.BadgeIt.SonarQube.Web.Services
{
    public class SonarQubeSvc : ISonarQubeSvc
    {
        // https://www.stevejgordon.co.uk/httpclientfactory-named-typed-clients-aspnetcore
        private readonly HttpClient _sonarQubeClient;
        private readonly SonarQubeApiConfig _sonarQubeApiConfig;

        public SonarQubeSvc(
            IHttpClientFactory httpClientFactory,
            IOptions<SonarQubeApiConfig> sonarQubeConfiguration)
        {
            _sonarQubeApiConfig = sonarQubeConfiguration.Value;

            var basicToken = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{sonarQubeConfiguration.Value.ApiKey}:"));
            var authHeader = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", basicToken);
            _sonarQubeClient = httpClientFactory.CreateClient("SonarQube");
            _sonarQubeClient.BaseAddress = sonarQubeConfiguration.Value.BaseUri;
            _sonarQubeClient.DefaultRequestHeaders.Authorization = authHeader;
        }

        public async Task<SonarQubeStats> FetchStatsAsync(string key, string branch, Metric metrics)
        {
            // TODO Keep a 10/30 second cache to avoid calling the server on massive load.
            var result = await _sonarQubeClient.GetAsync(
                $"measures/component?additionalFields=metrics" +
                $"&componentKey={System.Net.WebUtility.UrlEncode(key)}"+ 
                (string.IsNullOrEmpty(branch) ? string.Empty : $"&branch={System.Net.WebUtility.UrlEncode(branch)}") +
                $"&metricKeys={metrics.ToSnakeCase()}");

            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            return await result.Content.ReadAsAsync<SonarQubeStats>();
        }
    }
}