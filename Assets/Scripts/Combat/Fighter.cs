using UnityEngine;
using RPG.Core;
using RPG.Movement;
using System;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using RPG.Utils;
using RPG.Inventories;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform rightHand = null;
        [SerializeField] Transform leftHand = null;
        [SerializeField] WeaponConfig defaultWeapon = null;
        
        
        
        Equipment equipment;
        WeaponConfig equippedWeaponConfig = null;
        LazyValue<Weapon> equippedWeapon;

        [Range(0,1)] [SerializeField] float chaseSpeedFraction = 1f;
        
        Health target;
        Mover _mover;
        
        float timeSinceLastAttack = Mathf.Infinity;
        
        
        private void Awake() 
        {    
            equipment = GetComponent<Equipment>();
            _mover = GetComponent<Mover>();
            equippedWeaponConfig = defaultWeapon;
            equippedWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
            if(equipment)
            {
                equipment.equipmentUpdated += UpdateWeapon;
            }
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

        private void UpdateWeapon()
        {
            var currentWeapon = equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponConfig;
            if(currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
            else
            {
                EquipWeapon(currentWeapon);
            }
            
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

        public Transform GetHandTransform(bool isRightHand)
        {
            if(isRightHand) return rightHand;
            return leftHand;
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
            BaseStats targetBaseStats = target.GetComponent<BaseStats>();
            
            if(targetBaseStats != null)
            {
                float defence = targetBaseStats.GetStat(Stat.Defence);
                damage = damage / (1 + defence/damage);
            }
            

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
    }
}