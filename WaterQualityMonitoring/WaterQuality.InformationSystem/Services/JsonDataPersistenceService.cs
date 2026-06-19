using System;
using System.IO;
using Newtonsoft.Json;
using WaterQuality.InformationSystem.Data;

namespace WaterQuality.InformationSystem.Services
{
    public class JsonDataPersistenceService : IDataPersistenceService
    {
        private readonly string _dataFilePath;

        public JsonDataPersistenceService()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string dataDirectory = Path.Combine(baseDirectory, "Data");

            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }

            _dataFilePath = Path.Combine(dataDirectory, "water-quality-data.json");
        }

        public bool DataFileExists()
        {
            return File.Exists(_dataFilePath) && new FileInfo(_dataFilePath).Length > 0;
        }

        public WaterQualityDataStore LoadData()
        {
            if (!DataFileExists())
            {
                return new WaterQualityDataStore();
            }

            string json = File.ReadAllText(_dataFilePath);

            WaterQualityDataStore dataStore = JsonConvert.DeserializeObject<WaterQualityDataStore>(json);

            if (dataStore == null)
            {
                return new WaterQualityDataStore();
            }

            return dataStore;
        }

        public void SaveData(WaterQualityDataStore dataStore)
        {
            string json = JsonConvert.SerializeObject(dataStore, Formatting.Indented);

            File.WriteAllText(_dataFilePath, json);
        }
    }
}