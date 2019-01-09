using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using UnityEngine.UI;

namespace RPG.UI
{
    public class HealthDisplay : MonoBehaviour
    {
        Image fill;
        HealthSystem health;

        // Start is called before the first frame update
        void Start()
        {
            fill = GetComponent<Image>();
            health = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>();
        }

        // Update is called once per frame
        void Update()
        {
            fill.fillAmount = health.healthAsPercentage;
        }
    }
}
