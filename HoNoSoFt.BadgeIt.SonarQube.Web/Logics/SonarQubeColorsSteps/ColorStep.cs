using HoNoSoFt.BadgeIt.SonarQube.Web.Models;

namespace HoNoSoFt.BadgeIt.SonarQube.Web.Logics.SonarQubeColorsSteps
{
    internal class ColorStep : IColorStep
    {
        private readonly ShieldsColors _color;

        public ColorStep(ShieldsColors color)
        {
            _color = color;
        }

        public ShieldsColors Handle(Measure measure, SonarQubeMetric metric)
        {
            return _color;
        }

        public void SetSuccessor(IColorStep successorTrue, IColorStep successorFalse)
        {
            // do nothing
        }

        public bool ValidateSetup()
        {
            return true;
        }
    }
}