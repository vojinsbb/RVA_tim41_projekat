using System.Collections.Generic;
using WaterQuality.InformationSystem.Commands;

namespace WaterQuality.InformationSystem.Services
{
    public class UndoRedoService
    {
        private readonly Stack<IUndoableCommand> _undoStack;
        private readonly Stack<IUndoableCommand> _redoStack;

        public UndoRedoService()
        {
            _undoStack = new Stack<IUndoableCommand>();
            _redoStack = new Stack<IUndoableCommand>();
        }

        public bool CanUndo
        {
            get
            {
                return _undoStack.Count > 0;
            }
        }

        public bool CanRedo
        {
            get
            {
                return _redoStack.Count > 0;
            }
        }

        public void ExecuteCommand(IUndoableCommand command)
        {
            command.Execute();

            _undoStack.Push(command);
            _redoStack.Clear();
        }

        public IUndoableCommand Undo()
        {
            if (!CanUndo)
            {
                return null;
            }

            IUndoableCommand command = _undoStack.Pop();

            command.Unexecute();

            _redoStack.Push(command);

            return command;
        }

        public IUndoableCommand Redo()
        {
            if (!CanRedo)
            {
                return null;
            }

            IUndoableCommand command = _redoStack.Pop();

            command.Execute();

            _undoStack.Push(command);

            return command;
        }

        public void Clear()
        {
            _undoStack.Clear();
            _redoStack.Clear();
        }
    }
}