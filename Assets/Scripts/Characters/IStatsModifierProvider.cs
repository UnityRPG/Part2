using System.Collections.Generic;

namespace RPG.Characters
{
    public interface IStatsModifierProvider
    {
        IEnumerable<StatsModifier> modifiers
        {
            get;
        }
    }
}