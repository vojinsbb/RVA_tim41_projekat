using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WaterQuality.InformationSystem.Models;

namespace WaterQuality.InformationSystem.Repositories
{
    public interface IWaterQualityReadingRepository
    {
        ObservableCollection<WaterQualityReading> GetAll();

        ObservableCollection<WaterQualityReading> GetBySourceId(Guid sourceId);

        ObservableCollection<WaterQualityReading> GetBySourceIdAndYear(Guid sourceId, int year);

        WaterQualityReading GetById(Guid id);

        void Add(WaterQualityReading reading);

        void Update(WaterQualityReading reading);

        void Delete(Guid id);

        void ReplaceAll(IEnumerable<WaterQualityReading> readings);
    }
}