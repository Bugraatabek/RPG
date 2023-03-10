using System;
using System.Collections.Generic;
using RPG.Inventories;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class TraitStore : MonoBehaviour, IModifierProvider, ISaveable
    {
        [SerializeField] float resetPricePerPoint = 50;
        [SerializeField] TraitBonus[] bonusConfig;
        [System.Serializable]
        
        class TraitBonus
        {
            public Trait trait;
            public Stat stat;
            public float additiveBonusPerPoint = 0;
            public float percentageBonusPerPoint = 0;
        }
        
        Dictionary<Trait, int> assignedPoints = new Dictionary<Trait, int>();
        Dictionary<Trait, int> stagedPoints = new Dictionary<Trait, int>();

        Dictionary<Stat, Dictionary<Trait, float>> additiveBonusCache;
        Dictionary<Stat, Dictionary<Trait, float>> percentageBonusCache;

        int unassignedPoints = 0;

        private void Awake() 
        {
            additiveBonusCache = new Dictionary<Stat, Dictionary<Trait, float>>();
            percentageBonusCache = new Dictionary<Stat, Dictionary<Trait, float>>();
            
            foreach (var bonus in bonusConfig)
            {
                if(!additiveBonusCache.ContainsKey(bonus.stat))
                {
                    additiveBonusCache[bonus.stat] = new Dictionary<Trait, float>();
                }
                if(!percentageBonusCache.ContainsKey(bonus.stat))
                {
                    percentageBonusCache[bonus.stat] = new Dictionary<Trait, float>();
                }
                additiveBonusCache[bonus.stat][bonus.trait] = bonus.additiveBonusPerPoint;
                percentageBonusCache[bonus.stat][bonus.trait] = bonus.percentageBonusPerPoint;
            }    
        }

        public void StagePoints(Trait trait, int points)
        {
            //unassignedPoints -= points;
            if(!stagedPoints.ContainsKey(trait))
            {
                stagedPoints[trait] = points;
                return;
            }
            stagedPoints[trait] += points;
        }

        public int GetUnassignedPoints()
        {
            return unassignedPoints = GetAssignablePointsByLevel() - GetTotalProposedPoints();
        }

        private int GetTotalProposedPoints()
        {
            int totalProposedPoints = 0;
            foreach (var value in assignedPoints.Values)
            {
                totalProposedPoints += value;
            }
            foreach (var value in stagedPoints.Values)
            {
                totalProposedPoints += value;
            }
            return totalProposedPoints;
        }

        public int GetAssignedPoints(Trait trait)
        {
            // "?" question mark represents the if statement, ":" colon represents else in case we don't have the key just return 0
            return assignedPoints.ContainsKey(trait) ? assignedPoints[trait] : 0;
        }

        public int GetStagedPoints(Trait trait)
        {
            // "?" question mark represents the if statement, ":" colon represents else in case we don't have the key just return 0
            return stagedPoints.ContainsKey(trait) ? stagedPoints[trait] : 0;
        }

        public int GetProposedPoints(Trait trait)
        {
            return GetAssignedPoints(trait) + GetStagedPoints(trait);
        }

        public void Commit()
        {
            foreach (Trait trait in stagedPoints.Keys)
            {
                this.assignedPoints[trait] = GetProposedPoints(trait);
            }
            stagedPoints.Clear();
        }

        public int GetAssignablePointsByLevel()
        {
            return (int)GetComponent<BaseStats>().GetStat(Stat.TotalTraitPoints);
        }

        public void Reset(Trait trait)
        {
            if(!assignedPoints.ContainsKey(trait)) return;
            
            float totalPrice = assignedPoints[trait] * resetPricePerPoint;
            Purse purse = GetComponent<Purse>();
            if(totalPrice > purse.GetBalance()) return;
            
            unassignedPoints += assignedPoints[trait];
            purse.UpdateBalance(-totalPrice);
            assignedPoints[trait] = 0;
        }

        public void ResetAll()
        {
            var totalPoints = 0;
            foreach (Trait trait in assignedPoints.Keys)
            {
                totalPoints += assignedPoints[trait];
            }
            float totalPrice = totalPoints * resetPricePerPoint;
            Purse purse = GetComponent<Purse>(); 
            if(totalPrice > purse.GetBalance()) return;
            unassignedPoints += totalPoints;
            purse.UpdateBalance(-totalPrice);
            assignedPoints.Clear();
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if(!additiveBonusCache.ContainsKey(stat)) yield break;
            foreach (Trait trait in additiveBonusCache[stat].Keys)
            {
                yield return additiveBonusCache[stat][trait] * GetAssignedPoints(trait);
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if(!percentageBonusCache.ContainsKey(stat)) yield break;
            foreach (Trait trait in percentageBonusCache[stat].Keys)
            {
                yield return percentageBonusCache[stat][trait] * GetAssignedPoints(trait);
            }
        }

        public object CaptureState()
        {
            return assignedPoints;
        }

        public void RestoreState(object state)
        {
            assignedPoints = new Dictionary<Trait, int>((Dictionary<Trait, int>)state);
        }
    }   
}