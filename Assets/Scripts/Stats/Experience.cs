using RPG.Saving;
using UnityEngine;
using System;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;
        [SerializeField] float keyEExpAmount = 1000f;

        public event Action onExperienceGained;

        private void Update() 
        {
#if UNITY_EDITOR
            GainExperienceWithKey();
#endif
        }
        
        public void GainExperience(float experience)
        {  
            experiencePoints += experience;
            onExperienceGained();
        }
#if UNITY_EDITOR
        private void GainExperienceWithKey()
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                GameObject player = GameObject.FindWithTag("Player");
                player.GetComponent<Experience>().GainExperience(keyEExpAmount);
            }
        }
#endif

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
        public float GetXP()
        {
            return experiencePoints;
        }
    }
}
