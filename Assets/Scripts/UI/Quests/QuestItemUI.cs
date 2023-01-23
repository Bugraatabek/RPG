using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Quests;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI progress;
        QuestStatus questStatus;
        
        public void Setup(QuestStatus questStatus)
        {
            this.questStatus = questStatus;
            title.text = questStatus.GetQuest().GetTitle();
            progress.text = questStatus.CompletedObjectives().Count() + "/" + questStatus.GetQuest().GetObjectiveCount();
        }

        public QuestStatus GetQuestStatus()    
        {
            return questStatus;
        }
    }
}
