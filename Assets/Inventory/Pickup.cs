using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core.Saving;

namespace RPG.InventorySystem
{
    public class Pickup : MonoBehaviour
    {
        InventoryItem _item;
        
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
            Destroy(gameObject);
        }

        public InventoryItem item { get { return _item; } set { _item = value; } }
    }
}