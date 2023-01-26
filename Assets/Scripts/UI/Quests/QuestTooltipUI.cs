using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Quests;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestTooltipUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI rewardText;
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] Transform objectiveList;
        [SerializeField] GameObject objectivePrefab;
        [SerializeField] GameObject completedObjectivePrefab;

        public void Setup(QuestStatus questStatus)
        {
            Quest quest = questStatus.GetQuest();
            title.text = quest.GetTitle();
            
            foreach (Transform objective in objectiveList)
            {
                Destroy(objective.gameObject);
            }
            
            foreach (var objective in quest.GetObjectives())
            {
                if(questStatus.IsObjectiveCompleted(objective) == true)
                {
                    var completedObjectiveInstance = Instantiate(completedObjectivePrefab, objectiveList);
                    completedObjectiveInstance.GetComponentInChildren<TextMeshProUGUI>().text = objective.description;
                }
                else
                {
                    var objectiveInstance = Instantiate(objectivePrefab, objectiveList);
                    objectiveInstance.GetComponentInChildren<TextMeshProUGUI>().text = objective.description; 
                }
            }
                rewardText.text = GetRewardText(quest);
           
        }

        private string GetRewardText(Quest quest)
        {
            string rewardText = "";
            foreach (var reward in quest.GetRewards())
            {
                if(rewardText != "")
                {
                    rewardText += ", ";
                }
                if(reward.number > 1)
                {
                    rewardText += reward.number + " ";
                }
                rewardText += reward.item.GetDisplayName();
            }
            if(rewardText == "")
            {
                rewardText = "No Reward";
            }
            rewardText += ".";
            return rewardText;
        }
    }

}