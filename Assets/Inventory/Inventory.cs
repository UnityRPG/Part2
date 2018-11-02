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
        private InventorySlot[] inventorySlots;

        [SerializeField] bool hasDeliveryItem = true; // TODO go from mock to real
        [FormerlySerializedAs("modifiers")]
        [SerializeField] StatsModifier[] _modifiers; 
        [SerializeField] InventoryItemList inventoryItemList;
        [SerializeField] int inventorySize;

        public InventorySlot[] slots
        {
            get { 
                print(inventorySlots[0].item);
                return inventorySlots;
            }
        }

        public struct InventorySlot
        {
            public InventoryItem item;
        }

        private void Awake() {
            inventorySlots = new InventorySlot[inventorySize];
            inventorySlots[0].item = inventoryItemList.GetFromID("ba374279-da85-4530-8052-4c10a8ce03b5");
            inventorySlots[3].item = inventoryItemList.GetFromID("bedb2849-78fb-4167-af74-96612a5b5229");
            print(inventoryItemList.GetFromID("ba374279-da85-4530-8052-4c10a8ce03b5"));
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
            // var slotStrings = (string[]) state["inventorySlots"];
            // for (int i = 0; i < inventorySize; i++)
            // {
            //     inventorySlots[i].item = inventoryItemList.GetFromID(slotStrings[i]);
            // }
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