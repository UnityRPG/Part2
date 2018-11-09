using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core.Saving;

namespace RPG.InventorySystem
{
    public class Pickup : MonoBehaviour, ISaveable
    {
        [SerializeField] InventoryItem _item;
        
        bool _wasFromInventory = false;
        bool _wasCollected = false;

        public bool wasFromInventory { set { _wasFromInventory = value; } }

        public void PickupItem()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            var inventory = player.GetComponent<Inventory>();
            bool foundSlot = inventory.AddToFirstEmptySlot(_item);
            if (foundSlot)
            {
                Collect();
            }
        }

        private void Collect(bool collect=true)
        {
            if (_wasFromInventory && collect)
            {
                Destroy(gameObject);
            }
            if (!_wasFromInventory)
            {
                gameObject.SetActive(!collect);
            }
            _wasCollected = collect;
        }

        public void CaptureState(IDictionary<string, object> state)
        {
            state["wasCollected"] = _wasCollected;
        }

        public void RestoreState(IReadOnlyDictionary<string, object> state)
        {
            bool wasCollected = false;
            if (state.ContainsKey("wasCollected"))
            {
                wasCollected = (bool)state["wasCollected"];
            }
            Collect(wasCollected);
        }

        public InventoryItem item { get { return _item; } set { _item = value; } }
    }
}