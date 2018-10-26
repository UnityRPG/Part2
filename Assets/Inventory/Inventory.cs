using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core.Saving;

namespace RPG.Inventory
{
    public class Inventory : MonoBehaviour, ISaveable
    {
        public struct InventorySlot
        {
            public InventoryItem item;
        }


        public void CaptureState(IDictionary<string, object> state)
        {
            throw new System.NotImplementedException();
        }

        public void RestoreState(IReadOnlyDictionary<string, object> state)
        {
            throw new System.NotImplementedException();
        }


    }
}