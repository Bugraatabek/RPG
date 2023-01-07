using RPG.Inventories;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class RunoverPickup : MonoBehaviour,IRaycastable
    {
        GameObject player;
        Pickup pickup;
        NavMeshAgent navMeshAgent;

        private void Awake() 
        {
            player = GameObject.FindWithTag("Player");
            pickup = GetComponent<Pickup>();
            navMeshAgent = player.GetComponent<NavMeshAgent>();
        }
        
        void OnTriggerEnter(Collider other) 
        {
            if(other.tag == player.tag)
            {
                pickup.PickupItem();
            }
        }

        public CursorType GetCursorType()
        {
            if (pickup.CanBePickedUp())
            {
                return CursorType.Pickup;
            }
            else
            {
                return CursorType.FullPickup;
            }
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if(Input.GetMouseButtonDown(0))
            {
                navMeshAgent.destination = transform.position;
            }
            return true;
            //return player.GetComponent<PlayerController>().InteractWithMovement();
        }
    }
}


