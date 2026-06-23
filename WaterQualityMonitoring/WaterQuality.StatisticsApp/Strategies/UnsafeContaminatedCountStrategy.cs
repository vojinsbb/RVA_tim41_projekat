using System.Collections.Generic;
using System.Linq;
using WaterQuality.Contracts.DTOs;
using WaterQuality.Contracts.Enums;

namespace WaterQuality.StatisticsApp.Strategies
{
    public class UnsafeContaminatedCountStrategy : IStatisticsStrategy
    {
        public string Name => "Broj Unsafe i Contaminated uzoraka";

        public string Calculate(List<WaterQualityReadingDto> readings)
        {
            if (readings == null || readings.Count == 0)
                return "Nema podataka za izračunavanje.";

            int count = readings.Count(r =>
                r.State == WaterState.Unsafe ||
                r.State == WaterState.Contaminated);

            return $"Broj nebezbednih ili kontaminiranih uzoraka: {count}";
        }
    }
}