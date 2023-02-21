using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using RPG.Attributes;
using RPG.Utils;
using UnityEngine.AI;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 4f;
        [SerializeField] float aggroCooldown = 4f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waitTimeAtWaypoint = 2f;
        [SerializeField] float shoutDistance = 5f;
        
        [SerializeField] bool hasBeenAggroedRecently = false;

        
        [Range(0,1)] [SerializeField] float patrolSpeedFraction = 0.2f;
        
        float timeSpentAtWaypoint = Mathf.Infinity;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceAggrevated = Mathf.Infinity;
        int currentWaypointIndex = 0;

        
        Fighter fighter;
        Health health;
        GameObject player;
        Mover mover;

        LazyValue<Vector3> guardPosition;
        
        
        private void Awake() 
        {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player"); 
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
            guardPosition.ForceInit();
        }
        
        private void Update()
        {
            if (health.IsDead()) return;

            if (IsAggravated() && fighter.CanAttack(player))
            {
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
            UpdateTimers();

        }

        
        public Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition.value;
            if (patrolPath != null)
            {   
                if(AtWaypoint())
                {
                    timeSpentAtWaypoint = 0f;
                    CycleWaypoint();
                }  
                nextPosition = GetCurrentWaypoint();
            }
            if (timeSpentAtWaypoint > waitTimeAtWaypoint)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }  
        }


        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }


        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }


        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }


        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);

            AggrevateNearbyEnemies();
        }


        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
            hasBeenAggroedRecently = false;
        }

        public void Aggrevate()
        {
           timeSinceAggrevated = 0;
        }

        public void AggroAllies()
        {
            if(hasBeenAggroedRecently == true) return;
            if(hasBeenAggroedRecently == false)
            {
                print("I'll join the fight!");
                timeSinceAggrevated = 0f;
                timeSinceLastSawPlayer = 0f;
                hasBeenAggroedRecently = true;
            }
        }

        private void AggrevateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                AIController ai = hit.transform.GetComponent<AIController>();
                if(ai == null) continue;
                {
                    ai.AggroAllies();
                }        
            }
        }

        private bool IsAggravated()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer <= chaseDistance || timeSinceAggrevated < aggroCooldown;
        }



        private void OnDrawGizmosSelected() 
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

        private void UpdateTimers()
        {
            timeSinceAggrevated += Time.deltaTime;
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSpentAtWaypoint += Time.deltaTime;
        }

        public void Reset()
        {
            if(health.IsDead()) return;
            GetComponent<NavMeshAgent>().Warp(guardPosition.value);
            hasBeenAggroedRecently = false;
            timeSpentAtWaypoint = Mathf.Infinity;
            timeSinceLastSawPlayer = Mathf.Infinity;
            timeSinceAggrevated = Mathf.Infinity;
            currentWaypointIndex = 0;
            
        }
    }
}

