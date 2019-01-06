using System.Collections.Generic;

namespace RPG.Stats
{
    public interface IStatModifiersProvider
    {
        IEnumerable<StatModifier> GetModifiers(StatModifier.Filter filter);
    }
}