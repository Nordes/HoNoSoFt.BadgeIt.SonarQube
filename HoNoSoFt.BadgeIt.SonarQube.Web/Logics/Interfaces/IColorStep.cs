using HoNoSoFt.BadgeIt.SonarQube.Web.Models;

namespace HoNoSoFt.BadgeIt.SonarQube.Web.Logics.SonarQubeColorsSteps
{
    internal interface IColorStep
    {
        void SetSuccessor(IColorStep successorTrue, IColorStep successorFalse);
        ShieldsColors Handle(Measure measure, SonarQubeMetric metric);
        bool ValidateSetup();
    }
}