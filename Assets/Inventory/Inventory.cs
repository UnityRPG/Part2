using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using RPG.Core.Saving;
using RPG.Characters;

namespace RPG.InventorySystem
{
    public class Inventory : MonoBehaviour, ISaveable, IStatsModifierProvider
    {

        int coin;
        private InventorySlot[] inventorySlots;

        [SerializeField] bool hasDeliveryItem = true; // TODO go from mock to real
        [FormerlySerializedAs("modifiers")]
        [SerializeField] StatsModifier[] _modifiers; 
        [SerializeField] InventoryItemList inventoryItemList;
        [SerializeField] int inventorySize;
        
        
        public struct InventorySlot
        {
            public InventoryItem item;
        }

        private void Awake() {
            inventorySlots = new InventorySlot[inventorySize];
            inventorySlots[0].item = inventoryItemList.GetFromID("ba374279-da85-4530-8052-4c10a8ce03b5");
            inventorySlots[3].item = inventoryItemList.GetFromID("bedb2849-78fb-4167-af74-96612a5b5229");
        }

        public event Action inventoryUpdated = delegate {};

        public InventorySlot[] slots
        {
            get { 
                return inventorySlots;
            }
        }

        public bool AddToFirstEmptySlot(InventoryItem item)
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i].item == null)
                {
                    inventorySlots[i].item = item;
                    inventoryUpdated();
                    return true;
                }               
            }
            return false;
        }

        public InventoryItem ReplaceItemInSlot(InventoryItem item, int slot)
        {
            var oldItem = inventorySlots[slot].item;
            inventorySlots[slot].item = item;
            inventoryUpdated();
            return oldItem;
        }

        public InventoryItem PopItemFromSlot(int slot)
        {
            var item = inventorySlots[slot].item;
            inventorySlots[slot].item = null;
            inventoryUpdated();
            return item;
        }

        public void CaptureState(IDictionary<string, object> state)
        {
            var slotStrings = new string[inventorySize];
            for (int i = 0; i < inventorySize; i++)
            {
                if (inventorySlots[i].item != null)
                {
                    slotStrings[i] = inventorySlots[i].item.itemID;
                }
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
            inventoryUpdated();
        }

        public void AddCoin(int amount)
        {
            coin += amount;
        }

        public int GetCoinAmount()
        {
            return coin;
        }

        public bool IsPlayerCarrying()  // TODO pass paramater
        {
            return hasDeliveryItem;
        }

        public IEnumerable<StatsModifier> modifiers
        {
            get
            {
                return _modifiers;
            }
        }

    }
}