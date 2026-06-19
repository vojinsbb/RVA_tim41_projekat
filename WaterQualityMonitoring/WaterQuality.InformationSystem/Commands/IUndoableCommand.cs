namespace WaterQuality.InformationSystem.Commands
{
    public interface IUndoableCommand
    {
        void Execute();

        void Unexecute();

        string Description { get; }
    }
}