using UnityEngine;

namespace RPG.Inventory
{
    [CreateAssetMenu(menuName = ("RPG/Inventory/Equipable Item"))]
    public class EquipableItem : InventoryItem
    {
        public enum EquipLocation
        {
            Helmet,
            Necklace,
            Body,
            Trousers,
            Boots,
            Weapon,
            Shield
        }
        [SerializeField] EquipLocation _allowedEquipLocation;

        public EquipLocation allowedEquipLocation
        {
            get
            {
                return _allowedEquipLocation;
            }
        }

    }
}