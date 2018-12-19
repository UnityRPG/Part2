using System;

namespace RPG.Characters
{
    public class SchedulableAction
    {
        public event Action OnStart;
        public event Action OnCancel;
        public event Action OnFinish;
        public bool isRunning => _isRunning;
        public bool isCancelled => _isCancelled;
        public bool isInterruptable => _isInterruptable;

        private bool _isRunning = false;
        private bool _isCancelled = false;
        private bool _isInterruptable = false;

        public SchedulableAction(bool isInterruptable)
        {
            _isInterruptable = isInterruptable;
        }

        public void Start()
        {
            _isRunning = true;
            if (OnStart != null)
            {
                OnStart();
            }
        }
        public void Cancel()
        {
            _isRunning = false;
            _isCancelled = true;
            if (OnCancel != null)
            {
                OnCancel();
            }
        }
        public void Finish()
        {
            _isRunning = false;
            if (OnFinish != null)
            {
                OnFinish();
            }
        }
    }
}

