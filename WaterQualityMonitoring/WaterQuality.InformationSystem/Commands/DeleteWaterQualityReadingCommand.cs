using WaterQuality.InformationSystem.Models;
using WaterQuality.InformationSystem.Repositories;

namespace WaterQuality.InformationSystem.Commands
{
    public class DeleteWaterQualityReadingCommand : IUndoableCommand
    {
        private readonly IWaterQualityReadingRepository _readingRepository;
        private readonly WaterQualityReading _reading;

        public DeleteWaterQualityReadingCommand(
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
                return "Delete water quality reading";
            }
        }

        public void Execute()
        {
            _readingRepository.Delete(_reading.Id);
        }

        public void Unexecute()
        {
            _readingRepository.Add(_reading);
        }
    }
}