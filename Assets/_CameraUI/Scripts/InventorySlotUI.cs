using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RPG.InventorySystem;

namespace RPG.CameraUI
{
    public class InventorySlotUI : MonoBehaviour, IItemHolder, IDropHandler
    {
        [SerializeField] Image _iconImage;

        public int index { get; set; }

        Inventory _inventory;
        InventoryItem _item;

        public Inventory inventory { set { _inventory = value; } }

        public InventoryItem item { 
            get => _item; 
            set {
                SetItem(value);
            } 
        }

        public void SetItem(InventoryItem item)
        {
            _item = item;

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
            if (item.parentSlot.index == index) return;

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