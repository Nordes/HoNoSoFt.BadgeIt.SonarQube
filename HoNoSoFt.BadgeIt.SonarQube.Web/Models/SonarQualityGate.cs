using System;
using System.Collections.Generic;

namespace HoNoSoFt.BadgeIt.SonarQube.Web.Models
{
    public class Status
    {
        public string QualityGateStatus { get; set; }
    }

    public class Branch
    {
        public string Name { get; set; }
        public bool IsMain { get; set; }
        public string Type { get; set; }
        public Status Status { get; set; }
        public DateTime AnalysisDate { get; set; }
    }

    public class SonarQualityGate
    {
        public List<Branch> Branches { get; set; }
    }
}