using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Stats
{
    public class TraitRowUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI valueText;
        [SerializeField] Button minusButton;
        [SerializeField] Button plusButton;
        [SerializeField] Button resetButton;
        [SerializeField] Trait trait;

        TraitStore traitStore;

        private void Awake() 
        {
            traitStore = GameObject.FindWithTag("Player").GetComponent<TraitStore>();    
        }
        private void Start() 
        {
            minusButton.onClick.AddListener(() => Allocate(-1));
            plusButton.onClick.AddListener(() => Allocate(+1));
            resetButton.onClick.AddListener(() => traitStore.Reset(trait));
        }

        private void Update() 
        {
            plusButton.interactable = traitStore.GetUnassignedPoints() > 0;
            minusButton.interactable = traitStore.GetStagedPoints(trait) > 0;
            valueText.text = $"{traitStore.GetProposedPoints(trait)}";
        }
        
        public void Allocate(int points)
        {
            traitStore.StagePoints(trait, points);
        }
    }
}
