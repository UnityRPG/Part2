using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.SpecialActions;

namespace RPG.UI
{
    public class ManaDisplay : MonoBehaviour
    {
        Image fill;
        SpecialAbilities abilities;


        // Start is called before the first frame update
        void Start()
        {
            fill = GetComponent<Image>();
            abilities = GameObject.FindGameObjectWithTag("Player").GetComponent<SpecialAbilities>();
        }

        // Update is called once per frame
        void Update()
        {
            fill.fillAmount = abilities.energyAsPercent;
        }
    }
}