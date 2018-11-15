using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.InventorySystem
{
    public class Equipment : MonoBehaviour
    {
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
    }
}