using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.InventorySystem;

namespace RPG.CameraUI
{
    public class InventoryUI : MonoBehaviour
    {
        Inventory _playerInventory;

        [SerializeField] InventorySlotUI InventoryItemPrefab;

        // Start is called before the first frame update
        private void Start()
        {
            var player = GameObject.FindWithTag("Player");
            _playerInventory = player.GetComponent<Inventory>();
            _playerInventory.inventoryUpdated += Redraw;
            Redraw();
        }

        private void Redraw()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < _playerInventory.slots.Length; i++)
            {
                var itemUI = Instantiate(InventoryItemPrefab, transform);
                itemUI.inventory = _playerInventory;
                itemUI.index = i;
                itemUI.SetItem(_playerInventory.slots[i].item);
            }
        }
    }
}