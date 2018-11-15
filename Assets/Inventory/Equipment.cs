using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core.Saving;

namespace RPG.InventorySystem
{
    public class Equipment : MonoBehaviour, ISaveable
    {
        [SerializeField] InventoryItemList index;

        Dictionary<EquipableItem.EquipLocation, EquipableItem> equippedItems = new Dictionary<EquipableItem.EquipLocation, EquipableItem>();

        public event Action equipmentUpdated;

        public EquipableItem GetItemInSlot(EquipableItem.EquipLocation slot)
        {
            if (!equippedItems.ContainsKey(slot))
            {
                return null;
            }

            return equippedItems[slot];
        }

        public EquipableItem ReplaceItemInSlot(EquipableItem.EquipLocation slot, EquipableItem item)
        {
            EquipableItem replacedItem = null;
            if (equippedItems.ContainsKey(slot))
            {
                replacedItem = equippedItems[slot];
            }

            equippedItems[slot] = item;

            equipmentUpdated();

            return replacedItem;
        }

        public void CaptureState(IDictionary<string, object> state)
        {
            var equippedItemsForSerialization = new Dictionary<EquipableItem.EquipLocation, string>();
            foreach (var pair in equippedItems)
            {
                equippedItemsForSerialization[pair.Key] = pair.Value.itemID;
            }
            state["equippedItems"] = equippedItemsForSerialization;
        }


        public void RestoreState(IReadOnlyDictionary<string, object> state)
        {
            equippedItems = new Dictionary<EquipableItem.EquipLocation, EquipableItem>();

            if (!state.ContainsKey("equippedItems")) return;

            var equippedItemsForSerialization = (Dictionary<EquipableItem.EquipLocation, string>)state["equippedItems"];

            foreach (var pair in equippedItemsForSerialization)
            {
                equippedItems[pair.Key] = (EquipableItem)index.GetFromID(pair.Value);
            }

        }
    }
}