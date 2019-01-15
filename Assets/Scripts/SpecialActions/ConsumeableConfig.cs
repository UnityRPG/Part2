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
        public override void Use(GameObject source, GameObject target) 
        {
            source.GetComponent<Inventory>().ConsumeItem(this);
        }

        public override bool CanUseWhenInRange(GameObject source, GameObject target)
        {
            return base.CanUseWhenInRange(source, target) && CountRemaining(source) > 0;
        }

        public int CountRemaining(GameObject source)
        {
            return source.GetComponent<Inventory>().HasItem(this) ? 1 : 0;
        }
    }
}