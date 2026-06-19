using WaterQuality.InformationSystem.Models;
using WaterQuality.InformationSystem.Repositories;

namespace WaterQuality.InformationSystem.Commands
{
    public class AddWaterQualityReadingCommand : IUndoableCommand
    {
        private readonly IWaterQualityReadingRepository _readingRepository;
        private readonly WaterQualityReading _reading;

        public AddWaterQualityReadingCommand(
            IWaterQualityReadingRepository readingRepository,
            WaterQualityReading reading)
        {
            _readingRepository = readingRepository;
            _reading = reading;
        }

        public string Description
        {
            get
            {
                return "Add water quality reading";
            }
        }

        public void Execute()
        {
            _readingRepository.Add(_reading);
        }

        public void Unexecute()
        {
            _readingRepository.Delete(_reading.Id);
        }
    }
}