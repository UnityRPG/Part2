using System;

namespace RPG.Core
{
    public interface ISchedulableAction
    {
        void Start();
        void RequestCancel();
    }
}
