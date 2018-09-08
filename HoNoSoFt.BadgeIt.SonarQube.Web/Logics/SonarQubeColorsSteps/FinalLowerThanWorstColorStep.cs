using HoNoSoFt.BadgeIt.SonarQube.Web.Models;

namespace HoNoSoFt.BadgeIt.SonarQube.Web.Logics.SonarQubeColorsSteps
{
    internal class FinalLowerThanWorstColorStep : IColorStep
    {
        private IColorStep _successorTrue;
        private IColorStep _successorFalse;

        public FinalLowerThanWorstColorStep() { }

        public void SetSuccessor(IColorStep successorTrue, IColorStep successorFalse)
        {
            _successorTrue = successorTrue;
            _successorFalse = successorFalse;
        }

        public ShieldsColors Handle(Measure measure, SonarQubeMetric metric)
        {
            // Could have a null value :/ but not suppose to happen.
            return metric.SmallerThanValue(measure.Value ?? measure.Periods[0].Value, metric.WorstValue).Value
                ? ShieldsColors.Orange
                : ShieldsColors.Red;
        }

        public bool ValidateSetup()
        {
            return _successorFalse == null
                && _successorTrue == null;
        }
    }
}
