using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WaterQuality.InformationSystem.Models;

namespace WaterQuality.InformationSystem.Repositories
{
    public interface IWaterSourceRepository
    {
        ObservableCollection<WaterSource> GetAll();

        WaterSource GetById(Guid id);

        void Add(WaterSource source);

        void Update(WaterSource source);

        void Delete(Guid id);

        void ReplaceAll(IEnumerable<WaterSource> sources);
    }
}