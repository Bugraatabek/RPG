using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "RPG/Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        [SerializeField] List<Objective> objectives = new List<Objective>();
        Dictionary<string,Objective> objectiveLookup = null;

        [SerializeField]List<Reward> rewards = new List<Reward>();

        [System.Serializable]
        public class Reward
        {
            [Min(1)] public int number;
            public InventoryItem item;
        }

        public void BuildLookup()
        {
            if(objectiveLookup != null) return;
            objectiveLookup = new Dictionary<string, Objective>();
            foreach (var objective in objectives)
            {
                objectiveLookup.Add(objective.reference, objective);
            }
        }

        //GETTERS//

        public Objective ObjectiveLookup(string objectiveReference)
        {
            BuildLookup();
            return objectiveLookup[objectiveReference];
        }
        
        public string GetTitle()
        {
            return name;
        }

        public int GetObjectiveCount()
        {
            return objectives.Count;
        }

        public IEnumerable<Objective> GetObjectives()
        {
            return objectives;
        }

        public IEnumerable<Reward> GetRewards()
        {
            return rewards;
        }

        public bool HasObjective(Objective objective)
        {
            return objectives.Contains(objective);
        }

        public static Quest GetByName(string questName)
        {
            foreach(Quest quest in Resources.LoadAll<Quest>(""))
            {
                if(quest.name == questName)
                {
                    return quest;
                }
            }
            return null;
        }

        //GETTERS//
    }
}
