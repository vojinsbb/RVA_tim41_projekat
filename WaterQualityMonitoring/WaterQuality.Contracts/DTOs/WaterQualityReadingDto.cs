using System;
using WaterQuality.Contracts.Enums;

namespace WaterQuality.Contracts.DTOs
{
    public class WaterQualityReadingDto
    {
        public Guid Id { get; set; }

        public Guid SourceId { get; set; }

        public DateTime SampledAt { get; set; }

        public double PHLevel { get; set; }

        public double TurbidityNTU { get; set; }

        public double ChlorineLevel { get; set; }

        public WaterState State { get; set; }

        public WaterQualityReadingDto()
        {
            Id = Guid.NewGuid();
            SourceId = Guid.Empty;
            SampledAt = DateTime.Now;
            PHLevel = 7.0;
            TurbidityNTU = 0;
            ChlorineLevel = 0;
            State = WaterState.Safe;
        }
    }
}