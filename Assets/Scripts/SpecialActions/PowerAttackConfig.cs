﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SpecialActions
{
    [CreateAssetMenu(menuName = ("RPG/Special Abiltiy/Power Attack"))]
    public class PowerAttackConfig : ActionConfig
    {
        [Header("Power Attack Specific")]
        [SerializeField] float extraDamage = 10f;
        [SerializeField] float _damageDelay = 0.2f;

        public override ActionBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<PowerAttackBehaviour>();
        }
        public float GetExtraDamage()
        {
            return extraDamage;
        }

        public float damageDelay => _damageDelay;
    }
}