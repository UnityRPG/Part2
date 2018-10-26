using UnityEngine;
using RPG.Characters;

namespace RPG.Inventory
{
    public class InventoryItem : ScriptableObject
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

        [SerializeField] StatsModifier[] modifiers;
        [SerializeField] float baseCost;
        [SerializeField] Rarity rarity;
        [SerializeField] int level;
        [SerializeField] string description;
        [SerializeField] Texture2D icon;

    }
}
