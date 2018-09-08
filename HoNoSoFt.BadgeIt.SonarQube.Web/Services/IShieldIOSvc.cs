using HoNoSoFt.BadgeIt.SonarQube.Web.Models;
using System.Threading.Tasks;

namespace HoNoSoFt.BadgeIt.SonarQube.Web.Services
{
    public interface IShieldIoSvc
    {
        Task<string> GetSvgBadgeAsync(string label, string value, ShieldsColors color);
    }
}