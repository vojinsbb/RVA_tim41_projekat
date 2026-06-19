using System.Collections.Generic;
using WaterQuality.InformationSystem.Models;

namespace WaterQuality.InformationSystem.Data
{
    public class WaterQualityDataStore
    {
        public List<WaterSource> WaterSources { get; set; }

        public List<WaterQualityReading> WaterQualityReadings { get; set; }

        public WaterQualityDataStore()
        {
            WaterSources = new List<WaterSource>();
            WaterQualityReadings = new List<WaterQualityReading>();
        }
    }
}