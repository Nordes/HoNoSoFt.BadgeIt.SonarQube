using HoNoSoFt.BadgeIt.SonarQube.Web.Logics.Interfaces;
using HoNoSoFt.BadgeIt.SonarQube.Web.Logics.SonarQubeColorsSteps;
using HoNoSoFt.BadgeIt.SonarQube.Web.Models;
using Newtonsoft.Json.Linq;
using System;

namespace HoNoSoFt.BadgeIt.SonarQube.Web.Logics
{
    /// <summary>
    /// Use CORP = Chain Of Responsibility Pattern in order to achieve
    /// most of the colors.
    /// </summary>
    internal class SonarQubeColorExtractor : IExtractor<ShieldsColors>
    {
        private readonly ColorStep _redColor = new ColorStep(ShieldsColors.Red);
        private readonly ColorStep _orangeColorStep = new ColorStep(ShieldsColors.Orange);
        private readonly ColorStep _greenColorStep = new ColorStep(ShieldsColors.YellowGreen);
        private readonly ColorStep _lightGreyColorStep = new ColorStep(ShieldsColors.LightGrey);
        private readonly IColorStep _numericValuePipeline;

        public SonarQubeColorExtractor()
        {
            // Todo create the setup to fill private IColorStep that will be used in a later execution
            _numericValuePipeline = NumericValuePipeline();
        }

        public ShieldsColors Extract(Measure measure, SonarQubeMetric metric)
        {
            switch (metric.Type)
            {
                case SonarQubeMetric.MeasureType.Int:
                case SonarQubeMetric.MeasureType.Percent:
                case SonarQubeMetric.MeasureType.Rating: // 1..5 = A, B, C, D, E
                case SonarQubeMetric.MeasureType.Work_Dur:
                    return _numericValuePipeline.Handle(measure, metric);
                case SonarQubeMetric.MeasureType.Level:
                    return LevelValuePipeline(measure.Value);
                case SonarQubeMetric.MeasureType.Data:
                    // Special
                    if (metric.Key.Equals(Metric.QualityGateDetails.ToSnakeCase(), StringComparison.OrdinalIgnoreCase))
                    {
                        var jToken = JObject.Parse(measure.Value);
                        var status = (string)jToken.SelectToken("level");
                        // Basically : OK or ERROR
                        return LevelValuePipeline(status);
                    }

                    return ShieldsColors.LightGrey;
            }

            // Should not happen. We should probably add an exception.
            return ShieldsColors.LightGrey;
        }

        private static ShieldsColors LevelValuePipeline(string value)
        {
            // Basically : OK or ERROR
            if (value.Equals("Error", StringComparison.OrdinalIgnoreCase))
            {
                return ShieldsColors.Red;
            }
            else if (value.Equals("Warn", StringComparison.OrdinalIgnoreCase))
            {
                return ShieldsColors.Orange;
            }

            return ShieldsColors.YellowGreen;
        }

        private IColorStep NumericValuePipeline()
        {
            var isDomainSize = new IsDomainSize();
            var isQualitativeColorStep = new IsQualitativeColorStep();
            var isHigherAreBetterColorStep = new IsHigherAreBetterColorStep();
            var isContainingMetricValueColorStep = new IsContainingMetricValueColorStep();
            var finalBestValueColorStep = new FinalBestValueColorStep(); // need to be reviewed
            var isLowerOrEqualThanTheBest = new IsLowerThanTheBestColorStep();
            var finalLowerThanWorstStep = new FinalLowerThanWorstColorStep();
            var isHigherOrEqualThanWorstStep = new IsHigherOrEqualThanTheWorstColorStep();
            var finalHigherThanTheBestStep = new FinalHigherThanTheBestColorStep();

            // Do flow for number value
            isDomainSize.SetSuccessor(_lightGreyColorStep, isQualitativeColorStep);
            isQualitativeColorStep.SetSuccessor(isHigherAreBetterColorStep, isContainingMetricValueColorStep);
            isContainingMetricValueColorStep.SetSuccessor(finalBestValueColorStep, _lightGreyColorStep);
            finalBestValueColorStep.SetSuccessor(null, null); // to be reviewed
            isHigherAreBetterColorStep.SetSuccessor(isHigherOrEqualThanWorstStep, isLowerOrEqualThanTheBest); // From here
            isLowerOrEqualThanTheBest.SetSuccessor(_greenColorStep, finalLowerThanWorstStep);
            isHigherOrEqualThanWorstStep.SetSuccessor(_greenColorStep, finalHigherThanTheBestStep);
            finalHigherThanTheBestStep.SetSuccessor(_orangeColorStep, _redColor);

            if (!isDomainSize.ValidateSetup())
            {
                throw new Exception("Setup is invalid.");
            }

            return isDomainSize;
        }
    }
}