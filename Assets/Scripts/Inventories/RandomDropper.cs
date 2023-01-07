using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventories
{
    public class RandomDropper : ItemDropper
    {
        // CONFIG DATA
        [Tooltip("How far can the pickups be scattered from the dropper.")]
        [SerializeField] float scatterDistance = 1f;
        [SerializeField] DropLibrary dropLibrary;
        [SerializeField] int numberOfDrops = 2;
        
        // CONSTANTS
        const int ATTEMPTS = 30;
        
        public void RandomDrop()
        {
            if(dropLibrary == null) return;
            var baseStats = GetComponent<BaseStats>();
            for (int i = 0; i < numberOfDrops; i++)
            {
                var drops = dropLibrary.GetRandomDrops(baseStats.GetLevel());
                foreach (var drop in drops)
                {
                    DropItem(drop.item, drop.number);
                }
            }
            
        }
        
        protected override Vector3 GetDropLocation()
        {
            for(int i = 0; i < ATTEMPTS; i++)
            {
                Vector3 randomPoint = transform.position + Random.insideUnitSphere * scatterDistance;
                NavMeshHit hit;
                if(NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }
            return transform.position;
        }
    }
}
