using WaterQuality.Contracts.DTOs;
using WaterQuality.InformationSystem.Models;

namespace WaterQuality.InformationSystem.Helpers
{
    public static class DtoMapper
    {
        public static WaterSourceDto ToDto(WaterSource source)
        {
            return new WaterSourceDto
            {
                Id = source.Id,
                Name = source.Name,
                Location = source.Location,
                SourceType = source.SourceType,
                Municipality = source.Municipality,
                CapacityM3 = source.CapacityM3
            };
        }

        public static WaterQualityReadingDto ToDto(WaterQualityReading reading)
        {
            return new WaterQualityReadingDto
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