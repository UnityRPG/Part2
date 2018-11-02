using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.InventorySystem;

namespace RPG.CameraUI
{
    public class InventoryUI : MonoBehaviour
    {
        Inventory playerInventory;

        [SerializeField] InventorySlotUI InventoryItemPrefab;

        // Start is called before the first frame update
        IEnumerator Start()
        {
            var player = GameObject.FindWithTag("Player");
            playerInventory = player.GetComponent<Inventory>();
            while (isActiveAndEnabled)
            {
                Redraw();
                yield return new WaitForSeconds(10);
            }
        }

        private void Redraw()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < playerInventory.slots.Length; i++)
            {
                var itemUI = Instantiate(InventoryItemPrefab, transform);
                itemUI.inventory = playerInventory;
                itemUI.index = i;
                itemUI.SetItem(playerInventory.slots[i].item);
            }
        }
    }
}