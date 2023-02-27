using System.Collections.Generic;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Abilities
{
    public class CooldownStore : MonoBehaviour 
    {
        Dictionary<InventoryItem, float> cooldownTimers = new Dictionary<InventoryItem, float>();
        Dictionary<InventoryItem, float> initialCooldownTimes = new Dictionary<InventoryItem, float>();

        
        void Update()
        {
            var keys = new List<InventoryItem>(cooldownTimers.Keys);
            foreach (InventoryItem inventoryItem in keys)
            {
                cooldownTimers[inventoryItem] -= Time.deltaTime;
                if(cooldownTimers[inventoryItem] <= 0)
                {
                    cooldownTimers.Remove(inventoryItem);
                    initialCooldownTimes.Remove(inventoryItem);
                }
            }     
        }
        
        public void StartCooldown(InventoryItem ability, float cooldownTime)
        {
            cooldownTimers[ability] = cooldownTime;
            initialCooldownTimes[ability] = cooldownTime; 
        }

        public float GetTimeRemaining(InventoryItem ability)
        {
            if(!cooldownTimers.ContainsKey(ability))
            {
                return 0;
            }
            return cooldownTimers[ability];
        }
        
        public float GetFractionRemaining(InventoryItem inventoryItem)
        {
            if(!initialCooldownTimes.ContainsKey(inventoryItem)) return 0;
            
            float cooldownTime = initialCooldownTimes[inventoryItem];
            float timeRemaining = GetTimeRemaining(inventoryItem);
            
            float cooldownPercantage = timeRemaining / cooldownTime;
            return cooldownPercantage;
        }
    }
}