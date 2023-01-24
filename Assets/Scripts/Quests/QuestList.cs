using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Inventories;
using RPG.Saving;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour, ISaveable, IPredicateEvaluator
    {
        List<QuestStatus> statuses = new List<QuestStatus>();
        public event Action onQuestListUpdated;


        public void AddQuest(Quest quest)
        {
            if(HasQuest(quest))
            {
                return;
            }
            var newStatus = new QuestStatus(quest);
            statuses.Add(newStatus);
            if(onQuestListUpdated != null)
            {
                onQuestListUpdated();
            }  
        }

        public void CompleteObjective(Quest quest, Objective objective)
        {
            QuestStatus status = GetQuestStatus(quest);
            status.CompleteObjective(objective);

            if(status.IsComplete())
            {
                GiveReward(quest);
            }
            
            
            if(onQuestListUpdated != null)
            {
                onQuestListUpdated();
            } 
        }

        private void GiveReward(Quest quest)
        {
            foreach (var reward in quest.GetRewards())
            {
                bool success = GetComponent<Inventory>().AddToFirstEmptySlot(reward.item, reward.number);
                if(!success)
                {
                    GetComponent<ItemDropper>().DropItem(reward.item, reward.number);
                }
            }
        }

        public bool? Evaluate(string predicate, string[] parameters)
        {
            switch(predicate)
            {
                case "HasQuest":
                return HasQuest(Quest.GetByName(parameters[0]));

                case "CompletedQuest":
                return GetQuestStatus(Quest.GetByName(parameters[0])).IsComplete();
            }

            return null;
        }

        //GETTERS//

        private bool HasQuest(Quest quest)
        {
            return GetQuestStatus(quest) != null;
        }

        public IEnumerable<QuestStatus> GetStatuses()
        {
            return statuses;
        }

        private QuestStatus GetQuestStatus(Quest quest)
        {
            foreach (QuestStatus status in GetStatuses())
            {
                if(status.GetQuest() == quest)
                {
                    return status;
                }  
            }
            return null;
        }
        
        //GETTERS//

        //SAVING//

        public object CaptureState()
        {
            List<object> state = new List<object>();
            foreach (QuestStatus status in GetStatuses())
            {
                state.Add(status.CaptureState());
            }
            return state;
        }

        public void RestoreState(object state)
        {
            List<object> stateList = state as List<object>;
            if(stateList == null) return;

            statuses.Clear();
            foreach (object objectState in stateList)
            {
                statuses.Add(new QuestStatus(objectState));
            }
        }

        //SAVING//
    }
}
