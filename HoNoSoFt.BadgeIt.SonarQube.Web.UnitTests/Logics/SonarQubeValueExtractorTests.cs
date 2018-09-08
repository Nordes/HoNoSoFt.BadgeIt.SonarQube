using System.Linq;
using HoNoSoFt.BadgeIt.SonarQube.Web.Logics;
using HoNoSoFt.BadgeIt.SonarQube.Web.Models;
using HoNoSoFt.XUnit.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HoNoSoFt.BadgeIt.SonarQube.Web.UnitTests.Logics
{
    public class SonarQubeValueExtractorTests
    {
        private readonly ILogger<SonarQubeValueExtractor> _logger;

        private readonly SonarQubeValueExtractor _extractor;

        public SonarQubeValueExtractorTests()
        {
            _logger = Mock.Of<ILogger<SonarQubeValueExtractor>>();
            _extractor = new SonarQubeValueExtractor(_logger);
        }

        [Theory]
        [JsonFileData("_assets/SonarValues/intValue_1_asPeriodValue.json", typeof(SonarQubeStats), "1")] // Use the period if the value not present
        [JsonFileData("_assets/SonarValues/intValue_2_asBaseValue.json", typeof(SonarQubeStats), "2")] // Use the value if present
        [JsonFileData("_assets/SonarValues/percentValue_1.json", typeof(SonarQubeStats), "5.6%")] // Use value if present
        [JsonFileData("_assets/SonarValues/percentValue_2_fallBackPeriodNoDecimal.json", typeof(SonarQubeStats), "1%")] // Use the period if value not present
        [JsonFileData("_assets/SonarValues/ratingValue_1.json", typeof(SonarQubeStats), "A")] // 1 = A (best value is lower)
        [JsonFileData("_assets/SonarValues/ratingValue_2_higherValueAreBetter.json", typeof(SonarQubeStats), "E")] // 1 = E (worst value is lower)
        [JsonFileData("_assets/SonarValues/workDurValue_1.json", typeof(SonarQubeStats), "8567")] // Use the value if present
        [JsonFileData("_assets/SonarValues/workDurValue_2.json", typeof(SonarQubeStats), "70")] // Fallback to period if no value
        [JsonFileData("_assets/SonarValues/levelValue_1_ok.json", typeof(SonarQubeStats), "Ok")]
        [JsonFileData("_assets/SonarValues/levelValue_2_error.json", typeof(SonarQubeStats), "Error")]
        [JsonFileData("_assets/SonarValues/levelValue_3_warning.json", typeof(SonarQubeStats), "Warning")]
        [JsonFileData("_assets/SonarValues/dataQualityGate_1_levelError.json", typeof(SonarQubeStats), "Error")]
        [JsonFileData("_assets/SonarValues/dataNcLocDistribution_1.json", typeof(SonarQubeStats), "cs=7241")]
        public void Extract_WhenTyped_ExpectValiTypeOutput(JsonData jsonData, string expectedValue)
        {
            var stats = (SonarQubeStats)jsonData.Data;
            var result = _extractor.Extract(stats.Component.Measures.First(), stats.Metrics.First());

            Assert.Equal(expectedValue, result);
        }
    }
}
