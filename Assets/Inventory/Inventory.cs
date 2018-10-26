using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core.Saving;

namespace RPG.Inventory
{
    public class Inventory : MonoBehaviour, ISaveable
    {
        [SerializeField] InventoryItemList inventoryItemList;
        [SerializeField] int inventorySize;

        public struct InventorySlot
        {
            public InventoryItem item;
        }
        
        private InventorySlot[] inventorySlots;

        private void Awake() {
            inventorySlots = new InventorySlot[inventorySize];
        }

        public void CaptureState(IDictionary<string, object> state)
        {
            var slotStrings = new string[inventorySize];
            for (int i = 0; i < inventorySize; i++)
            {
                slotStrings[i] = inventorySlots[i].item.itemID;
            }
            state["inventorySlots"] = slotStrings;
        }

        public void RestoreState(IReadOnlyDictionary<string, object> state)
        {
            var slotStrings = (string[]) state["inventorySlots"];
            for (int i = 0; i < inventorySize; i++)
            {
                inventorySlots[i].item = inventoryItemList.GetFromID(slotStrings[i]);
            }
        }


    }
}