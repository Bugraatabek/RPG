using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Control;
using UnityEngine;
using UnityEngine.AI;

    namespace RPG.Globes
    {
        public class Globe : MonoBehaviour, IRaycastable
        {
            [SerializeField] float amount = 0;
            [SerializeField] [Range(1,100)] float percantage = 1;
            
            [Tooltip("While this is true Gain By Amount/Amount will not work")]
            [SerializeField] bool gainByPercantage = true;
            [SerializeField] bool gainByAmount = false;

            GameObject player;
            NavMeshAgent navMeshAgent;


            public CursorType GetCursorType()
            {
                return CursorType.Pickup;
            }

            public bool HandleRaycast(PlayerController callingController)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    navMeshAgent = callingController.GetComponent<NavMeshAgent>(); 
                    navMeshAgent.destination = transform.position;
                }
                return true;
                //return player.GetComponent<PlayerController>().InteractWithMovement();
            }

            private void OnTriggerEnter(Collider other) 
            {
                player = GameObject.FindWithTag("Player");
                if(other.tag == player.tag)
                {
                    var maxAmountFraction = MaxAmountFraction();
                    
                    if(gainByPercantage == true)
                    {
                        gainByAmount = false;
                        amount = percantage * maxAmountFraction;
                        GainStat(amount);
                    }

                    if(gainByAmount == true)
                    {
                        GainStat(amount);
                    }

                    Destroy(gameObject);  
                }
            }

            protected virtual float MaxAmountFraction(){ return 0;}
            protected virtual void GainStat(float amount){}
        }

    }



