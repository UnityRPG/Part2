using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RPG.InventorySystem;

namespace RPG.CameraUI
{
    public class InventorySlotUI : MonoBehaviour, IDropHandler
    {
        [SerializeField] Image _iconImage;

        public int index { get; set; }

        Inventory _inventory;

        public Inventory inventory { set { _inventory = value; } }

        public void SetItem(InventoryItem item)
        {
            if (item == null)
            {
                _iconImage.enabled = false;
            }
            else 
            {
                _iconImage.enabled = true;
                _iconImage.sprite = item.icon;
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            var item = eventData.pointerDrag.GetComponent<InventoryItemUI>();
            var sendingItem = _inventory.PopItemFromSlot(item.parentSlot.index);
            var swappedItem = _inventory.ReplaceItemInSlot(sendingItem, index);
            _inventory.ReplaceItemInSlot(swappedItem, item.parentSlot.index);
        }

        public void DiscardItem()
        {
            _inventory.DropItem(index);
        }
    }
}