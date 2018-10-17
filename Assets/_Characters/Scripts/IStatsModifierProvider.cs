using System.Collections.Generic;

namespace RPG.Characters
{
    interface IStatsModifierProvider
    {
        IEnumerable<StatsModifier> GetModifiersForAttribute(StatsModifier.Attribute attribute);
    }
}