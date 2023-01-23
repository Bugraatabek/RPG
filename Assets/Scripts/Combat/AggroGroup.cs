using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class AggroGroup : MonoBehaviour
    {
        List<Fighter> fighters = new List<Fighter>();
        [SerializeField] bool activateOnStart = false;

        private void Start() 
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
    }
}
