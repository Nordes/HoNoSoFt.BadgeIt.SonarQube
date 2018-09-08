using HoNoSoFt.BadgeIt.SonarQube.Web.Models;

namespace HoNoSoFt.BadgeIt.SonarQube.Web.Logics.SonarQubeColorsSteps
{
    internal class IsLowerThanTheBestColorStep : IColorStep
    {
        private IColorStep _successorTrue;
        private IColorStep _successorFalse;

        public IsLowerThanTheBestColorStep() { }

        public IsLowerThanTheBestColorStep(IColorStep successorFalse)
        {
            SetSuccessor(null, successorFalse);
        }

        public void SetSuccessor(IColorStep successorTrue, IColorStep successorFalse)
        {
            _successorTrue = successorTrue;
            _successorFalse = successorFalse;
        }

        public ShieldsColors Handle(Measure measure, SonarQubeMetric metric)
        {
            // Could have a null value :/ but not suppose to happen.
            return metric.SmallerOrEqualsThanValue(measure.Value ?? measure.Periods[0].Value, metric.BestValue).Value
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
