using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using RPG.Scenemanagement;
using RPG.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        [SerializeField] Button resumeButton;
        [SerializeField] Button saveButton;
        [SerializeField] Button saveAndQuitButton;
        [SerializeField] Button quitButton;

        LazyValue<SavingWrapper> savingWrapper;
        PlayerController playerController;

        private void Awake() 
        {
            savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper);
            playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            resumeButton.onClick.AddListener(() => gameObject.SetActive(false));
            saveButton.onClick.AddListener(Save);
            saveAndQuitButton.onClick.AddListener(SaveAndQuit);
            quitButton.onClick.AddListener(Quit);
        }

        private void OnEnable() 
        {
            if(playerController == null) return;
            Time.timeScale = 0; //if 0 while using with time.deltaTime whichever function using time.deltatime won't work. Instead use time.unscaleddeltatime.
            playerController.enabled = false;
        }

        private void OnDisable() 
        {
            if(playerController == null) return;
            Time.timeScale = 1;
            playerController.enabled = true;
        }

        private void Quit()
        {
            savingWrapper.value.LoadMainMenu();
        }

        private void Save()
        {
            savingWrapper.value.Save();
        }

        private void SaveAndQuit()
        {
            Save();
            Quit();
        }

        private SavingWrapper GetSavingWrapper()
        {
            return FindObjectOfType<SavingWrapper>();
        }
    }
}
