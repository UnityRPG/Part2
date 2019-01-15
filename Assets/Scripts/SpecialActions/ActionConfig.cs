﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Inventories;

namespace RPG.SpecialActions
{
    public abstract class ActionConfig : InventoryItem
    {
        [Header("Spcial Ability General")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] GameObject particlePrefab;
        [SerializeField] AnimationClip abilityAnimation;
        [SerializeField] AudioClip[] audioClips;

        const float PARTICLE_CLEAN_UP_DELAY = 20f;

        public virtual bool IsInRange(GameObject source, GameObject target) => true;

        public virtual bool CanUseWhenInRange(GameObject source, GameObject target) => true;

        public abstract void Use(GameObject source, GameObject target);

        public float GetEnergyCost() => energyCost;

        public GameObject GetParticlePrefab() => particlePrefab;

        public AnimationClip GetAbilityAnimation()
        {
            abilityAnimation.events = new AnimationEvent[0]; // TODO consdier centralising with RemoveAnimationEvents()
            return abilityAnimation;
        }

        public AudioClip GetRandomAbilitySound()
        {
            return audioClips[Random.Range(0, audioClips.Length)];
        }

        protected void PlayParticleEffect(GameObject source)
        {
            var particlePrefab = GetParticlePrefab();
            var particleObject = Instantiate(
                particlePrefab,
                source.transform.position,
                particlePrefab.transform.rotation
            );
            particleObject.transform.parent = source.transform; // set world space in prefab if required
            particleObject.GetComponent<ParticleSystem>().Play();
            source.GetComponent<SpecialAbilities>().StartCoroutine(DestroyParticleWhenFinished(particleObject));
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

        protected void PlayAbilityAnimation(GameObject source, System.Action callback)
        {
            var animator = source.GetComponent<Animator>();
            var switchableAnimation = animator.GetBehaviour<SwappableAction>();
            switchableAnimation.ReplaceNextAction(animator, GetAbilityAnimation(), callback);
        }

        protected void PlayAbilitySound(GameObject source)
        {
            var abilitySound = GetRandomAbilitySound();
            var audioSource = source.GetComponent<AudioSource>();
            audioSource.PlayOneShot(abilitySound);
        }
    }
}