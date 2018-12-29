using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;

namespace RPG.Abilities
{
    public class SelfHealBehaviour : AbilityBehaviour
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