using System;
using RPG.Attributes;
using RPG.Combat;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Spawn Projectile Effect", menuName = "Abilities/Effects/Spawn Projectile", order = 0)]
    public class SpawnProjectileEffect : EffectStrategy
    {
        [SerializeField] Projectile projectileToSpawn; 
        [SerializeField] float damage;
        [SerializeField] bool isRightHand = true;
        [SerializeField] bool useTargetPoint = true;

        public override void StartEffect(AbilityData data, Action finished)
        {
            Vector3 spawnPosition = data.GetUser().GetComponent<Fighter>().GetHandTransform(isRightHand).position;
            if(useTargetPoint)
            {
                SpawnProjectilesForTargetPoint(data, spawnPosition);
            }
            else
            {
                SpawnProjectilesForTargets(data, spawnPosition);
            }
            finished();
        }

        private void SpawnProjectilesForTargetPoint(AbilityData data, Vector3 spawnPosition)
        {
            Projectile projectile = Instantiate(projectileToSpawn);
            projectile.transform.position = spawnPosition;
            projectile.SetTarget(data.GetTargetedPoint(), data.GetUser(), damage);
        }

        private void SpawnProjectilesForTargets(AbilityData data, Vector3 spawnPosition)
        {
            foreach (GameObject target in data.GetTargets())
            {
                Health targetHealth = target.GetComponent<Health>();
                if (targetHealth != null)
                {
                    Projectile projectile = Instantiate(projectileToSpawn);
                    projectile.transform.position = spawnPosition;
                    projectile.SetTarget(targetHealth, data.GetUser(), damage);
                }
            }
        }
    }
}