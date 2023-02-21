using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Scenemanagement;
using RPG.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RPG.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("Main Menu")]
        [SerializeField] GameObject mainMenu;
        [SerializeField] Button continueButton;
        [SerializeField] Button newGameButton;
        [SerializeField] Button loadButton;
        [SerializeField] Button quitButton;
        
        [Header("New Game Menu")]
        [SerializeField] GameObject newGameMenu;
        [SerializeField] Button backFromNewGameButton;
        [SerializeField] Button createNewGameButton;
        [SerializeField] TMPro.TMP_InputField newGameName;

        [Header("Load Menu")]
        [SerializeField] GameObject loadMenu;
        [SerializeField] Button backFromLoadButton;
        
        
        [Header("Elements of UI")]
        [SerializeField] UISwitcher switcher;
        
        
        LazyValue<SavingWrapper> savingWrapper;

        private void Awake() 
        {
            savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper);
            // Main Menu Buttons //
            continueButton.onClick.AddListener(ContinueGame);
            newGameButton.onClick.AddListener(() => switcher.SwitchTo(newGameMenu));
            loadButton.onClick.AddListener(() => switcher.SwitchTo(loadMenu));
            quitButton.onClick.AddListener(Quit);

            // New Game Menu Buttons //
            backFromNewGameButton.onClick.AddListener(() => switcher.SwitchTo(mainMenu));
            createNewGameButton.onClick.AddListener(() => NewGame(newGameName.text));

            // Load Menu Buttons //
            backFromLoadButton.onClick.AddListener(() => switcher.SwitchTo(mainMenu));
        }

        private SavingWrapper GetSavingWrapper()
        {
            return FindObjectOfType<SavingWrapper>();
        }

        private void ContinueGame()
        {
            savingWrapper.value.ContinueGame();
        }

        private void SaveGame()
        {
            savingWrapper.value.Save();
        }

        private void NewGame(string saveFile)
        {
            savingWrapper.value.NewGame(saveFile);
        }

        private void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

    }
}
