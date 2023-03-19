using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Dialogue;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant playerConversant;
        [SerializeField] TextMeshProUGUI AIText;
        [SerializeField] TextMeshProUGUI conversantName;
        
        [SerializeField] Button nextButton;
        [SerializeField] Button quitButton;
        [SerializeField] Button choiceButton;

        [SerializeField] Transform choiceButtonsTab;

        private void Start() 
        {
            playerConversant = GameObject.FindWithTag("Player").GetComponent<PlayerConversant>();
            playerConversant.onConversationUpdated += UpdateUI;

            
            nextButton.onClick.AddListener(() => playerConversant.Next());
            quitButton.onClick.AddListener(() => playerConversant.Quit());

            UpdateUI();
        }

        void UpdateUI()
        {
            if(!playerConversant.IsActive())
            {
                gameObject.SetActive(false);
                return;
            }
                gameObject.SetActive(true);
                choiceButtonsTab.gameObject.SetActive(false);
                AIText.gameObject.SetActive(true);
                conversantName.text = playerConversant.GetCurrentConversantName();
                AIText.text = playerConversant.GetText();
                nextButton.gameObject.SetActive(playerConversant.HasNext());
            
            if(playerConversant.IsChoosing())
            {
                BuildChoiceList();
            }
        }

        private void BuildChoiceList()
        {
            choiceButtonsTab.gameObject.SetActive(true);
            AIText.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);
            conversantName.text = playerConversant.GetCurrentConversantName();

            foreach (Transform button in choiceButtonsTab)
            {
                Destroy(button.gameObject);
            }

            foreach (var choice in playerConversant.GetChoices())
            {
                var choiceButtonInstance = Instantiate(choiceButton, choiceButtonsTab);
                choiceButtonInstance.GetComponentInChildren<TextMeshProUGUI>().text = choice.GetText();
                Button button = choiceButtonInstance.GetComponentInChildren<Button>();
                button.onClick.AddListener(() => { playerConversant.SelectChoice(choice); } );
            }
        }
    }
}