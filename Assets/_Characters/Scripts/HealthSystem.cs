using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using RPG.Core.Saving;

namespace RPG.Characters
{
    public class HealthSystem : MonoBehaviour, ISaveable
    {
        float maxHealthPoints
        {
            get
            {
                var ec = GetComponent<CharacterLevel>();
                if (ec == null)
                {
                    return 0;
                }
                return ec.health;
            }
        }
        [SerializeField] Image healthBar;
        [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;
        [SerializeField] float deathVanishSeconds = 2.0f;

        const string DEATH_TRIGGER = "Death";

        Animator animator;
        AudioSource audioSource;
        Character characterMovement;
		
        float currentHealthPoints;
        bool healthPointsSetByRestore = false;

        public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

        public bool isAlive
        {
            get
            {
                return currentHealthPoints > 0;
            }
        }
        
        void Start()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            characterMovement = GetComponent<Character>();

            if (!healthPointsSetByRestore)
            {
                currentHealthPoints = maxHealthPoints;
            }
        }

        void Update()
        {
            UpdateHealthBar();
            CheckShouldCharacterDie();
        }

        void UpdateHealthBar()
        {
            if (healthBar) // Enemies may not have health bars to update
            {
                healthBar.fillAmount = healthAsPercentage;
            }
        }

        public void TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            var clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
            audioSource.PlayOneShot(clip);
        }

        private void CheckShouldCharacterDie()
        {
            if (!isAlive)
            {
                StartCoroutine(KillCharacter());
            }
        }

        public void Heal(float points)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + points, 0f, maxHealthPoints);
        }

        IEnumerator KillCharacter()
        {
            animator.SetTrigger(DEATH_TRIGGER);

            audioSource.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
            audioSource.Play(); // overrind any existing sounds
            yield return new WaitForSecondsRealtime(audioSource.clip.length);

            var playerComponent = GetComponent<PlayerControl>();
            if (playerComponent && playerComponent.isActiveAndEnabled) // relying on lazy evaluation
            {
                SceneManager.LoadScene(0);
            }
            else // assume is enemy fr now, reconsider on other NPCs
            {
                yield return new WaitForSecondsRealtime(deathVanishSeconds);
                gameObject.SetActive(false);
            }
        }

        public void CaptureState(IDictionary<string, object> state)
        {
            state["currentHealthPoints"] = currentHealthPoints;
        }

        public void RestoreState(IReadOnlyDictionary<string, object> state)
        {
            currentHealthPoints = (float)state["currentHealthPoints"];
            healthPointsSetByRestore = true;
            gameObject.SetActive(isAlive);
        }
    }
}