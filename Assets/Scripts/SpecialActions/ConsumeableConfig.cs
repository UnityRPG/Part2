using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Inventories;
using System;

namespace RPG.SpecialActions
{
    public abstract class ConsumableConfig : ActionConfig
    {
        private Inventory playerInventory => GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        public override void Use(GameObject target) 
        {
            playerInventory.ConsumeItem(this);
            
            base.Use(target);
        }

        public override bool CanUseWhenInRange(GameObject target)
        {
            return base.CanUseWhenInRange(target) && HasAny();
        }

        public bool HasAny() => playerInventory.HasItem(this);
    }
}