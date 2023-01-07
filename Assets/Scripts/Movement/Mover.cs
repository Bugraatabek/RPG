using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        NavMeshAgent navMeshAgent;
        Health health;
        ActionScheduler actionScheduler;
        float maxMoveSpeed = 6f;

        [SerializeField] float maxNavPathLength = 40f;
        private void Awake() 
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        void Update()
        {
            navMeshAgent.enabled = !health.IsDead(); // because there are 2 booleans we can set them like this instead of if(health.Isdead()) {navMeshAgent.enabled}
            UpdateAnimator();
        }
        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            actionScheduler.StartAction(this);
            MoveTo(destination, speedFraction);
        }
        
        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxMoveSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if(!hasPath) return false;
            if(path.status != NavMeshPathStatus.PathComplete) return false;
            if(GetPathLength(path) > maxNavPathLength) return false;
            
            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0;
            if(path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i+1]);
            }
            return total;
        }
        public void Cancel()
        {
           navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity); // Tekrar et
            float speed =localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
            
        }
    }
}

        
        // saving as a dictionary
        // public object CaptureState()
        // {
        //     Dictionary<string, object> data = new Dictionary<string, object>();
        //     data["position"] = new SerializableVector3(transform.position);
        //     data["rotation"] = new SerializableVector3(transform.eulerAngles);
        //     return data;
        // }

        // public void RestoreState(object state)
        // {
        //     Dictionary<string, object> data = (Dictionary<string, object>)state;
        //     GetComponent<NavMeshAgent>().enabled = false;
        //     transform.position = ((SerializableVector3)data["position"]).ToVector();
        //     transform.eulerAngles = ((SerializableVector3)data["rotation"]).ToVector();
        //     GetComponent<NavMeshAgent>().enabled = true;
        // }

        
        
        
        // saving as a struct
        // [System.Serializable]
        // struct MoverSaveData
        // {
        //     public SerializableVector3 position;
        //     public SerializableVector3 rotation;
        // }
        // public object CaptureState()
        // {
        //     MoverSaveData data = new MoverSaveData();
        //     data.position = new SerializableVector3(transform.position);
        //     data.rotation = new SerializableVector3(transform.eulerAngles);
        //     return data;
        // }

        // public void RestoreState(object state)
        // {
        //     MoverSaveData data = (MoverSaveData)state;
        //     GetComponent<NavMeshAgent>().enabled = false;
        //     transform.position = data.position.ToVector();
        //     transform.eulerAngles = data.rotation.ToVector();
        //     GetComponent<NavMeshAgent>().enabled = true;
        // }
