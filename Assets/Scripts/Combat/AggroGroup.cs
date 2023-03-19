using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.Combat
{
    public class AggroGroup : MonoBehaviour, ISaveable
    {
        List<Fighter> fighters = new List<Fighter>();
        [SerializeField] bool activateOnStart = false;


        private void Awake() 
        {
            PopulateFighters();
        }

        private void PopulateFighters()
        {
            foreach (Fighter enemy in GetComponentsInChildren<Fighter>())
            {
                fighters.Add(enemy);
            }
            Activate(activateOnStart);
        }

        public void Activate(bool shouldActivate)
        {
            activateOnStart = shouldActivate;
            foreach (Fighter fighter in fighters)
            {
                CombatTarget target = fighter.GetComponent<CombatTarget>();
                if(target != null)
                {
                    target.enabled = shouldActivate;
                }
                fighter.enabled = shouldActivate;
            }
        }

        public object CaptureState()
        {
            return activateOnStart;
        }

        public void RestoreState(object state)
        {
            bool shouldActivate = (bool)state;
            Activate(shouldActivate);
        }
    }
}
