using WaterQuality.InformationSystem.Models;
using WaterQuality.InformationSystem.Repositories;

namespace WaterQuality.InformationSystem.Commands
{
    public class UpdateWaterQualityReadingCommand : IUndoableCommand
    {
        private readonly IWaterQualityReadingRepository _readingRepository;
        private readonly WaterQualityReading _oldReading;
        private readonly WaterQualityReading _newReading;

        public UpdateWaterQualityReadingCommand(
            IWaterQualityReadingRepository readingRepository,
            WaterQualityReading oldReading,
            WaterQualityReading newReading)
        {
            _readingRepository = readingRepository;
            _oldReading = oldReading;
            _newReading = newReading;
        }

        public string Description
        {
            get
            {
                return "Update water quality reading";
            }
        }

        public void Execute()
        {
            _readingRepository.Update(_newReading);
        }

        public void Unexecute()
        {
            _readingRepository.Update(_oldReading);
        }
    }
}