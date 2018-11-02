using UnityEngine;
using RPG.Characters;

namespace RPG.InventorySystem
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
        [SerializeField] Sprite _icon;
        [SerializeField] Pickup pickup;

        public string itemID { get { return _itemID; } }
        public Sprite icon { get { return _icon; } } 

    }
}
