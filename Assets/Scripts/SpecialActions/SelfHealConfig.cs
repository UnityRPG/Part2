using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;

namespace RPG.SpecialActions
{
    [CreateAssetMenu(menuName = ("RPG/Special Abiltiy/Self Heal"))]
    public class SelfHealConfig : ConsumableConfig
	{
		[Header("Self Heal Specific")]
		[SerializeField] float extraHealth = 50f;

		public float GetExtraHealth()
		{
			return extraHealth;
		}

        public override void Use(GameObject source, GameObject target)
        {
			base.Use(source, target);
            PlayAbilityAnimation(source, () =>
            {
                PlayAbilitySound(source);
                var playerHealth = source.GetComponent<HealthSystem>();
                playerHealth.Heal(GetExtraHealth());
                PlayParticleEffect(source);
            });
        }
    }
}