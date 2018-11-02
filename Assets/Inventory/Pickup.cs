using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.InventorySystem
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField] InventoryItem _item;

        public void PickupItem()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            var inventory = player.GetComponent<Inventory>();
            bool foundSlot = inventory.AddToFirstEmptySlot(_item);
            if (foundSlot)
            {
                Destroy(gameObject);
            }
        }

        public InventoryItem item { set { _item = value; } }
    }
}