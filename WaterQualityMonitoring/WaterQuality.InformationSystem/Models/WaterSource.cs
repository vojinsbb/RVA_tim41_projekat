using System;

namespace WaterQuality.InformationSystem.Models
{
    public class WaterSource
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public string SourceType { get; set; }

        public string Municipality { get; set; }

        public double CapacityM3 { get; set; }

        public WaterSource()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Location = string.Empty;
            SourceType = string.Empty;
            Municipality = string.Empty;
            CapacityM3 = 0;
        }
    }
}