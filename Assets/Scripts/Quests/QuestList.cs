using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour, ISaveable
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
            GetQuestStatus(quest).CompleteObjective(objective);
            
            
            if(onQuestListUpdated != null)
            {
                onQuestListUpdated();
            } 
        }

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
            foreach (var status in GetStatuses())
            {
                if(status.GetQuest() == quest)
                {
                    return status;
                }  
            }
            return null;
        }

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
    }
}
