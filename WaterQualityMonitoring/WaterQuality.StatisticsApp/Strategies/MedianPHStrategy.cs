using System.Collections.Generic;
using System.Linq;
using WaterQuality.Contracts.DTOs;

namespace WaterQuality.StatisticsApp.Strategies
{
    public class MedianPHStrategy : IStatisticsStrategy
    {
        public string Name => "Medijalna vrednost pH";

        public string Calculate(List<WaterQualityReadingDto> readings)
        {
            if (readings == null || readings.Count == 0)
                return "Nema podataka za izračunavanje.";

            var values = readings.Select(r => r.PHLevel).OrderBy(x => x).ToList();
            int count = values.Count;

            double median = count % 2 == 1
                ? values[count / 2]
                : (values[count / 2 - 1] + values[count / 2]) / 2.0;

            return $"Medijalna vrednost pH: {median:F2}";
        }
    }
}