using HoNoSoFt.BadgeIt.SonarQube.Web.Models;

namespace HoNoSoFt.BadgeIt.SonarQube.Web.Logics.SonarQubeColorsSteps
{
    internal class IsHigherAreBetterColorStep : IColorStep
    {
        private IColorStep _successorTrue;
        private IColorStep _successorFalse;

        public IsHigherAreBetterColorStep() { }

        public IsHigherAreBetterColorStep(IColorStep successorTrue, IColorStep successorFalse)
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
            return metric.HigherValuesAreBetter
                ? _successorTrue.Handle(measure, metric)
                : _successorFalse.Handle(measure, metric);
        }

        public bool ValidateSetup()
        {
            return _successorTrue != null
                && _successorFalse != null
                && _successorTrue.ValidateSetup()
                && _successorFalse.ValidateSetup();
        }
    }
}
