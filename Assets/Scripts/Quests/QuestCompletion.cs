using System.Collections;
using System.Collections.Generic;
using RPG.Dialogue;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestCompletion : MonoBehaviour
    {
        [SerializeField] Quest quest;
        [SerializeField] TriggerType objectiveTrigger;
        Objective objective;

        private void Start() 
        {
            foreach (var newObjective in quest.GetObjectives())
            {
                if(objectiveTrigger == newObjective.triggerType)
                {
                    objective = newObjective;
                } 
            }
        }
        
        public void CompleteObjective()
        {
            var questList = GameObject.FindWithTag("Player").GetComponent<QuestList>();
            questList.CompleteObjective(quest, objective);
        }
    }
}
