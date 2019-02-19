using System;
using System.Collections.Generic;

namespace RPG.Core
{
    public class ScheduledAction<T> : ISchedulableAction, IEquatable<ScheduledAction<T>>
    {
        public ISchedulableAction<T> action { get; private set; }
        public T data { get; private set; }

        public ScheduledAction(ISchedulableAction<T> action, T data)
        {
            this.action = action;
            this.data = data;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ScheduledAction<T>);
        }
        
        public bool Equals(ScheduledAction<T> other)
        {
            if (other == null) return false;
            return System.Object.ReferenceEquals(action, other.action) 
                && EqualityComparer<T>.Default.Equals(data, other.data);
        }

        void ISchedulableAction.RequestCancel() => action.RequestCancel(data);

        void ISchedulableAction.Start() => action.Start(data);
    }
}