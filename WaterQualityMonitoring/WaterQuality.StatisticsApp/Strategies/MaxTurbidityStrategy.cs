using System.Collections.Generic;
using System.Linq;
using WaterQuality.Contracts.DTOs;

namespace WaterQuality.StatisticsApp.Strategies
{
    public class MaxTurbidityStrategy : IStatisticsStrategy
    {
        public string Name => "Maksimalna mutnoća";

        public string Calculate(List<WaterQualityReadingDto> readings)
        {
            if (readings == null || readings.Count == 0)
                return "Nema podataka za izračunavanje.";

            double max = readings.Max(r => r.TurbidityNTU);

            return $"Maksimalna izmerena mutnoća: {max:F2} NTU";
        }
    }
}