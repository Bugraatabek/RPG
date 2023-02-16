using UnityEngine;
using System.Collections.Generic;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/NewProgression", order = 0)]
    public class Progression : ScriptableObject 
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses;
        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;


        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup(); // building
            
            if(!lookupTable[characterClass].ContainsKey(stat))
            {
                Debug.Log($"Progression LookupDict Doesn't Contains The Key : {stat}");
                return 0;
            }
            
            float[] levels = lookupTable[characterClass][stat]; // using dictionary instead of foreach loop because dictionaries are more efficent than loops especially nested loops
            
            if(levels.Length == 0)
            {
                return 0;
            }
            
            if(levels.Length < level) 
            {
                return levels[levels.Length - 1];
            }
            if(level <= 0)
            {
                return levels[level - 1];
            }
            return levels[level - 1];   // level - 1 because unity starts to count from 0     
        }
        
        private void BuildLookup() // using foreach loop for once to build the dictionary for less usage of memory
        {
            if(lookupTable != null) return; //Lazy Evaluation making sure we use these loops only when the dictionary is null.
            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                var statLookupTable = new Dictionary<Stat, float[]>();
                foreach (ProgressionStat progressionStat in progressionClass.stats)
                {
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }
                lookupTable[progressionClass.characterClass] = statLookupTable;
            }
        }

        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            BuildLookup();

            float[] levels = lookupTable[characterClass][stat];
            return levels.Length;
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass; 
            public ProgressionStat[] stats;
        }
        
        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }
}
