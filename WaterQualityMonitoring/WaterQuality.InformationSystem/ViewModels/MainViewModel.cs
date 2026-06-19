using System.Collections.ObjectModel;
using WaterQuality.InformationSystem.Models;
using WaterQuality.InformationSystem.Repositories;
using WaterQuality.InformationSystem.Services;

namespace WaterQuality.InformationSystem.ViewModels
{
    public class MainViewModel
    {
        private readonly IWaterSourceRepository _sourceRepository;
        private readonly IWaterQualityReadingRepository _readingRepository;
        
        public ObservableCollection<WaterSource> WaterSources { get; set; }

        public ObservableCollection<WaterQualityReading> WaterQualityReadings { get; set; }

        public MainViewModel()
        {
            _sourceRepository = new WaterSourceRepository();
            _readingRepository = new WaterQualityReadingRepository();

            SeedDataService seedDataService = new SeedDataService(
                _sourceRepository,
                _readingRepository);

            seedDataService.LoadInitialData();

            WaterSources = _sourceRepository.GetAll();
            WaterQualityReadings = _readingRepository.GetAll();
        }
    }
}