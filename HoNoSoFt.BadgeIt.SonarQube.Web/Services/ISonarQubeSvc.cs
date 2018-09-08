using HoNoSoFt.BadgeIt.SonarQube.Web.Models;
using System.Threading.Tasks;

namespace HoNoSoFt.BadgeIt.SonarQube.Web.Services
{
    public interface ISonarQubeSvc
    {
        Task<SonarQubeStats> FetchStatsAsync(string key, Metric metrics);
    }
}