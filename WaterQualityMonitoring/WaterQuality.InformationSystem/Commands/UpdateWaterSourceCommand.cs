using WaterQuality.InformationSystem.Models;
using WaterQuality.InformationSystem.Repositories;

namespace WaterQuality.InformationSystem.Commands
{
    public class UpdateWaterSourceCommand : IUndoableCommand
    {
        private readonly IWaterSourceRepository _sourceRepository;
        private readonly WaterSource _oldSource;
        private readonly WaterSource _newSource;

        public UpdateWaterSourceCommand(
            IWaterSourceRepository sourceRepository,
            WaterSource oldSource,
            WaterSource newSource)
        {
            _sourceRepository = sourceRepository;
            _oldSource = oldSource;
            _newSource = newSource;
        }

        public string Description
        {
            get
            {
                return "Update water source";
            }
        }

        public void Execute()
        {
            _sourceRepository.Update(_newSource);
        }

        public void Unexecute()
        {
            _sourceRepository.Update(_oldSource);
        }
    }
}