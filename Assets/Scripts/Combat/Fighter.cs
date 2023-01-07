using UnityEngine;
using RPG.Core;
using RPG.Movement;
using System;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider 
    {
        
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform rightHand = null;
        [SerializeField] Transform leftHand = null;
        [SerializeField] WeaponConfig defaultWeapon = null;
        
        
        
        WeaponConfig equippedWeaponConfig = null;
        LazyValue<Weapon> equippedWeapon;

        [Range(0,1)] [SerializeField] float chaseSpeedFraction = 1f;
        
        Health target;
        Mover _mover;
        
        float timeSinceLastAttack = Mathf.Infinity;
        
        
        private void Awake() 
        {    
            _mover = GetComponent<Mover>();
            equippedWeaponConfig = defaultWeapon;
            equippedWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }
 

        private void Start() 
        {
            equippedWeapon.ForceInit();
        }
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if(target == null) return;
            
            if (target != null && !GetIsInRange(target.transform))
            {
                _mover.MoveTo(target.transform.position, chaseSpeedFraction);
            }
            else
            {
                if(target.IsDead() == true) return;
                _mover.Cancel();
                AttackBehaviour();
                            
            }
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);
        }
        
    
         public void EquipWeapon(WeaponConfig weapon)
        {
            equippedWeaponConfig = weapon;
            equippedWeapon.value = AttachWeapon(weapon);
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            
            Animator animator = GetComponent<Animator>();
            return weapon.Spawn(rightHand, leftHand, animator);
        
        }

        public Health GetTarget()
        {
            return target;
            
        }

        private void AttackBehaviour()
        {
           transform.LookAt(target.transform.position);
           if(timeSinceLastAttack >= timeBetweenAttacks)
           {
            //This will trigger the Hit() event.
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
            timeSinceLastAttack = 0;
           }
        }

        void Hit() // Animation Event
        {
            if(target == null) return;
            
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if(equippedWeapon.value != null)
            {
                equippedWeapon.value.OnHit();
            }

            if(equippedWeaponConfig.HasProjectile())
            {
                equippedWeaponConfig.LaunchProjectile(rightHand,leftHand, target, gameObject, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
            }
            
        }

        void Shoot() // Animation Event
        {
            Hit();
        }  

        private bool GetIsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < equippedWeaponConfig.GetRange();
        }

        public void Attack(GameObject combatTarget)
        { 
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public bool CanAttack(GameObject combatTarget)
        { 
            if(!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position) && !GetIsInRange(combatTarget.transform)) { return false; }
            if (combatTarget == null) {return false;}
            Health targetToTest = combatTarget.GetComponent<Health>();
            return !targetToTest.IsDead() && targetToTest != null;
            
        }
        
        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().SetTrigger("stopAttack");
            GetComponent<Animator>().ResetTrigger("attack");
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return equippedWeaponConfig.GetDamage();
            }
        }
        
        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if(stat == Stat.Damage)
            {
            yield return equippedWeaponConfig.GetPercentageBonus();
            }
        }

        public object CaptureState()
        {
            return equippedWeaponConfig.name;
        }

        public void RestoreState(object state)
        {   
            string weaponName = (string)state;
            WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }

        
    }
}