using HoNoSoFt.BadgeIt.SonarQube.Web.Logics.Interfaces;
using HoNoSoFt.BadgeIt.SonarQube.Web.Models;
using HoNoSoFt.BadgeIt.SonarQube.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace HoNoSoFt.BadgeIt.SonarQube.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BadgesController : ControllerBase
    {
        private readonly ISonarQubeSvc _sonarQubeSvc;
        private readonly IShieldIoSvc _shieldIoSvc;
        private readonly IExtractor<ShieldsColors> _sonarQubeColorCORP;
        private readonly IExtractor<string> _sonarQubeValueExtractor;

        public BadgesController(
            ISonarQubeSvc sonarQubeSvc,
            IShieldIoSvc shieldIoSvc,
            IExtractor<ShieldsColors> sonarQubeColorCORP,
            IExtractor<string> sonarQubeValueExtractor)
        {
            _sonarQubeSvc = sonarQubeSvc;
            _shieldIoSvc = shieldIoSvc;
            _sonarQubeColorCORP = sonarQubeColorCORP;
            _sonarQubeValueExtractor = sonarQubeValueExtractor;
        }

        [HttpGet()]
        public async Task<ActionResult<System.IO.Stream>> Get(
            [FromQuery]string key,
            [FromQuery]Metric metric,
            [FromQuery]string branch = null,
            [FromQuery]string label = null)
        {
            string value;
            ShieldsColors color = ShieldsColors.YellowGreen;
            string officialLabel = null;

            if (metric == Metric.QualityGate)
            {
                label = "Quality Gate";
                metric = Metric.QualityGateDetails;
            }

            var res = await _sonarQubeSvc.FetchStatsAsync(key, branch, metric);
            var currentMeasure = res.Component.Measures.FirstOrDefault();
            if (currentMeasure == null)
            {
                value = "---";
                color = ShieldsColors.Yellow;
            }
            else
            {
                var currentMetric = res.Metrics.FirstOrDefault(f => f.Key == metric.ToSnakeCase());
                officialLabel = label ?? currentMetric.Name;

                color = _sonarQubeColorCORP.Extract(currentMeasure, currentMetric);
                value = _sonarQubeValueExtractor.Extract(currentMeasure, currentMetric);
            }

            if (string.IsNullOrEmpty(officialLabel))
            {
                officialLabel = label ?? metric.ToSnakeCase().Replace('_', ' ');
            }

            var svgContent = await _shieldIoSvc.GetSvgBadgeAsync(officialLabel, value, color);

            return Content(svgContent, "image/svg+xml; charset=utf-8");
        }
    }
}
