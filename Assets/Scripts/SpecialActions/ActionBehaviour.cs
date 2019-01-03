using RPG.Core;
using System.Collections;
using UnityEngine;

namespace RPG.SpecialActions
{
    public abstract class ActionBehaviour : MonoBehaviour
    {
        protected ActionConfig config;

        const float PARTICLE_CLEAN_UP_DELAY = 20f;

        public abstract void Use(GameObject target = null);

        public virtual bool IsInRange(GameObject target) => true;

        public virtual bool CanUseWhenInRange(GameObject target) => true;

        public void SetConfig(ActionConfig configToSet)
        {
            config = configToSet;
        }

        protected void PlayParticleEffect()
        {
            var particlePrefab = config.GetParticlePrefab();
            var particleObject = Instantiate(
                particlePrefab,
                transform.position,
                particlePrefab.transform.rotation
            );
            particleObject.transform.parent = transform; // set world space in prefab if required
            particleObject.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DestroyParticleWhenFinished(particleObject));
        }

        IEnumerator DestroyParticleWhenFinished(GameObject particlePrefab)
        {
            while (particlePrefab.GetComponent<ParticleSystem>().isPlaying)
            {
                yield return new WaitForSeconds(PARTICLE_CLEAN_UP_DELAY);
            }
            Destroy(particlePrefab);
            yield return new WaitForEndOfFrame();
        }

        protected void PlayAbilityAnimation(System.Action callback)
        {
            var animator = GetComponent<Animator>();
            var switchableAnimation = animator.GetBehaviour<SwappableAction>();
            switchableAnimation.ReplaceNextAction(animator, config.GetAbilityAnimation(), callback);
        }

        protected void PlayAbilitySound()
        {
            var abilitySound = config.GetRandomAbilitySound();
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(abilitySound);
        }
    }
}