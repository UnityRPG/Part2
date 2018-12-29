using UnityEngine;

namespace RPG.InventorySystem
{
    public abstract class UseableItem : InventoryItem
    {
        public abstract void Use(GameObject user);
    }
}