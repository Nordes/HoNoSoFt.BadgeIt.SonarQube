using HoNoSoFt.BadgeIt.SonarQube.Web.Logics.Interfaces;
using HoNoSoFt.BadgeIt.SonarQube.Web.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace HoNoSoFt.BadgeIt.SonarQube.Web.Logics
{
    /// <summary>
    /// Sonar qube value extractor. The definition of the metric can be found at 
    /// https://docs.sonarqube.org/display/SONAR/Metric+Definitions
    /// </summary>
    internal class SonarQubeValueExtractor : IExtractor<string>
    {
        private readonly ILogger<SonarQubeValueExtractor> _logger;

        public SonarQubeValueExtractor(ILogger<SonarQubeValueExtractor> logger)
        {
            _logger = logger;
        }

        public string Extract(Measure measure, SonarQubeMetric metric)
        {
            if (measure == null)
            {
                throw new ArgumentNullException(nameof(measure));
            }

            if (metric == null)
            {
                throw new ArgumentNullException(nameof(metric));
            }

            switch (metric.Type)
            {
                case SonarQubeMetric.MeasureType.Int:
                    return ProcessInt(measure);
                case SonarQubeMetric.MeasureType.Percent:
                    return ProcessPercent(measure);
                case SonarQubeMetric.MeasureType.Rating:
                    return ProcessRating(measure, metric);
                case SonarQubeMetric.MeasureType.Work_Dur:
                    return ProcessWorkDur(measure);
                case SonarQubeMetric.MeasureType.Level:
                    return ProcessLevel(measure).ToString();
                case SonarQubeMetric.MeasureType.Data:
                    return ProcessData(measure, metric);
            }

            return measure.Value ?? measure.Periods.FirstOrDefault()?.Value ?? "Unknown";
        }

        private static string ProcessWorkDur(Measure measure)
        {
            return measure.Value ?? measure.Periods.FirstOrDefault()?.Value ?? "0";
        }

        private static string ProcessInt(Measure measure)
        {
            return measure.Value ?? measure.Periods.FirstOrDefault()?.Value ?? "0";
        }

        private static string ProcessPercent(Measure measure)
        {
            var val = measure.Value ?? measure.Periods.FirstOrDefault()?.Value ?? "0";
            var finalVal = double.Parse(val);
            return $"{finalVal}%";
        }

        /// <summary>
        /// Processes the rating.
        /// 1..5 = A, B, C, D, E
        /// </summary>
        /// <param name="measure">The measure.</param>
        /// <param name="metric">The metric.</param>
        /// <returns>Return the letter based value.</returns>
        private static string ProcessRating(Measure measure, SonarQubeMetric metric)
        {
            char rating;
            if (metric.HigherValuesAreBetter)
            {
                var startAt = 64 + (int)decimal.Parse(metric.WorstValue);
                rating = (char)(startAt + (int)decimal.Parse(metric.BestValue) - (int)decimal.Parse(measure.Value ?? measure.Periods.First().Value));
            }
            else
            {
                rating = (char)(64 + (int)decimal.Parse(measure.Value ?? measure.Periods.First().Value));
            }

            return rating.ToString();
        }

        private static LevelType ProcessLevel(Measure measure)
        {
            return GetLevel(measure.Value);
        }

        private static LevelType GetLevel(string value)
        {
            // Basically : OK or ERROR
            if (value.Equals("Error", StringComparison.OrdinalIgnoreCase))
            {
                return LevelType.Error;
            }
            else if (value.Equals("Warn", StringComparison.OrdinalIgnoreCase))
            {
                return LevelType.Warning;
            }

            return LevelType.Ok;
        }

        private static string ProcessData(Measure measure, SonarQubeMetric metric)
        {
            // Quality gate
            if (metric.Key.Equals(Metric.QualityGateDetails.ToSnakeCase(), StringComparison.OrdinalIgnoreCase))
            {
                var jToken = JObject.Parse(measure.Value);
                var status = (string)jToken.SelectToken("level");
                // Basically : OK, warning or ERROR
                return GetLevel(status).ToString();
            }

            return measure.Value;
        }

        private enum LevelType
        {
            Error,
            Warning,
            Ok,
        }
    }
}