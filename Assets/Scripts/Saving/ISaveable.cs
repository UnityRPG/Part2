using System.Collections.Generic;

namespace RPG.Saving
{
    public interface ISaveable
    {
        void CaptureState(IDictionary<string, object> state);
        void RestoreState(IReadOnlyDictionary<string, object> state);
    }
}
