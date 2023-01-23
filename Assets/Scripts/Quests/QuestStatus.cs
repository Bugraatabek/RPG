using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    
    public class QuestStatus
    {
        Quest quest;
        List<Objective> completedObjectives = new List<Objective>();

        public QuestStatus(Quest quest)
        {
            this.quest = quest;
        }

        public void CompleteObjective(Objective objective)
        {
            if(quest.HasObjective(objective))
            {
                completedObjectives.Add(objective);
            }
        }

        public Quest GetQuest()
        {
            return quest;
        }

        public IEnumerable<Objective> CompletedObjectives()
        {
            foreach (Objective completedObjective in completedObjectives)
            {
                yield return completedObjective;
            }
        }

        public bool IsObjectiveCompleted(Objective objective)
        {
            return completedObjectives.Contains(objective);
        }
    }
}
