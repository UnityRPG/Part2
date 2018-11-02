using UnityEngine;

namespace RPG.InventorySystem
{
    [CreateAssetMenu(menuName = ("RPG/Inventory/Index"))]
    public class InventoryItemList : ScriptableObject
    {
        [SerializeField] InventoryItem[] allInventoryItems;

        public InventoryItem GetFromID(string itemID)
        {
            foreach (var item in allInventoryItems)
            {
                Debug.Log("Item id: " + item.itemID);
                if (item.itemID == itemID)
                {
                    return item;
                }
            }

            return null;
        }
    }
}