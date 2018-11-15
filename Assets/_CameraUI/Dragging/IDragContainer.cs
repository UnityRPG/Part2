using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.InventorySystem;

namespace RPG.CameraUI.Dragging
{
    public interface IDragContainer
    {
        void DropItem();
        InventoryItem PopItem();
        InventoryItem ReplaceItem(InventoryItem item);
    }
}