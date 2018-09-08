using System;

namespace HoNoSoFt.BadgeIt.SonarQube.Web.Models
{
    [Flags]
    public enum Metric
    {
        QualityGate = 0,
        Vulnerabilities = 1,
        SqaleIndex = 2,
        CodeSmells = 4,
        Coverage = 8,
        DuplicatedLinesDensity = 16,
        Bugs = 32,
        NewBugs = 64,
        NewVulnerabilities = 128,
        NewReliabilityRating = 256,
        NewSecurityRating = 512,
        Ncloc = 1024,
        SqaleRating = 2048,
        NewTechnicalDebt = 4096,
        NewCodeSmells = 8192,
        Tests = 16384,
        SecurityRating = 32768,
        NewMaintainabilityRating = 65536,
        AlertStatus = 131072,
        ReliabilityRating = 262144,
        QualityGateDetails = 524288,
        DuplicatedBlocks = 1048576,
        NclocLanguageDistribution = 2097152,
        NewLinesToCover = 4194304,
    }
}