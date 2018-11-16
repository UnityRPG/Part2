using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core.Saving;
using RPG.Characters;

namespace RPG.InventorySystem
{
    public class Equipment : MonoBehaviour, ISaveable, IStatsModifierProvider
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

        public IEnumerable<StatsModifier> modifiers 
        {
            get
            {
                foreach (var pair in equippedItems)
                {
                    if (pair.Value == null) continue;
                    
                    foreach (var modifier in pair.Value.modifiers)
                    {
                        yield return modifier;
                    }
                }
            }
        }

        public void CaptureState(IDictionary<string, object> state)
        {
            var equippedItemsForSerialization = new Dictionary<EquipableItem.EquipLocation, string>();
            foreach (var pair in equippedItems)
            {
                print(pair);
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
                var item =(EquipableItem)InventoryItem.GetFromID(pair.Value);
                if (item != null)
                {
                    equippedItems[pair.Key] = item;
                }
            }

        }
    }
}