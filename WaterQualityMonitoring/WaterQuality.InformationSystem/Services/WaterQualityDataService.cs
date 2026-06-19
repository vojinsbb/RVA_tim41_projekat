using System;
using System.Collections.Generic;
using System.Linq;
using WaterQuality.Contracts.DTOs;
using WaterQuality.Contracts.Services;
using WaterQuality.InformationSystem.Data;
using WaterQuality.InformationSystem.Helpers;

namespace WaterQuality.InformationSystem.Services
{
    public class WaterQualityDataService : IWaterQualityDataService
    {
        private readonly IDataPersistenceService _dataPersistenceService;

        public WaterQualityDataService()
        {
            _dataPersistenceService = new JsonDataPersistenceService();
        }

        public List<WaterSourceDto> GetAllSources()
        {
            WaterQualityDataStore dataStore = _dataPersistenceService.LoadData();

            return dataStore.WaterSources
                .Select(DtoMapper.ToDto)
                .ToList();
        }

        public List<WaterQualityReadingDto> GetReadingsBySourceAndYear(Guid sourceId, int year)
        {
            WaterQualityDataStore dataStore = _dataPersistenceService.LoadData();

            return dataStore.WaterQualityReadings
                .Where(reading =>
                    reading.SourceId == sourceId &&
                    reading.SampledAt.Year == year)
                .Select(DtoMapper.ToDto)
                .ToList();
        }
    }
}