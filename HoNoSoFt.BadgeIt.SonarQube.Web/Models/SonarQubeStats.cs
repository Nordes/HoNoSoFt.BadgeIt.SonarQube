using System;
using System.Collections.Generic;

namespace HoNoSoFt.BadgeIt.SonarQube.Web.Models
{
    public class Period
    {
        public int Index { get; set; }
        public string Value { get; set; }
    }

    public class Measure
    {
        public string Metric { get; set; }
        public string Value { get; set; }
        public List<Period> Periods { get; set; }
    }

    public class SonarQubeMetric
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Domain { get; set; }
        public MeasureType Type { get; set; }
        public bool HigherValuesAreBetter { get; set; }
        public bool Qualitative { get; set; }
        public bool Hidden { get; set; }
        public bool Custom { get; set; }
        public int? DecimalScale { get; set; }
        public string BestValue { get; set; }
        public string WorstValue { get; set; }

        public bool? SmallerThanValue(string value, string valueToCompareWith)
        {
            var value2Compare = string.IsNullOrEmpty(valueToCompareWith) ? "0" : valueToCompareWith;
            switch (Type)
            {
                case MeasureType.Work_Dur:
                case MeasureType.Int:
                    return int.Parse(value) < int.Parse(value2Compare);
                case MeasureType.Percent:
                case MeasureType.Rating: // Goes from 1 to 5 (A, B, C, D, E)
                    return decimal.Parse(value) < decimal.Parse(value2Compare);
            }

            return null;
        }

        public bool? HigherOrEqualsThanValue(string value, string valueToCompareWith)
        {
            switch (Type)
            {
                case MeasureType.Work_Dur:
                case MeasureType.Int:
                    return int.Parse(value) >= int.Parse(valueToCompareWith);
                case MeasureType.Percent:
                case MeasureType.Rating: // Goes from 1 to 5 (A, B, C, D, E)
                    return decimal.Parse(value) >= decimal.Parse(valueToCompareWith);
            }

            return null;
        }

        public bool? SmallerOrEqualsThanValue(string value, string valueToCompareWith)
        {
            switch (Type)
            {
                case MeasureType.Work_Dur:
                case MeasureType.Int:
                    return int.Parse(value) <= int.Parse(valueToCompareWith);
                case MeasureType.Percent:
                case MeasureType.Rating: // Goes from 1 to 5 (A, B, C, D, E)
                    return decimal.Parse(value) <= decimal.Parse(valueToCompareWith);
            }

            return null;
        }

        public enum MeasureType
        {
            Int,
            Percent,
            Data,
            Work_Dur,
            Rating,
            Level,
        }
    }

    public class Component
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Qualifier { get; set; }
        public List<Measure> Measures { get; set; }
    }

    public class Period2
    {
        public int Index { get; set; }
        public string Mode { get; set; }
        public DateTime Date { get; set; }
        public string Parameter { get; set; }
    }

    public class SonarQubeStats
    {
        public Component Component { get; set; }
        public List<SonarQubeMetric> Metrics { get; set; }
        public List<Period2> Periods { get; set; }
    }
}