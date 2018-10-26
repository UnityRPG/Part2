using UnityEngine;

namespace RPG.Inventory
{
    [CreateAssetMenu(menuName = ("RPG/Inventory/Index"))]
    public class InventoryItemList : ScriptableObject
    {
        [SerializeField] InventoryItem[] allInventoryItems;
    }
}