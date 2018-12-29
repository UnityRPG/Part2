using System.Collections.Generic;

namespace RPG.Attributes
{
    public interface IAttributeModifierProvider
    {
        IEnumerable<AttributeModifier> modifiers
        {
            get;
        }
    }
}