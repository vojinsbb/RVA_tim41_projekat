using System;
using System.Runtime.Serialization;
using WaterQuality.Contracts.Enums;

namespace WaterQuality.Contracts.DTOs
{
    [DataContract]
    public class WaterQualityReadingDto
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public Guid SourceId { get; set; }

        [DataMember]
        public DateTime SampledAt { get; set; }

        [DataMember]
        public double PHLevel { get; set; }

        [DataMember]
        public double TurbidityNTU { get; set; }

        [DataMember]
        public double ChlorineLevel { get; set; }

        [DataMember]
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