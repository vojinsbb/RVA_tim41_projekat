using System.Collections.Generic;
using WaterQuality.Contracts.DTOs;

namespace WaterQuality.StatisticsApp.Strategies
{
    public interface IStatisticsStrategy
    {
        string Name { get; }
        string Calculate(List<WaterQualityReadingDto> readings);
    }
}