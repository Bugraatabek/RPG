using System;
using RPG.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float hackHealAmount = 100;
        [SerializeField] UnityEvent<float> takeDamage;
        public UnityEvent die;
        [SerializeField] UnityEvent takeDamageSFX;
        [Tooltip("This should be null unless this is the Player GameObject")]
        [SerializeField] GameObject player = null;
        Health playerHealth;

        int takeDamageSFXCount = 2;
        bool wasDeadLastFrame = false;
        

        BaseStats baseStats;
        LazyValue<float> currentHealth;
        LazyValue<float> maxHealth;

        

        private void Awake() 
        {
            baseStats = GetComponent<BaseStats>();
            currentHealth = new LazyValue<float>(GetInitialHealth);
            maxHealth = new LazyValue<float>(GetInitialHealth);
            if(player != null)
            {
                playerHealth = player.GetComponent<Health>();
            }
        }

        private void Start() 
        {
            currentHealth.ForceInit();
            maxHealth.ForceInit();
        }

        private void Update()
        {
            if(player != null)
            {
                HealHack();
            }
            maxHealth.value = baseStats.GetStat(Stat.Health);

            if(player != null) 
            {
                StartCoroutine(RegenerateHealth());
            }
        }

        private float GetInitialHealth()
        {
            return baseStats.GetStat(Stat.Health);
        }

        private float GetRegenRate()
        {
            return baseStats.GetStat(Stat.HealthRegenRate);
        }

        private void OnEnable() 
        {
            baseStats.onLevelUp += FillMissingHP;
        }

        private void OnDisable() 
        {
            baseStats.onLevelUp -= FillMissingHP;
        }

        public void TakeDamage(GameObject instigator, float damage)
        { 
            takeDamageSFXCount ++;
            takeDamage.Invoke(damage); 
            currentHealth.value = Mathf.Max(currentHealth.value - damage, 0f);
            
            if(IsDead())
            {
                die.Invoke();
                AwardExperience(instigator);
            }

            if(takeDamageSFXCount == 3 && currentHealth.value > 0)
            {
                takeDamageSFX.Invoke();
                takeDamageSFXCount = 0;
            }
            UpdateState();
        }

        private void AwardExperience(GameObject instigator)
        {
            
            float experienceReward = GetComponent<BaseStats>().GetStat(Stat.ExperienceReward);
            Experience experience = instigator.GetComponent<Experience>();
            if(experience != null)
            {
                experience.GainExperience(experienceReward);
            }
            else return;
            
        }
        
        private void FillMissingHP()
        {
            maxHealth.value = GetComponent<BaseStats>().GetStat(Stat.Health);
            currentHealth.value = maxHealth.value;
        }

        public IEnumerator RegenerateHealth()
        {
            if(currentHealth.value <= 0)
            {
                yield return new WaitForSeconds(8);
            }
            if(currentHealth.value < GetMaxHealth())
            {
                currentHealth.value += Time.deltaTime * GetRegenRate();
                yield return null;
            }
            if (currentHealth.value >= GetMaxHealth())
            {
                currentHealth.value = GetMaxHealth();
                yield return null;
            }
        }

        public float GetPercentage()
        {
            return 100 * (currentHealth.value / maxHealth.value);
        }

        public float GetFraction()
        {
            return (currentHealth.value / maxHealth.value);
        }

        private void UpdateState()
        {
            if(!wasDeadLastFrame && IsDead())
            {
                GetComponent<Animator>().SetTrigger("die");
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }

            if(wasDeadLastFrame && !IsDead())
            {
                GetComponent<Animator>().Rebind();
            }
           

            wasDeadLastFrame = IsDead();
        }
        
        
        public bool IsDead()
        {
            return currentHealth.value <= 0;
        }

        public float GetCurrentHealth()
        {
            return currentHealth.value;
        }

        public float GetMaxHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void HealHack()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                playerHealth.Heal(hackHealAmount);
            }
        }

        public void Heal(float heal)
        { 
            currentHealth.value = Mathf.Min(currentHealth.value + heal, maxHealth.value);
            UpdateState();
        }

        public object CaptureState()
        {
            return currentHealth.value;
        }

        public void RestoreState(object state)
        {
            currentHealth.value = (float)state;
            UpdateState();
        }

    }
}