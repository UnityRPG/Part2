using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.CameraUI.Dragging;
using RPG.InventorySystem;

namespace RPG.CameraUI.Equipment
{
    public class EquipmentSlotUI : MonoBehaviour, IDragContainer
    {
        [SerializeField] Image _iconImage;
        [SerializeField] EquipableItem.EquipLocation equipLocation;

        public int index { get; set; }

        Inventory _inventory;
        InventoryItem _item;

        public Inventory inventory { set { _inventory = value; } }

        public InventoryItem item
        {
            get => _item;
            set => SetItem(value);
        }

        private void Start() {
            item = null;
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
            var oldItem = this.item;
            this.item = item;
            return oldItem;
        }

        public bool CanAcceptItem(InventoryItem item)
        {
            if (!item is EquipableItem)
            {
                return false;
            }
            var equipableItem = (EquipableItem)item;
            return equipableItem.allowedEquipLocation == equipLocation;
        }
    }
}