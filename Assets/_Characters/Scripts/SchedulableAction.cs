using System;

namespace RPG.Characters
{
    public class SchedulableAction
    {
        public event Action OnStart;
        public event Action OnCancel;
        public event Action OnFinish;
        public bool isCancelled => _isCancelled;
        public bool isInterruptable => _isInterruptable;

        private bool _isCancelled = false;
        private bool _isInterruptable = false;

        public SchedulableAction(bool isInterruptable)
        {
            _isInterruptable = isInterruptable;
        }

        public void Start() => OnStart();
        public void Cancel()
        {
            _isCancelled = true;
            OnCancel();
        }
        public void Finish()
        {
            OnFinish();
        }
    }
}

