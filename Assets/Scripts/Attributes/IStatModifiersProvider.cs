using System.Collections.Generic;

namespace RPG.Attributes
{
    public interface IStatModifiersProvider
    {
        IEnumerable<StatModifier> modifiers
        {
            get;
        }
    }
}