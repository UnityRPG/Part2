using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PlayerInventory : MonoBehaviour
    {
        int coin;

        public void AddCoin(int amount)
        {
            coin += amount;
        }

        public int GetCoinAmount()
        {
            return coin;
        }
    }
}