using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Inventories
{
    public class Purse : MonoBehaviour, ISaveable
    {
        [SerializeField] float startingBalance = 400f;

        float balance = 0;

        public event Action balanceChanged;

        // SETTERS //
        
        private void Awake() 
        {
            balance = startingBalance; 
        }

        public void UpdateBalance(float amount)
        {
            balance += amount;
            if(balanceChanged != null)
            {
                balanceChanged();
            }
            
        }

        // SETTERS //
       
        // GETTERS //

        public float GetBalance()
        {
            return balance;
        }

        public object CaptureState()
        {
            return balance;
        }

        public void RestoreState(object state)
        {
            balance = (float)state;
        }

        // GETTERS //


    }
}
