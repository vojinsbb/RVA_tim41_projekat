using WaterQuality.InformationSystem.Data;

namespace WaterQuality.InformationSystem.Services
{
    public interface IDataPersistenceService
    {
        bool DataFileExists();

        WaterQualityDataStore LoadData();

        void SaveData(WaterQualityDataStore dataStore);
    }
}