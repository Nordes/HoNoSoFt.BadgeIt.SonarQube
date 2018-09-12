using System;

namespace HoNoSoFt.BadgeIt.SonarQube.Web.Configurations
{
    public class SonarQubeApiConfig
    {
        public string ApiKey { get; set; }
        public Uri BaseUri { get; set; }

        internal void LoadFromEnvironmentVariables()
        {
            var apiKey = Environment.GetEnvironmentVariable("SONAR_API_KEY");
            var baseUri = Environment.GetEnvironmentVariable("SONAR_URI");

            if (!string.IsNullOrEmpty(apiKey))
                ApiKey = apiKey.Trim();
            if (!string.IsNullOrEmpty(baseUri))
                BaseUri = new Uri(baseUri.Trim());
        }
    }
}