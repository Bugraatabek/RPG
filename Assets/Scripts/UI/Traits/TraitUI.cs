using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Stats
{
    public class TraitUI : MonoBehaviour 
    {
        [SerializeField] Button confirmButton;
        [SerializeField] Button resetAllButton;
        [SerializeField] TextMeshProUGUI unallocatedPointsText;
        TraitStore traitStore;

        private void Start() 
        {
            traitStore = GameObject.FindWithTag("Player").GetComponent<TraitStore>();
            confirmButton.onClick.AddListener(traitStore.Commit);
            resetAllButton.onClick.AddListener(traitStore.ResetAll);
        }

        private void Update() 
        {
            unallocatedPointsText.text = $"{traitStore.GetUnassignedPoints()}";    
        }
    }
}