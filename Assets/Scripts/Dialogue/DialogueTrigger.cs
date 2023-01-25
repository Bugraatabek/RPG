using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] ETrigger action;
        [SerializeField] UnityEvent onTrigger;

        public void Trigger(ETrigger actionToTrigger)
        {
            if(actionToTrigger == action)
            {
                onTrigger.Invoke();
            }
        }
    }
}