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
        [SerializeField] GameObject _pickup;

        public string itemID { get { return _itemID; } }
        public Sprite icon { get { return _icon; } }
        public string displayName { get { return _displayName; } }
        public string description { get { return _description; } }

        public Pickup SpawnPickup(Vector3 position)
        {
            var pickupGameObject = Instantiate(_pickup);
            var pickup = pickupGameObject.AddComponent<Pickup>();
            pickup.transform.position = position;
            pickup.item = this;
            return pickup;
        }

    }
}
