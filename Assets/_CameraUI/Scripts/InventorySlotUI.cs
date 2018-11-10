using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RPG.InventorySystem;

namespace RPG.CameraUI
{
    public class InventorySlotUI : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] Image _iconImage;
        [SerializeField] ItemTooltip tooltipPrefab;

        public int index { get; set; }

        Inventory _inventory;
        ItemTooltip _tooltip;

        public Inventory inventory { set { _inventory = value; } }

        private void OnDestroy() {
            ClearTooltip();
        }

        private void OnDisable() {
            ClearTooltip();
        }

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
            if (item.parentSlot.index == index) return;

            var sendingItem = _inventory.PopItemFromSlot(item.parentSlot.index);
            var swappedItem = _inventory.ReplaceItemInSlot(sendingItem, index);
            _inventory.ReplaceItemInSlot(swappedItem, item.parentSlot.index);
        }

        public void DiscardItem()
        {
            _inventory.DropItem(index);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            var item = _inventory.GetItemInSlot(index);
            if (!item) return;

            var parentCanvas = GetComponentInParent<Canvas>();

            if (!_tooltip)
            {
                _tooltip = Instantiate(tooltipPrefab, parentCanvas.transform);
            }

            _tooltip.title = item.displayName;
            _tooltip.body = item.description;

            PositionTooltip();
        }

        private void PositionTooltip()
        {
            // Required to ensure corners are updated by positioning elements.
            Canvas.ForceUpdateCanvases();

            var tooltipCorners = new Vector3[4];
            _tooltip.GetComponent<RectTransform>().GetWorldCorners(tooltipCorners);
            var slotCorners = new Vector3[4];
            GetComponent<RectTransform>().GetWorldCorners(slotCorners);

            bool below = transform.position.y > Screen.height / 2;
            bool right = transform.position.x < Screen.width / 2;

            int slotCorner = GetCornerIndex(below, right);
            int tooltipCorner = GetCornerIndex(!below, !right);

            _tooltip.transform.position = slotCorners[slotCorner] - tooltipCorners[tooltipCorner] + _tooltip.transform.position;
        }

        private int GetCornerIndex(bool below, bool right)
        {
            if (below && !right) return 0;
            else if (!below && !right) return 1;
            else if (!below && right) return 2;
            else return 3;

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ClearTooltip();
        }

        private void ClearTooltip()
        {
            if (_tooltip)
            {
                Destroy(_tooltip.gameObject);
            }
        }
    }
}