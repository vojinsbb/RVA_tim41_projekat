using System;
using System.Runtime.Serialization;

namespace WaterQuality.Contracts.DTOs
{
    [DataContract]
    public class WaterSourceDto
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public string SourceType { get; set; }

        [DataMember]
        public string Municipality { get; set; }

        [DataMember]
        public double CapacityM3 { get; set; }

        public WaterSourceDto()
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