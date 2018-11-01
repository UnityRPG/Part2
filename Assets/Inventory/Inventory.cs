using System.Collections;
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
        InventoryItem[] _contents;

        [SerializeField] bool hasDeliveryItem = true; // TODO go from mock to real
        [FormerlySerializedAs("modifiers")]
        [SerializeField] StatsModifier[] _modifiers; 
        [SerializeField] InventoryItemList inventoryItemList;
        [SerializeField] int inventorySize;

        private void Start() {
            _contents = new InventoryItem[]{inventoryItemList.GetFromID("1b90eb28-2371-4cd6-b631-4a3232215d08"), inventoryItemList.GetFromID("1b90eb28-2371-4cd6-b631-4a3232215d08")};
        }

        public InventoryItem[] contents
        {
            get { return _contents; }
        }

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