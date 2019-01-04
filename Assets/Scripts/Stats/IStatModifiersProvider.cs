using System.Collections.Generic;

namespace RPG.Stats
{
    public interface IStatModifiersProvider
    {
        IEnumerable<StatModifier> modifiers
        {
            get;
        }
    }
}