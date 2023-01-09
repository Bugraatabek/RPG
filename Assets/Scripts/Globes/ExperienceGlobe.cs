using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using RPG.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Globes
{
    public class ExperienceGlobe : Globe
    {
       Experience experience;
       BaseStats baseStats;

        private void Awake() 
        {
            GameObject player = GameObject.FindWithTag("Player");
            experience = player.GetComponent<Experience>();
            baseStats = player.GetComponent<BaseStats>();
        }
        
        protected override void GainStat(float amount)
        {
            experience.GainExperience(amount);
        }

        protected override float MaxAmountFraction()
        {
           return baseStats.GetStat(Stat.ExperienceToLevelUp);
        }
    }
}


