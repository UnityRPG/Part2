using System.Collections;
using System.Collections.Generic;
using RPG.Core.UI.Dragging;
using RPG.Inventories;
using RPG.SpecialActions;
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
            return item is ActionConfig;
        }

        InventoryItem IDragContainer<InventoryItem>.ReplaceItem(InventoryItem item)
        {
            var oldItem = this._item;
            this._item = item;
            _icon.SetItem(item);
            return oldItem;
        }
    }
}
