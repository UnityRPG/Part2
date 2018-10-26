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

        [SerializeField] string _itemID = System.Guid.NewGuid().ToString();
        [SerializeField] StatsModifier[] _modifiers;
        [SerializeField] float _baseCost;
        [SerializeField] Rarity _rarity;
        [SerializeField] int _level;
        [SerializeField] string _displayName;
        [TextArea]
        [SerializeField] string _description;
        [SerializeField] Texture2D _icon;

        public string itemID { get; }

    }
}
