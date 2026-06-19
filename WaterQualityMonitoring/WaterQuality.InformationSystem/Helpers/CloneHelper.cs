using WaterQuality.InformationSystem.Models;

namespace WaterQuality.InformationSystem.Helpers
{
    public static class CloneHelper
    {
        public static WaterSource CloneWaterSource(WaterSource source)
        {
            if (source == null)
            {
                return null;
            }

            return new WaterSource
            {
                Id = source.Id,
                Name = source.Name,
                Location = source.Location,
                SourceType = source.SourceType,
                Municipality = source.Municipality,
                CapacityM3 = source.CapacityM3
            };
        }

        public static WaterQualityReading CloneWaterQualityReading(WaterQualityReading reading)
        {
            if (reading == null)
            {
                return null;
            }

            return new WaterQualityReading
            {
                Id = reading.Id,
                SourceId = reading.SourceId,
                SampledAt = reading.SampledAt,
                PHLevel = reading.PHLevel,
                TurbidityNTU = reading.TurbidityNTU,
                ChlorineLevel = reading.ChlorineLevel,
                State = reading.State
            };
        }
    }
}