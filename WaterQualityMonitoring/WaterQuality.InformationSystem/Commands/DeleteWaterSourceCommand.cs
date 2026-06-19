using WaterQuality.InformationSystem.Models;
using WaterQuality.InformationSystem.Repositories;

namespace WaterQuality.InformationSystem.Commands
{
    public class DeleteWaterSourceCommand : IUndoableCommand
    {
        private readonly IWaterSourceRepository _sourceRepository;
        private readonly WaterSource _source;

        public DeleteWaterSourceCommand(
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
                return "Delete water source";
            }
        }

        public void Execute()
        {
            _sourceRepository.Delete(_source.Id);
        }

        public void Unexecute()
        {
            _sourceRepository.Add(_source);
        }
    }
}