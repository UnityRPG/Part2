using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.InventorySystem;

namespace RPG.UI.Dragging
{
    public interface IDragContainer
    {
        bool CanAcceptItem(InventoryItem item);
        InventoryItem ReplaceItem(InventoryItem item);
    }
}