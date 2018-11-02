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
            Debug.Log("Drawing.");
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            foreach (var slot in playerInventory.slots)
            {
                var itemUI = Instantiate(InventoryItemPrefab, transform);
                itemUI.SetItem(slot.item);
            }
        }
    }
}