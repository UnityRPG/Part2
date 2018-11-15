using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using RPG.Core.Saving;
using RPG.Core.Saving;
using RPG.Characters;

namespace RPG.InventorySystem
{
    public class Inventory : MonoBehaviour, ISaveable, IStatsModifierProvider
    {

        int coin;
        private InventorySlot[] inventorySlots;
        private List<Pickup> droppedItems = new List<Pickup>();

        [SerializeField] bool hasDeliveryItem = true; // TODO go from mock to real
        [FormerlySerializedAs("modifiers")]
        [SerializeField] StatsModifier[] _modifiers; 
        [SerializeField] InventoryItemList inventoryItemList;
        [SerializeField] int inventorySize;
        
        
        public static Inventory GetPlayerInventory()
        {
            var player = GameObject.FindWithTag("Player");
            return player.GetComponent<Inventory>();
        }

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

        public bool DropItem(InventoryItem item)
        {
            if (item == null) return false;

            var spawnLocation = transform.position;
            SpawnPickup(item, spawnLocation);

            return true;
        }

        private void SpawnPickup(InventoryItem item, Vector3 spawnLocation)
        {
            var pickup = item.SpawnPickup(spawnLocation);
            droppedItems.Add(pickup);
        }

        public InventoryItem ReplaceItemInSlot(InventoryItem item, int slot)
        {
            var oldItem = inventorySlots[slot].item;
            inventorySlots[slot].item = item;
            inventoryUpdated();
            return oldItem;
        }

        public InventoryItem GetItemInSlot(int slot)
        {
            return inventorySlots[slot].item;
        }

        public void CaptureState(IDictionary<string, object> state)
        {
            CaptureInventoryState(state);
            CaptureDropState(state);
        }

        private void CaptureInventoryState(IDictionary<string, object> state)
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

        private void CaptureDropState(IDictionary<string, object> state)
        {
            RemoveDestroyedDrops();
            var droppedItemsList = new Dictionary<string, object>[droppedItems.Count];
            for (int i = 0; i < droppedItemsList.Length; i++)
            {
                droppedItemsList[i] = new Dictionary<string, object>();
                droppedItemsList[i]["itemID"] = droppedItems[i].item.itemID;
                droppedItemsList[i]["position"] = (SerializableVector3)droppedItems[i].transform.position;
            }
            state["droppedItems"] = droppedItemsList;
        }

        private void RemoveDestroyedDrops()
        {
            var newList = new List<Pickup>();
            foreach (var item in droppedItems)
            {
                if (item != null)
                {
                    newList.Add(item);
                }
            }
            droppedItems = newList;
        }

        public void RestoreState(IReadOnlyDictionary<string, object> state)
        {
            RestoreInventory(state);
            inventoryUpdated();

            DeleteAllDrops();
            if (state.ContainsKey("droppedItems"))
            {
                var droppedItemsList = (Dictionary<string, object>[])state["droppedItems"];
                foreach (var item in droppedItemsList)
                {
                    var pickupItem = inventoryItemList.GetFromID((string)item["itemID"]);
                    Vector3 position = (SerializableVector3)item["position"];
                    SpawnPickup(pickupItem, position);
                }
            }
        }

        private void DeleteAllDrops()
        {
            RemoveDestroyedDrops();
            foreach (var item in droppedItems)
            {
                Destroy(item.gameObject);
            }
        }

        private void RestoreInventory(IReadOnlyDictionary<string, object> state)
        {
            var slotStrings = (string[])state["inventorySlots"];
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