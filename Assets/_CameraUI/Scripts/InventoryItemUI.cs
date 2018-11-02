using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RPG.InventorySystem;

namespace RPG.CameraUI
{
    public class InventoryItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        Vector3 _startPosition;
        Transform _originalParent;
        
        Canvas _parentCanvas;
        InventorySlotUI _parentSlot;

        private void Awake() {
            _parentCanvas = GetComponentInParent<Canvas>();
            _parentSlot = GetComponentInParent<InventorySlotUI>();
        }

        public InventorySlotUI parentSlot { get { return _parentSlot; } }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _startPosition = transform.position;
            _originalParent = transform.parent;
            transform.parent = _parentCanvas.transform;
            // Else won't get the drop event.
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.position = _startPosition;
            transform.parent = _originalParent;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

    }
}