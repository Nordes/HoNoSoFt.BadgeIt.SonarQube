using HoNoSoFt.BadgeIt.SonarQube.Web.Logics;
using HoNoSoFt.BadgeIt.SonarQube.Web.Models;
using HoNoSoFt.XUnit.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq;
using Xunit;

namespace HoNoSoFt.BadgeIt.SonarQube.Web.UnitTests.Logics
{
    public class SonarQubeColorExtractorTests
    {
        private readonly ILogger<SonarQubeColorExtractor> _logger;

        private readonly SonarQubeColorExtractor _extractor;

        public SonarQubeColorExtractorTests()
        {
            _logger = Mock.Of<ILogger<SonarQubeColorExtractor>>();
            _extractor = new SonarQubeColorExtractor();
        }

        [Theory]
        [JsonFileData("_assets/SonarColors/levelColor_1_ok.json", typeof(SonarQubeStats), ShieldsColors.YellowGreen)]
        [JsonFileData("_assets/SonarColors/levelColor_2_error.json", typeof(SonarQubeStats), ShieldsColors.Red)]
        [JsonFileData("_assets/SonarColors/levelColor_3_warning.json", typeof(SonarQubeStats), ShieldsColors.Orange)]
        public void Extract_WhenLevel_ExpectMatchingColor(JsonData jsonData, ShieldsColors expectedValue)
        {
            var stats = (SonarQubeStats)jsonData.Data;
            var result = _extractor.Extract(stats.Component.Measures.First(), stats.Metrics.First());
            
            Assert.Equal(expectedValue, result);
        }

        [Theory]
        // Rating + lower is better
        [JsonFileData("_assets/SonarColors/ratingColor_1_1-5-green.json", typeof(SonarQubeStats), ShieldsColors.YellowGreen)]
        [JsonFileData("_assets/SonarColors/ratingColor_1_2-5-orange.json", typeof(SonarQubeStats), ShieldsColors.Orange)]
        [JsonFileData("_assets/SonarColors/ratingColor_1_4-5-orange.json", typeof(SonarQubeStats), ShieldsColors.Orange)]
        [JsonFileData("_assets/SonarColors/ratingColor_1_5-5-Red.json", typeof(SonarQubeStats), ShieldsColors.Red)]
        // Rating + higher is better
        [JsonFileData("_assets/SonarColors/ratingColor_1_5-5-Green.json", typeof(SonarQubeStats), ShieldsColors.YellowGreen)]
        [JsonFileData("_assets/SonarColors/ratingColor_1_1-5-red.json", typeof(SonarQubeStats), ShieldsColors.Red)]
        public void Extract_WhenRating_ExpectMatchingColor(JsonData jsonData, ShieldsColors expectedValue)
        {
            var stats = (SonarQubeStats)jsonData.Data;
            var result = _extractor.Extract(stats.Component.Measures.First(), stats.Metrics.First());

            Assert.Equal(expectedValue, result);
        }

        [Theory]
        [JsonFileData("_assets/SonarColors/percentColor_1_0-green.json", typeof(SonarQubeStats), ShieldsColors.YellowGreen)]
        [JsonFileData("_assets/SonarColors/percentColor_1_1-orange.json", typeof(SonarQubeStats), ShieldsColors.Orange)]
        [JsonFileData("_assets/SonarColors/percentColor_1_99-orange.json", typeof(SonarQubeStats), ShieldsColors.Orange)]
        [JsonFileData("_assets/SonarColors/percentColor_1_100-red.json", typeof(SonarQubeStats), ShieldsColors.Red)]
        public void Extract_WhenPercent_ExpectMatchingColor(JsonData jsonData, ShieldsColors expectedValue)
        {
            var stats = (SonarQubeStats)jsonData.Data;
            var result = _extractor.Extract(stats.Component.Measures.First(), stats.Metrics.First());

            Assert.Equal(expectedValue, result);
        }

        [Theory]
        [JsonFileData("_assets/SonarColors/intColor_1_domainSize.json", typeof(SonarQubeStats))]
        public void Extract_WhenDomainSize_ExpectLightgray(JsonData jsonData)
        {
            var stats = (SonarQubeStats)jsonData.Data;
            var result = _extractor.Extract(stats.Component.Measures.First(), stats.Metrics.First());

            Assert.Equal(ShieldsColors.LightGrey, result);
        }

        [Theory]
        [JsonFileData("_assets/SonarColors/intColor_2_lowerIsBetter_green.json", typeof(SonarQubeStats), ShieldsColors.YellowGreen)]
        [JsonFileData("_assets/SonarColors/intColor_3_higherThanBest_red.json", typeof(SonarQubeStats), ShieldsColors.Red)]
        public void Extract_WhenInt_ExpectMatchingColor(JsonData jsonData, ShieldsColors expectedValue)
        {
            // LowerIsBetter is tested, normally we don't have the other way around with integer.
            var stats = (SonarQubeStats)jsonData.Data;
            var result = _extractor.Extract(stats.Component.Measures.First(), stats.Metrics.First());

            Assert.Equal(expectedValue, result);
        }

        // DATA
        //// data quality gate
        //// data ... c# (LOCs per language)

        // WORK_DUR
        //// something
    }
}
