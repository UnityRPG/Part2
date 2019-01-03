using System.Collections.Generic;

namespace RPG.Attributes
{
    public interface IPerformanceModifierProvider
    {
        IEnumerable<PerformanceModifier> modifiers
        {
            get;
        }
    }
}