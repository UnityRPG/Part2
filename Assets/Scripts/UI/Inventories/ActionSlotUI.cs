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
        [SerializeField] int index = 0;

        SpecialAbilities _abilitiesStore;

        InventoryItem IItemHolder.item { get => _abilitiesStore.GetAbility(index); }

        private void Awake() {
            _abilitiesStore = GameObject.FindGameObjectWithTag("Player").GetComponent<SpecialAbilities>();
        }

        bool IDragContainer<InventoryItem>.CanAcceptItem(InventoryItem item)
        {
            return item is ActionConfig;
        }

        InventoryItem IDragContainer<InventoryItem>.ReplaceItem(InventoryItem item)
        {
            _icon.SetItem(item);
            return _abilitiesStore.ReplaceAbility(item as ActionConfig, index);
        }
    }
}
