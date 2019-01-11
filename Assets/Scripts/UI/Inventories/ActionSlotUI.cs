using System.Collections;
using System.Collections.Generic;
using RPG.Core.UI.Dragging;
using RPG.Inventories;
using UnityEngine;

namespace RPG.UI.Inventories
{
    public class ActionSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        [SerializeField] InventoryItemIcon _icon;

        InventoryItem _item;

        InventoryItem IItemHolder.item { get => _item; }

        bool IDragContainer<InventoryItem>.CanAcceptItem(InventoryItem item)
        {
            return true;
        }

        InventoryItem IDragContainer<InventoryItem>.ReplaceItem(InventoryItem item)
        {
            var oldItem = this._item;
            this._item = item;
            _icon.SetItem(item);
            return oldItem;
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
