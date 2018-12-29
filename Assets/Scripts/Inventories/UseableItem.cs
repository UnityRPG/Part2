using UnityEngine;

namespace RPG.Inventories
{
    public abstract class UseableItem : InventoryItem
    {
        public abstract void Use(GameObject user);
    }
}