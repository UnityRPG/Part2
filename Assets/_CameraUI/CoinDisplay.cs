using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class CoinDisplay : MonoBehaviour
    {

        PlayerInventory playerInventory;
        Text coinText;

        // Use this for initialization
        void Start()
        {
            playerInventory = FindObjectOfType<PlayerInventory>(); // assuming only player
            coinText = GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            coinText.text = playerInventory.GetCoinAmount().ToString();
        }
    }
}