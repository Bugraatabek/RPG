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
        [SerializeField] GameObject[] destronOnHit = null;
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
            if(target == null) return;
            if(isHoming && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * Time.deltaTime * projectileSpeed);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if(targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height/2;
        }

        private void OnTriggerEnter(Collider other) 
        {   
                if(other.gameObject.GetComponent<Health>() != target) return;
                
                onHit.Invoke();
                if(target.IsDead())
                {
                    Destroy(gameObject, maxLifeTime);
                    return;
                } 
        
                target.TakeDamage(instigator, damage);
                projectileSpeed = 0;
                
                if(hitEffect != null)
                {
                    GameObject fX = Instantiate(hitEffect, GetAimLocation(), transform.rotation);
                    Destroy(fX, 1f);
                }
                
                foreach(GameObject toDestroy in destronOnHit)
                {
                    Destroy(toDestroy);
                } 
                Destroy(gameObject, lifeAfterImpact);     
        }
    }
}
