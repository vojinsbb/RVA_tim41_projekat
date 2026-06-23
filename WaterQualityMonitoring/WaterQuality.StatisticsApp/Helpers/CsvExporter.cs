using System.Collections.Generic;
using System.IO;
using System.Text;
using WaterQuality.Contracts.DTOs;

namespace WaterQuality.StatisticsApp.Helpers
{
    public static class CsvExporter
    {
        public static void ExportReadings(string filePath, List<WaterQualityReadingDto> readings, string statisticsResult)
        {
            StringBuilder csv = new StringBuilder();

            csv.AppendLine("Id,SourceId,SampledAt,PHLevel,TurbidityNTU,ChlorineLevel,State");

            foreach (var reading in readings)
            {
                csv.AppendLine($"{reading.Id},{reading.SourceId},{reading.SampledAt},{reading.PHLevel},{reading.TurbidityNTU},{reading.ChlorineLevel},{reading.State}");
            }

            csv.AppendLine();
            csv.AppendLine("StatisticsResult");
            csv.AppendLine(statisticsResult);

            File.WriteAllText(filePath, csv.ToString(), Encoding.UTF8);
        }
    }
}