using System.Collections.Generic;

namespace RPG.Core.Saving
{
    public interface ISaveable
    {
        void CaptureState(IDictionary<string, object> state);
        void RestoreState(IReadOnlyDictionary<string, object> state);
    }
}
