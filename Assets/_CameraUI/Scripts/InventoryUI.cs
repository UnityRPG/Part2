using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.InventorySystem;

namespace RPG.CameraUI
{
    public class InventoryUI : MonoBehaviour
    {
        Inventory playerInventory;

        [SerializeField] InventoryItemSlotUI InventoryItemPrefab;

        // Start is called before the first frame update
        IEnumerator Start()
        {
            var player = GameObject.FindWithTag("Player");
            playerInventory = player.GetComponent<Inventory>();
            while (isActiveAndEnabled)
            {
                yield return new WaitForSeconds(2);
                Redraw();
            }
        }

        private void Redraw()
        {
            Debug.Log("Drawing.");
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            foreach (var item in playerInventory.contents)
            {
                var itemUI = Instantiate(InventoryItemPrefab, transform);
            }
        }
    }
}