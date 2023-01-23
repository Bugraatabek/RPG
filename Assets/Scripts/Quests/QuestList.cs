using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour
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
    }
}
