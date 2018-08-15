using System.Collections.Generic;

namespace RPG.Core.Saving
{
    public interface ISaveable
    {
        IDictionary<string, object> CaptureState();
        void RestoreState(IDictionary<string, object> state);
    }
}
