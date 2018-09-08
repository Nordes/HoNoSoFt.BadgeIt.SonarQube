using HoNoSoFt.BadgeIt.SonarQube.Web.Models;

namespace HoNoSoFt.BadgeIt.SonarQube.Web.Logics.SonarQubeColorsSteps
{
    internal class FinalHigherThanTheBestColorStep : IColorStep
    {
        private IColorStep _successorTrue;
        private IColorStep _successorFalse;

        public FinalHigherThanTheBestColorStep() { }

        public FinalHigherThanTheBestColorStep(IColorStep successorTrue, IColorStep successorFalse)
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
            var isThisTrue = !metric.SmallerOrEqualsThanValue(measure.Value ?? measure.Periods[0].Value, metric.BestValue);

            return isThisTrue.HasValue && isThisTrue.Value
                ? _successorTrue.Handle(measure, metric) // ShieldsColors.Green
                : _successorFalse.Handle(measure, metric); // ShieldsColors.Orange;
        }

        public bool ValidateSetup()
        {
            return _successorTrue != null && _successorTrue.ValidateSetup()
                && _successorFalse != null && _successorFalse.ValidateSetup();
        }
    }
}
