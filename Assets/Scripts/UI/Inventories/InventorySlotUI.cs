using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RPG.InventorySystem;
using RPG.UI.Dragging;

namespace RPG.UI.InventorySystem
{
    public class InventorySlotUI : MonoBehaviour, IItemHolder, IDragContainer
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

        public InventoryItem ReplaceItem(InventoryItem item)
        {
            return _inventory.ReplaceItemInSlot(item, index);
        }

        public bool CanAcceptItem(InventoryItem item)
        {
            return true;
        }
    }
}