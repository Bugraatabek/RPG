using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;
using UnityEngine.AI;
using RPG.Attributes;

namespace RPG.Globes
{
    public class HealthGlobe : Globe
    {
        Health health;

        private void Awake() 
        {
            GameObject player = GameObject.FindWithTag("Player");
            health = player.GetComponent<Health>();
        }
        
        protected override void GainStat(float amount)
        {
            health.Heal(amount);
        }

        protected override float MaxAmountFraction()
        {
           return health.GetMaxHealth();
        }
    }
}

