using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestTooltipUI : MonoBehaviour
    {
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
                    completedObjectiveInstance.GetComponentInChildren<TextMeshProUGUI>().text = objective.text;
                }
                else
                {
                    var objectiveInstance = Instantiate(objectivePrefab, objectiveList);
                    objectiveInstance.GetComponentInChildren<TextMeshProUGUI>().text = objective.text; 
                }
            }
        }
    }

}