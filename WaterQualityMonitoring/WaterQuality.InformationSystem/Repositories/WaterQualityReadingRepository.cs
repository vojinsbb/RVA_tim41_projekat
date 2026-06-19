using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WaterQuality.InformationSystem.Models;

namespace WaterQuality.InformationSystem.Repositories
{
    public class WaterQualityReadingRepository : IWaterQualityReadingRepository
    {
        private readonly ObservableCollection<WaterQualityReading> _readings;

        public WaterQualityReadingRepository()
        {
            _readings = new ObservableCollection<WaterQualityReading>();
        }

        public ObservableCollection<WaterQualityReading> GetAll()
        {
            return _readings;
        }

        public ObservableCollection<WaterQualityReading> GetBySourceId(Guid sourceId)
        {
            return new ObservableCollection<WaterQualityReading>(
                _readings.Where(reading => reading.SourceId == sourceId)
            );
        }

        public ObservableCollection<WaterQualityReading> GetBySourceIdAndYear(Guid sourceId, int year)
        {
            return new ObservableCollection<WaterQualityReading>(
                _readings.Where(reading =>
                    reading.SourceId == sourceId &&
                    reading.SampledAt.Year == year)
            );
        }

        public WaterQualityReading GetById(Guid id)
        {
            return _readings.FirstOrDefault(reading => reading.Id == id);
        }

        public void Add(WaterQualityReading reading)
        {
            _readings.Add(reading);
        }

        public void Update(WaterQualityReading reading)
        {
            WaterQualityReading existingReading = GetById(reading.Id);

            if (existingReading == null)
            {
                return;
            }

            existingReading.SourceId = reading.SourceId;
            existingReading.SampledAt = reading.SampledAt;
            existingReading.PHLevel = reading.PHLevel;
            existingReading.TurbidityNTU = reading.TurbidityNTU;
            existingReading.ChlorineLevel = reading.ChlorineLevel;
            existingReading.State = reading.State;
        }

        public void Delete(Guid id)
        {
            WaterQualityReading reading = GetById(id);

            if (reading == null)
            {
                return;
            }

            _readings.Remove(reading);
        }

        public void ReplaceAll(IEnumerable<WaterQualityReading> readings)
        {
            _readings.Clear();

            foreach (WaterQualityReading reading in readings)
            {
                _readings.Add(reading);
            }
        }
    }
}