﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RPG.Inventories;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class WeaponConfig : EquipableItem
    {
        public Transform gripTransform;

        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackAnimation;
        [SerializeField] float timeBetweenAnimationCycles = .5f;
        [SerializeField] float maxAttackRange = 2f;
        [SerializeField] DamageRange damageRange = new DamageRange(-1, -1);
        [SerializeField] float damageDelay = .5f;
        [SerializeField] float hitsPerSecond = 0.1f;

        #region Deprecation and Migration
        [HideInInspector]
        [SerializeField]
        float additionalDamage = 10f;

        void OnValidate()
        {
            if (damageRange.min == -1 && damageRange.max == -1)
            {
                damageRange = DamageRange.FromPercentage(additionalDamage, 0.1f);
            }
        }
        #endregion

        public float GetTimeBetweenAnimationCycles()
        {
            return timeBetweenAnimationCycles;
        }

        public float GetMaxAttackRange()
        {
            return maxAttackRange;
        }

        public float GetDamageDelay()
        {
            return damageDelay;
        }

        public GameObject GetWeaponPrefab()
        {
            return weaponPrefab;
        }
        
        public AnimationClip GetAttackAnimClip()
        {
            RemoveAnimationEvents();
            return attackAnimation;
        }

        [System.Obsolete("Use the range function instead.")]
        public float GetAdditionalDamage()
        {
            return damageRange.RandomlyChooseDamage();
        }

        public DamageRange GetDamageRange()
        {
            return damageRange;
        }

        public float GetHitsPerSecond()
        {
            return hitsPerSecond;
        }

        // So that asset packs cannot cause crashes
        private void RemoveAnimationEvents()
        {
            attackAnimation.events = new AnimationEvent[0];
        }
    }
}