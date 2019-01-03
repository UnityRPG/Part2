using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;

namespace RPG.SpecialActions
{
    public class SelfHealBehaviour : ActionBehaviour
    {
		public override void Use(GameObject target)
		{
            PlayAbilityAnimation(() =>
            {
                PlayAbilitySound();
                var playerHealth = GetComponent<HealthSystem>();
                playerHealth.Heal((config as SelfHealConfig).GetExtraHealth());
                PlayParticleEffect();
            });
		}
    }
}