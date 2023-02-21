using UnityEngine;
using RPG.Saving;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

namespace RPG.Scenemanagement
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string currentSaveKey = "currentSaveName";
        [SerializeField] float fadeInOnStart = 0.5f;
        [SerializeField] float fadeOutTime = 0.5f;
        [SerializeField] int firstLevelBuildIndex = 1;
        [SerializeField] int mainMenuSceneBuildIndex = 0;

        public void LoadMainMenu()
        {
            StartCoroutine(LoadFirstScene());
        }

        public void SaveAndQuit()
        {
            Save();
            LoadMainMenu();
        }

        public void ContinueGame()
        {
            if(!PlayerPrefs.HasKey(currentSaveKey)) return;
            if(!GetComponent<SavingSystem>().SaveFileExists(GetCurrentSave())) return;
            StartCoroutine(LoadLastScene());        
        }

        public void NewGame(string saveFile)
        {
            if(String.IsNullOrEmpty(saveFile)) return;
            SetCurrentSave(saveFile);
            StartCoroutine(StartNewGame(saveFile));  
        }

        public void LoadSelectedSave(string saveFile)
        {
            SetCurrentSave(saveFile);
            ContinueGame();
        }

        private void SetCurrentSave(string saveFile)
        {
            PlayerPrefs.SetString(currentSaveKey, saveFile);
        }

        private string GetCurrentSave()
        {
            return PlayerPrefs.GetString(currentSaveKey);
        }

        private void Update() 
        {
            if(Input.GetKeyDown(KeyCode.F5))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.F4))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }
        }
        
        private IEnumerator LoadLastScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            yield return GetComponent<SavingSystem>().LoadLastScene(GetCurrentSave());
            yield return fader.FadeIn(fadeInOnStart);
        }

        private IEnumerator StartNewGame(string saveFile)
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(firstLevelBuildIndex);
            yield return fader.FadeIn(fadeInOnStart);
            GetComponent<SavingSystem>().Save(saveFile);
        }

        private IEnumerator LoadFirstScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(mainMenuSceneBuildIndex);
            yield return fader.FadeIn(fadeInOnStart);
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(GetCurrentSave());
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(GetCurrentSave());
        }

        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(GetCurrentSave());
        }

        public IEnumerable<string> ListSaves()
        {
            return GetComponent<SavingSystem>().ListSaves();
        }
    }
}
