using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WaterQuality.InformationSystem.Models;

namespace WaterQuality.InformationSystem.Repositories
{
    public class WaterSourceRepository : IWaterSourceRepository
    {
        private readonly ObservableCollection<WaterSource> _sources;

        public WaterSourceRepository()
        {
            _sources = new ObservableCollection<WaterSource>();
        }

        public ObservableCollection<WaterSource> GetAll()
        {
            return _sources;
        }

        public WaterSource GetById(Guid id)
        {
            return _sources.FirstOrDefault(source => source.Id == id);
        }

        public void Add(WaterSource source)
        {
            _sources.Add(source);
        }

        public void Update(WaterSource source)
        {
            WaterSource existingSource = GetById(source.Id);

            if (existingSource == null)
            {
                return;
            }

            existingSource.Name = source.Name;
            existingSource.Location = source.Location;
            existingSource.SourceType = source.SourceType;
            existingSource.Municipality = source.Municipality;
            existingSource.CapacityM3 = source.CapacityM3;
        }

        public void Delete(Guid id)
        {
            WaterSource source = GetById(id);

            if (source == null)
            {
                return;
            }

            _sources.Remove(source);
        }

        public void ReplaceAll(IEnumerable<WaterSource> sources)
        {
            _sources.Clear();

            foreach (WaterSource source in sources)
            {
                _sources.Add(source);
            }
        }
    }
}