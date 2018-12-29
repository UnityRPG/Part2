using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.InventorySystem;

namespace RPG.UI.InventorySystem
{
    public interface IItemHolder
    {
        InventoryItem item { get; set; }
    }
}