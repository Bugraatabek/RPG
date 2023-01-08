using RPG.Attributes;
using UnityEngine;
using RPG.Inventories;
using RPG.Stats;
using System.Collections.Generic;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/MakeNewWeapon", order = 0)]
    public class WeaponConfig : StatsEquipableItem
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] Weapon equippedPrefab = null;
        //[SerializeField] float weaponDamage = 5f;
        [SerializeField] float weaponRange = 2f;
        //[SerializeField] float weaponPercentageBonus = 20f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;
        const string weaponName = "Weapon";

        public Weapon Spawn(Transform rightHand,Transform leftHand, Animator animator)
        {
            Weapon weapon = null;
            DestroyOldWeapon(rightHand, leftHand);
            if(equippedPrefab != null)
            {
                Transform handTransform = GetHandTransform(rightHand, leftHand);
                weapon = Instantiate(equippedPrefab, handTransform);
                weapon.gameObject.name = weaponName;
            }
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if(overrideController != null)
            {
                
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController; 
            }
            return weapon;
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if(oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if(oldWeapon == null) return;
            
            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetHandTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHanded) { handTransform = rightHand; }
            else handTransform = leftHand;
            return handTransform;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }
        
        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(projectile, GetHandTransform(rightHand,leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
        }

        public float GetRange()
        {
            return weaponRange;
        }
        
        // public float GetDamage()
        // {
        //     return weaponDamage;
        // }

        // public float GetPercentageBonus()
        // {
        //     return weaponPercentageBonus;
        // }

        // public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        // {
        //     if(stat == Stat.Damage)
        //     {
        //         yield return weaponDamage;
        //     }
        // }

        // public IEnumerable<float> GetPercentageModifiers(Stat stat)
        // {
        //     if(stat == Stat.Damage)
        //     {
        //         yield return weaponPercentageBonus;
        //     }
        // }
    }   
}
