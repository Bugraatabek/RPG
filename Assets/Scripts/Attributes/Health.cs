using System;
using RPG.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;


namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float hackHealAmount = 100;
        [SerializeField] UnityEvent<float> takeDamage;
        [SerializeField] UnityEvent die;
        [SerializeField] UnityEvent takeDamageSFX;

        int takeDamageSFXCount = 2;
        bool isDead = false;
        

        BaseStats baseStats;
        LazyValue<float> currentHealth;
        LazyValue<float> maxHealth;

        

        private void Awake() 
        {
            baseStats = GetComponent<BaseStats>();
            currentHealth = new LazyValue<float>(GetInitialHealth);
            maxHealth = new LazyValue<float>(GetInitialHealth);
        }

        private void Start() 
        {
            currentHealth.ForceInit();
            maxHealth.ForceInit();
        }

        private void Update()
        {
            HealHack();
            maxHealth.value = baseStats.GetStat(Stat.Health);
        }

        private float GetInitialHealth()
        {
            return baseStats.GetStat(Stat.Health);
        }

        private void OnEnable() 
        {
            baseStats.onLevelUp += RegenerateHealth;
        }

        private void OnDisable() 
        {
            baseStats.onLevelUp -= RegenerateHealth;
        }

        public void TakeDamage(GameObject instigator, float damage)
        { 
            takeDamageSFXCount ++;
            takeDamage.Invoke(damage); 
            currentHealth.value = Mathf.Max(currentHealth.value - damage, 0f);
            
            if(currentHealth.value == 0 && isDead == false)
            {
                Die();
                die.Invoke();
                AwardExperience(instigator);
            }

            if(takeDamageSFXCount == 3 && currentHealth.value > 0)
            {
                takeDamageSFX.Invoke();
                takeDamageSFXCount = 0;
            }
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
        
        private void RegenerateHealth()
        {
            maxHealth.value = GetComponent<BaseStats>().GetStat(Stat.Health);
            currentHealth.value = maxHealth.value;
        }

        public float GetPercentage()
        {
            return 100 * (currentHealth.value / maxHealth.value);
        }

        public float GetFraction()
        {
            return (currentHealth.value / maxHealth.value);
        }

        private void Die()
        {
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
        
        
        public bool IsDead()
        {
            return isDead;
        }

        public float GetCurrentHealth()
        {
            return currentHealth.value;
        }

        public float GetMaxHealth()
        {
            return maxHealth.value;
        }

        private void HealHack()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                Heal(hackHealAmount);
            }
        }

        public void Heal(float heal)
        {
            GameObject player = GameObject.FindWithTag("Player");   
            Health playerHealth = player.GetComponent<Health>();
            playerHealth.currentHealth.value = Mathf.Min(playerHealth.currentHealth.value + heal, playerHealth.maxHealth.value);
        }

        public object CaptureState()
        {
            return currentHealth.value;
        }

        public void RestoreState(object state)
        {
            currentHealth.value = (float)state;
            if(currentHealth.value == 0 && isDead == false)
            {
                Die();
            }
        }

    }
}