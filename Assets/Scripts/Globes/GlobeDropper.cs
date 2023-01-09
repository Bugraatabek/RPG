using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Globes
{
    public class GlobeDropper : MonoBehaviour
    {
        [Tooltip("How far can the pickups be scattered from the dropper.")]
        [SerializeField] float scatterDistance = 2f;
        
        [Range(0,100)] public int dropChance = 20;
        [SerializeField] GlobeDropLibrary globeDropLibrary = null;

        const int ATTEMPTS = 30;

        public void DropGlobes()
        {
            var globesToDrop = globeDropLibrary.GlobesToDrop(dropChance);
            foreach (var drop in globesToDrop)
            {
                var globe = GameObject.Instantiate(drop.globe);
                globe.transform.position = DropLocation();
            }
        }

        private Vector3 DropLocation()
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
