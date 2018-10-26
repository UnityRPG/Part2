using UnityEngine;

namespace RPG.Inventory
{
    public abstract class UseableItem : InventoryItem
    {
        public abstract void Use(GameObject user);
    }
}