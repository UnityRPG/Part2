using System;
using UnityEngine;
using UnityEngine.UI;
using RPG.Saving;
using System.Collections.Generic;

namespace RPG.SpecialActions
{
    public class SpecialAbilities : MonoBehaviour, ISaveable
    {
        [SerializeField] int _numberOfAbilities = 6;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float regenPointsPerSecond = 1f;
        [SerializeField] AudioClip outOfEnergy;

        float currentEnergyPoints;
        ActionConfig[] abilities;

        AudioSource audioSource;        

        public float energyAsPercent { get { return currentEnergyPoints / maxEnergyPoints; } }

        private void Awake() {
            abilities = new ActionConfig[_numberOfAbilities];
        }

        // Use this for initialization
        void Start()
        {
            audioSource = GetComponent<AudioSource>();

            currentEnergyPoints = maxEnergyPoints;
            AttachInitialAbilities();
        }

        void Update()
        {
            if (currentEnergyPoints < maxEnergyPoints)
            {
                AddEnergyPoints();
            }
        }

        void AttachInitialAbilities()
        {
            for (int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
            {
                if (abilities[abilityIndex] == null) continue;

                abilities[abilityIndex].AttachAbilityTo(gameObject);
            }
        }

        public ActionConfig GetAbility(int index)
        {
            return abilities[index];
        }

        public ActionConfig ReplaceAbility(ActionConfig replacement, int index)
        {
            var oldAbility = abilities[index];
            abilities[index] = replacement;
            replacement.AttachAbilityTo(gameObject);
            if (oldAbility != null)
            {
                oldAbility.DetachAbility();
            }
            return oldAbility;
        }

        public bool CanUseWhenInRange(int abilityIndex, GameObject target = null)
        {
            if (abilities[abilityIndex] == null) return false;
            return abilities[abilityIndex].CanUseWhenInRange(target);
        }

        public bool IsInRange(int abilityIndex, GameObject target)
        {
            return abilities[abilityIndex].IsInRange(target);
        }

        public void AttemptSpecialAbility(int abilityIndex, GameObject target = null)
        {
            if (!CanUseWhenInRange(abilityIndex, target)) return;

            var energyCost = abilities[abilityIndex].GetEnergyCost();

            if (energyCost <= currentEnergyPoints)
            {
                ConsumeEnergy(energyCost);
                abilities[abilityIndex].Use(target);
            }
            else
            {
                audioSource.PlayOneShot(outOfEnergy);
            }
        }

        public int GetNumberOfAbilities()
        {
            return abilities.Length;
        }

        private void AddEnergyPoints()
        {
            var pointsToAdd = regenPointsPerSecond * Time.deltaTime;
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + pointsToAdd, 0, maxEnergyPoints);
        }

        public void ConsumeEnergy(float amount)
        {
            float newEnergyPoints = currentEnergyPoints - amount;
            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0, maxEnergyPoints);
        }

        void ISaveable.CaptureState(IDictionary<string, object> state)
        {
            // TODO
        }

        void ISaveable.RestoreState(IReadOnlyDictionary<string, object> state)
        {
        }
    }
}