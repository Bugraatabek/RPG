using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Quests
{
    public class QuestUI : MonoBehaviour
    {
        [SerializeField] Button quitButton;

        private void Start() 
        {
            ShowHideUI showHideUI = GetComponent<ShowHideUI>();
            quitButton.onClick.AddListener(() => showHideUI.Toggle());   
        }
    }
}
