using UnityEngine;
using RPG.Characters;

namespace RPG.Inventory
{
    public abstract class InventoryItem : ScriptableObject
    {
        public enum Rarity
        {
            Household,
            Common,
            Uncommon,
            Rare,
            Legendary,
            Mythical
        }

        [SerializeField] string itemID = System.Guid.NewGuid().ToString();
        [SerializeField] StatsModifier[] modifiers;
        [SerializeField] float baseCost;
        [SerializeField] Rarity rarity;
        [SerializeField] int level;
        [SerializeField] string displayName;
        [TextArea]
        [SerializeField] string description;
        [SerializeField] Texture2D icon;

    }
}
