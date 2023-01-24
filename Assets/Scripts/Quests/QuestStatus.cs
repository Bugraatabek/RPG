using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Quests
{
    
    public class QuestStatus
    {
        Quest quest;
        List<Objective> completedObjectives = new List<Objective>();

        //SETTERS//

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

        //SETTERS//

        //GETTERS//

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

        public bool IsComplete()
        {
            foreach (var objective in quest.GetObjectives())
            {
                if(!completedObjectives.Contains(objective))
                {
                    return false;
                }
            }
            return true;
            
            // if(GetQuest().GetObjectiveCount() == completedObjectives.Count())
            // {
            //     return true;
            // }
            // return false;
        }
        
        //GETTERS//
        
        //SAVING//

        [System.Serializable]
        class QuestStatusRecord
        {
            public string questName;
            public List<string> completedObjectives = new List<string>();
        }

        public QuestStatus(object objectState)
        {
            QuestStatusRecord state = objectState as QuestStatusRecord;
            quest = Quest.GetByName(state.questName);
            
            List<string> completedObjectiveRefs = new List<string>();
            completedObjectiveRefs = state.completedObjectives;

            foreach (var objectiveReference in completedObjectiveRefs)
            {
                Objective completedObjective = quest.ObjectiveLookup(objectiveReference);
                completedObjectives.Add(completedObjective);
            }
        }

        public object CaptureState()
        {
            quest.BuildLookup();
            var state = new QuestStatusRecord();
            state.questName = quest.GetTitle();
            foreach (var objective in completedObjectives)
            {
                state.completedObjectives.Add(objective.reference);
            }
            return state;
        }
    
        //SAVING//
    }

    
}
