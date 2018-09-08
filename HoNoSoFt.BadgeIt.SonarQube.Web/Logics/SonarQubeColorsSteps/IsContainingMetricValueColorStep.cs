using HoNoSoFt.BadgeIt.SonarQube.Web.Models;

namespace HoNoSoFt.BadgeIt.SonarQube.Web.Logics.SonarQubeColorsSteps
{
    internal class IsContainingMetricValueColorStep : IColorStep
    {
        private IColorStep _successorTrue;
        private IColorStep _successorFalse;

        public IsContainingMetricValueColorStep() { }

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
                return _successorFalse.Handle(measure, metric); // ShieldsColors.LightGrey;
            }

            return _successorTrue.Handle(measure, metric);
        }

        public bool ValidateSetup()
        {
            return _successorTrue != null
              && _successorTrue.ValidateSetup()
                && _successorFalse != null
              && _successorFalse.ValidateSetup();
        }
    }
}
