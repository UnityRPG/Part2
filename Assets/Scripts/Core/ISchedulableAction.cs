using System;

namespace RPG.Core
{
    public interface ISchedulableAction
    {
        void Start();
        void RequestCancel();
    }

    public interface ISchedulableAction<T>
    {
        void Start(T data);
        void RequestCancel(T data);
    }
}
