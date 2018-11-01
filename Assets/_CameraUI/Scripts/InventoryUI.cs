using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.InventorySystem;

namespace RPG.CameraUI
{
    public class InventoryUI : MonoBehaviour
    {
        Inventory playerInventory;

        // Start is called before the first frame update
        void Start()
        {
            var player = GameObject.FindWithTag("Player");
            playerInventory = player.GetComponent<Inventory>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}