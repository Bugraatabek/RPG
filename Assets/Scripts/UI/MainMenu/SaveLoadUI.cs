using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Scenemanagement;
using RPG.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class SaveLoadUI : MonoBehaviour
    {
        [SerializeField] Transform content;
        [SerializeField] GameObject buttonPrefab;
        LazyValue<SavingWrapper> savingWrapper;

        
        private void Awake() 
        {
            savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper);
        }

        private SavingWrapper GetSavingWrapper()
        {
            return FindObjectOfType<SavingWrapper>();
        }

        private void OnEnable() 
        {
            print("Loading Saves...");
            foreach (Transform file in content)
            {
                Destroy(file.gameObject);
            }

            foreach (string saveFile in savingWrapper.value.ListSaves())
            {
                var buttonInstance = Instantiate(buttonPrefab, content);
                buttonInstance.GetComponentInChildren<TMP_Text>().text = saveFile;
                buttonInstance.GetComponentInChildren<Button>().onClick.AddListener( () => savingWrapper.value.LoadSelectedSave(saveFile) );
            }
            
        }
        
    }
}