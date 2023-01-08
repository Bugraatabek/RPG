using System;
using RPG.Utils;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)] [SerializeField] int level = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpEffect = null;
        [SerializeField] bool shouldUseModifiers = false;

        public event Action onLevelUp;
        Experience experience = null;
        LazyValue<int> currentLevel;

        
        private void Awake() 
        {
            currentLevel = new LazyValue<int>(CalculateLevel);
            experience = GetComponent<Experience>();
        }
        
        private void Start()
        {
            currentLevel.ForceInit();
            level = currentLevel.value;
        }

        private void OnEnable()
        {
            if(experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable() 
        {
            if(experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }

        private void UpdateLevel() 
        {
            int newLevel = CalculateLevel();
            if(newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                newLevel = level;
                LevelUpEffect();
                onLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            if (levelUpEffect != null)
            {
                GameObject lvlUpFx = Instantiate(levelUpEffect, transform);
                Destroy(lvlUpFx, 2f);
            }
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat)/100);
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if(!shouldUseModifiers) return 0;
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach(float modifier in provider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if(!shouldUseModifiers) return 0;
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach(float modifier in provider.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        public int GetLevel()
        {
            if( experience == null) {return level;}
            return currentLevel.value;
            // if (currentLevel < 1) before lazy value currentLevel was set to 0. so against any race conditions in case the currentLevel wasn't set, this is a safeguard.
            // the progression is calling [level - 1] so level gets calculated as (0 - 1 = -1) and this is out of bounds of the array so it was throwing an exception 
            // {
            //     currentLevel = CalculateLevel(); this will calculate the level and return the "wanted" value of currentLevel
                    
            // } 
            // return currentLevel;
            // with lazyvalue we are making sure that currentLevel is set to wanted value whenever its needed, so we don't use this safeguard anymore.

        }

        public int CalculateLevel()
        {
            if(experience == null) { return level;} 
            float currentXp = experience.GetXP();
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float xpToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if(xpToLevelUp > currentXp)
                {
                    return level;
                }
                
            }
            return penultimateLevel + 1;
        }

        
    }
}
