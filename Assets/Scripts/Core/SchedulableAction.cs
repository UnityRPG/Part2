using System;

namespace RPG.Core
{
    public class SchedulableAction : ISchedulableAction
    {
        public event Action OnStart;
        public event Action OnCancel;
        public event Action OnFinish;

        public ActionScheduler scheduler { get; set; }
        public bool isStarted { get; private set; } = false;
        public bool isFinished { get; private set; } = false;
        public bool cancelRequested { get; private set; } = false;

        public void Finish()
        {
            isFinished = true;
            if (scheduler != null)
            {
                scheduler.FinishAction(this);
            }
            if (OnFinish != null)
            {
                OnFinish();
            }
        }

        void ISchedulableAction.Start()
        {
            isStarted = true;
            if (OnStart != null)
            {
                OnStart();
            }
        }

        void ISchedulableAction.RequestCancel()
        {
            cancelRequested = true;
            if (OnCancel != null)
            {
                OnCancel();
            }
        }
    }
}

