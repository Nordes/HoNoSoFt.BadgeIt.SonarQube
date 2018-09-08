using HoNoSoFt.BadgeIt.SonarQube.Web.Models;

namespace HoNoSoFt.BadgeIt.SonarQube.Web.Logics.Interfaces
{
    public interface IExtractor<T>
    {
        T Extract(Measure measure, SonarQubeMetric metric);
    }
}
