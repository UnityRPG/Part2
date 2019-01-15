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

        public override void Use(GameObject source, GameObject target) 
        {
            playerInventory.ConsumeItem(this);
        }

        public override bool CanUseWhenInRange(GameObject source, GameObject target)
        {
            return base.CanUseWhenInRange(source, target) && HasAny();
        }

        public bool HasAny() => playerInventory.HasItem(this);
    }
}