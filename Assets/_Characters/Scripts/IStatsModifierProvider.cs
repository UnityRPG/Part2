using System.Collections.Generic;

namespace RPG.Characters
{
    interface IStatsModifierProvider
    {
        IEnumerable<StatsModifier> modifiers
        {
            get;
        }
    }
}