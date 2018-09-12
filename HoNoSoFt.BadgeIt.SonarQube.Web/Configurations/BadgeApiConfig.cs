using System;

namespace HoNoSoFt.BadgeIt.SonarQube.Web.Configurations
{
    public class BadgeApiConfig
    {
        public Uri BaseUri { get; set; }

        internal void LoadFromEnvironmentVariables()
        {
            var baseUri = Environment.GetEnvironmentVariable("SHIELDS_URI");

            if (!string.IsNullOrEmpty(baseUri))
                BaseUri = new Uri(baseUri.Trim());
        }
    }
}