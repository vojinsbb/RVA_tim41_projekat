using System.Runtime.Serialization;

namespace WaterQuality.Contracts.Enums
{
    [DataContract]
    public enum WaterState
    {
        [EnumMember]
        Safe,

        [EnumMember]
        Acceptable,

        [EnumMember]
        Unsafe,

        [EnumMember]
        Contaminated
    }
}