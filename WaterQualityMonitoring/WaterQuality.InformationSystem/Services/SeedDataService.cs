using System;
using WaterQuality.Contracts.Enums;
using WaterQuality.InformationSystem.Models;
using WaterQuality.InformationSystem.Repositories;

namespace WaterQuality.InformationSystem.Services
{
    public class SeedDataService
    {
        private readonly IWaterSourceRepository _sourceRepository;
        private readonly IWaterQualityReadingRepository _readingRepository;

        public SeedDataService(
            IWaterSourceRepository sourceRepository,
            IWaterQualityReadingRepository readingRepository)
        {
            _sourceRepository = sourceRepository;
            _readingRepository = readingRepository;
        }

        public void LoadInitialData()
        {
            WaterSource source1 = new WaterSource
            {
                Id = Guid.NewGuid(),
                Name = "Dunav - Novi Sad",
                Location = "Novi Sad",
                SourceType = "Reka",
                Municipality = "Novi Sad",
                CapacityM3 = 500000
            };

            WaterSource source2 = new WaterSource
            {
                Id = Guid.NewGuid(),
                Name = "Palićko jezero",
                Location = "Palić",
                SourceType = "Jezero",
                Municipality = "Subotica",
                CapacityM3 = 120000
            };

            WaterSource source3 = new WaterSource
            {
                Id = Guid.NewGuid(),
                Name = "Gradski bunar Sombor",
                Location = "Sombor",
                SourceType = "Bunar",
                Municipality = "Sombor",
                CapacityM3 = 30000
            };

            _sourceRepository.Add(source1);
            _sourceRepository.Add(source2);
            _sourceRepository.Add(source3);

            _readingRepository.Add(new WaterQualityReading
            {
                Id = Guid.NewGuid(),
                SourceId = source1.Id,
                SampledAt = new DateTime(2025, 3, 15, 10, 30, 0),
                PHLevel = 7.2,
                TurbidityNTU = 1.3,
                ChlorineLevel = 0.8,
                State = WaterState.Safe
            });

            _readingRepository.Add(new WaterQualityReading
            {
                Id = Guid.NewGuid(),
                SourceId = source2.Id,
                SampledAt = new DateTime(2025, 5, 20, 9, 0, 0),
                PHLevel = 6.8,
                TurbidityNTU = 4.5,
                ChlorineLevel = 0.4,
                State = WaterState.Acceptable
            });

            _readingRepository.Add(new WaterQualityReading
            {
                Id = Guid.NewGuid(),
                SourceId = source3.Id,
                SampledAt = new DateTime(2024, 11, 5, 12, 15, 0),
                PHLevel = 6.1,
                TurbidityNTU = 8.7,
                ChlorineLevel = 0.2,
                State = WaterState.Unsafe
            });
        }
    }
}