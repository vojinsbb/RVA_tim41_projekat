using WaterQuality.InformationSystem.Models;
using WaterQuality.InformationSystem.Repositories;

namespace WaterQuality.InformationSystem.Commands
{
    public class AddWaterSourceCommand : IUndoableCommand
    {
        private readonly IWaterSourceRepository _sourceRepository;
        private readonly WaterSource _source;

        public AddWaterSourceCommand(
            IWaterSourceRepository sourceRepository,
            WaterSource source)
        {
            _sourceRepository = sourceRepository;
            _source = source;
        }

        public string Description
        {
            get
            {
                return "Add water source";
            }
        }

        public void Execute()
        {
            _sourceRepository.Add(_source);
        }

        public void Unexecute()
        {
            _sourceRepository.Delete(_source.Id);
        }
    }
}