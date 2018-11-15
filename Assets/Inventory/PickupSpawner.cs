using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core.Saving;

namespace RPG.InventorySystem
{
    public class PickupSpawner : MonoBehaviour, ISaveable
    {
        [SerializeField] InventoryItem item;

        public Pickup pickup { get => GetComponentInChildren<Pickup>(); }

        public bool wasCollected { get => pickup == null; }

        public void CaptureState(IDictionary<string, object> state)
        {
            state["wasCollected"] = wasCollected;
        }

        public void RestoreState(IReadOnlyDictionary<string, object> state)
        {
            bool wasCollected = false;
            if (state.ContainsKey("wasCollected"))
            {
                wasCollected = (bool)state["wasCollected"];
            }

            DestroyPickup();
            if (!wasCollected)
            {
                Debug.Log("Spawning.");
                SpawnPickup();
            }
        }

        private void SpawnPickup()
        {   
            var spawnedPickup = item.SpawnPickup(transform.position);
            spawnedPickup.transform.parent = transform;
        }

        private void DestroyPickup()
        {
            if (pickup)
            {
                Destroy(pickup.gameObject);
            }
        }
    }
}