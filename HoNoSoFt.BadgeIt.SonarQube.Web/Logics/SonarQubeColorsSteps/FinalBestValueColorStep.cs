using HoNoSoFt.BadgeIt.SonarQube.Web.Models;
using System;

namespace HoNoSoFt.BadgeIt.SonarQube.Web.Logics.SonarQubeColorsSteps
{
    internal class FinalBestValueColorStep : IColorStep
    {
        private IColorStep _successorTrue;
        private IColorStep _successorFalse;

        public FinalBestValueColorStep() { }

        public void SetSuccessor(IColorStep successorTrue, IColorStep successorFalse)
        {
            _successorTrue = successorTrue;
            _successorFalse = successorFalse;
        }

        public ShieldsColors Handle(Measure measure, SonarQubeMetric metric)
        {
            var isThisTrue = metric.SmallerThanValue(measure.Value ?? measure.Periods[0].Value, metric.BestValue ?? "0");
            if (string.IsNullOrEmpty(metric.BestValue) || !isThisTrue.HasValue)
            {
                return ShieldsColors.LightGrey;
            }

            // We could also invert in case of higherIsBetter
            if (!metric.HigherValuesAreBetter)
            {
                return isThisTrue.Value || measure.Value.Equals(metric.BestValue, StringComparison.OrdinalIgnoreCase)
                    ? ShieldsColors.YellowGreen
                    : ShieldsColors.Red;
            }

            return isThisTrue.Value
                ? ShieldsColors.Red
                : ShieldsColors.YellowGreen;
        }

        public bool ValidateSetup()
        {
            return _successorTrue == null
                && _successorFalse == null;
        }
    }
}
