using System;

namespace HoNoSoFt.BadgeIt.SonarQube.Web.Configurations
{
    public class SonarQubeApiConfig
    {
        public string ApiKey { get; set; }
        public Uri BaseUri { get; set; }
    }
}