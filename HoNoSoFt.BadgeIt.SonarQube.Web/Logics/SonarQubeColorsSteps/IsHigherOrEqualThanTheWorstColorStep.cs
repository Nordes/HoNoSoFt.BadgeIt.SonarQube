using HoNoSoFt.BadgeIt.SonarQube.Web.Models;

namespace HoNoSoFt.BadgeIt.SonarQube.Web.Logics.SonarQubeColorsSteps
{
    internal class IsHigherOrEqualThanTheWorstColorStep : IColorStep
    {
        private IColorStep _successorTrue;
        private IColorStep _successorFalse;

        public IsHigherOrEqualThanTheWorstColorStep() { }

        public IsHigherOrEqualThanTheWorstColorStep(IColorStep successorTrue, IColorStep successorFalse)
        {
            SetSuccessor(successorTrue, successorFalse);
        }

        public void SetSuccessor(IColorStep successorTrue, IColorStep successorFalse)
        {
            _successorTrue = successorTrue;
            _successorFalse = successorFalse;
        }

        public ShieldsColors Handle(Measure measure, SonarQubeMetric metric)
        {
            // Could have a null value :/ but not suppose to happen.
            return string.IsNullOrEmpty(metric.WorstValue) 
                || metric.HigherOrEqualsThanValue(measure.Value ?? measure.Periods[0].Value, metric.WorstValue).Value
                ? _successorTrue.Handle(measure, metric)
                : _successorFalse.Handle(measure, metric);
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
