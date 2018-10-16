using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PlayerInventory : MonoBehaviour
    {
        int coin;
        List<GameObject> inventory = new List<GameObject>();
        [SerializeField] bool hasDeliveryItem = true; // TODO go from mock to real
        [SerializeField] Stats.Modifier[] modifiers; 

        public void AddCoin(int amount)
        {
            coin += amount;
        }

        public int GetCoinAmount()
        {
            return coin;
        }

        public void AddToInventory(GameObject gameObject)
        {
            inventory.Add(gameObject);
        }

        public bool IsPlayerCarrying()  // TODO pass paramater
        {
            return hasDeliveryItem;
        }

        public float GetAdditiveModifiers(Stats.Attribute attribute)
        {
            float total = 0;
            foreach (var modifier in modifiers)
            {
                if (modifier.modifierType != Stats.ModifierType.Additive) continue;
                if (modifier.attribute != attribute) continue;

                total += modifier.value;
            }

            return total;
        }

        public float GetMultiplicativeModifiers(Stats.Attribute attribute)
        {
            float total = 1;
            foreach (var modifier in modifiers)
            {
                if (modifier.modifierType != Stats.ModifierType.Multiplicative) continue;
                if (modifier.attribute != attribute) continue;

                total *= 1 + modifier.value / 100;
            }

            return total;
        }

    }
}