using HoNoSoFt.BadgeIt.SonarQube.Web.Models;
using System;

namespace HoNoSoFt.BadgeIt.SonarQube.Web.Logics.SonarQubeColorsSteps
{
    internal class IsDomainSize : IColorStep
    {
        private IColorStep _successorTrue;
        private IColorStep _successorFalse;

        public IsDomainSize() { }

        public IsDomainSize(IColorStep successorTrue, IColorStep successorFalse)
        {
            SetSuccessor(successorTrue, successorFalse);
        }

        public ShieldsColors Handle(Measure measure, SonarQubeMetric metric)
        {
            // TODO use an enum instead.
            return metric.Domain.Equals("Size", StringComparison.OrdinalIgnoreCase)
                ? _successorTrue.Handle(measure, metric)
                : _successorFalse.Handle(measure, metric);
        }

        public void SetSuccessor(IColorStep successorTrue, IColorStep successorFalse)
        {
            _successorTrue = successorTrue;
            _successorFalse = successorFalse;
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