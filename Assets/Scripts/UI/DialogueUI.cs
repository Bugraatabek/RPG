using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Dialogue;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant playerConversant;
        [SerializeField] TextMeshProUGUI AIText;
        [SerializeField] Button nextButton;
        [SerializeField] Button quitButton;

        private void Start() 
        {
            playerConversant = GameObject.FindWithTag("Player").GetComponent<PlayerConversant>();
            nextButton.onClick.AddListener(Next);
            quitButton.onClick.AddListener(Quit);

            UpdateUI();
        }

        public void Next()
        {
            playerConversant.Next();
            UpdateUI();
        }

        public void Quit()
        {
            gameObject.SetActive(false);
        }

        void UpdateUI()
        {
            AIText.text = playerConversant.GetText();
            nextButton.gameObject.SetActive(playerConversant.HasNext());
        }
    }
}