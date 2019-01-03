using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SpecialActions
{
    [CreateAssetMenu(menuName = ("RPG/Special Abiltiy/Self Heal"))]
    public class SelfHealConfig : ActionConfig
	{
		[Header("Self Heal Specific")]
		[SerializeField] float extraHealth = 50f;

        public override ActionBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<SelfHealBehaviour>();
        }

		public float GetExtraHealth()
		{
			return extraHealth;
		}
	}
}