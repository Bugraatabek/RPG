using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float maxLifeTime = 1f;
        [SerializeField] float projectileSpeed = 2f;
        [SerializeField] float lifeAfterImpact = 2f;
        
        [SerializeField] bool isHoming = true;

        [SerializeField] Health target = null;
        Vector3 targetPoint;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] GameObject hitEffect = null;
        GameObject instigator = null;
        float damage = 0;

        [SerializeField] UnityEvent onHit;
        
        void Start()
        {
            transform.LookAt(GetAimLocation());
        }
        void Update()
        {
            if(target != null && isHoming && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * Time.deltaTime * projectileSpeed);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            SetTarget(instigator, damage, target);
        }

        public void SetTarget(Vector3 targetPoint, GameObject instigator, float damage)
        {
            SetTarget(instigator, damage, null, targetPoint);
        }

        public void SetTarget(GameObject instigator, float damage, Health target = null, Vector3 targetPoint = default)
        {
            this.target = target;
            this.targetPoint = targetPoint;
            this.damage = damage;
            this.instigator = instigator;

            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            if(target == null)
            {
                return targetPoint;
            }

            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if(targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height/2;
        }

        private void OnTriggerEnter(Collider other) 
        {
                Health health = other.gameObject.GetComponent<Health>();
                if(target != null && health != target) return;
                if(health == null || health.IsDead()) return;
                if(other.gameObject == instigator) return;
    
                onHit.Invoke();
                health.TakeDamage(instigator, damage);
                projectileSpeed = 0;
                
                if(hitEffect != null)
                {
                    GameObject fX = Instantiate(hitEffect, GetAimLocation(), transform.rotation);
                    Destroy(fX, 1f);
                }
                
                foreach(GameObject toDestroy in destroyOnHit)
                {
                    Destroy(toDestroy);
                } 
                Destroy(gameObject, lifeAfterImpact);     
        }
    }
}
