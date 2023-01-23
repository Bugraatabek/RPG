using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] QuestItemUI questPrefab;
        
        QuestList questList;

        private void Awake() 
        {
            questList = GameObject.FindWithTag("Player").GetComponent<QuestList>();
        }
        
        private void Start()
        {
            questList.onQuestListUpdated += UpdateUI;
            UpdateUI();
        }

        private void UpdateUI()
        {
            foreach (Transform questItemUI in gameObject.transform)
            {
                Destroy(questItemUI.gameObject);
            }

            foreach (var status in questList.GetStatuses())
            {
                var questInstance = Instantiate<QuestItemUI>(questPrefab, transform);
                questInstance.Setup(status);
            }
        }
    }

}