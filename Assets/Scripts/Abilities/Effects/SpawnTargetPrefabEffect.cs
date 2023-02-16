using System;
using System.Collections;
using RPG.Control;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Spawn Target Prefab Effect", menuName = "Abilities/Effects/Spawn Target Prefab", order = 0)]
    public class SpawnTargetPrefabEffect : EffectStrategy
    {
        [SerializeField] GameObject fxGameObject;
        [SerializeField] float destroyDelay = -1;
        [SerializeField] bool instantiateOnUser = false;
        GameObject fxInstance;
        public override void StartEffect(AbilityData data, Action finished)
        {
            data.StartCoroutine(Effect(data, finished));
        }

        private IEnumerator Effect(AbilityData data, Action finished)
        {
            if(fxInstance == null) 
            {
                if(instantiateOnUser == true) 
                {
                    fxInstance = GameObject.Instantiate(fxGameObject, data.GetUser().transform);
                }
                else
                {
                    fxInstance = GameObject.Instantiate(fxGameObject);
                } 
                
                
            }
            fxInstance.SetActive(true);
            fxInstance.transform.position = data.GetTargetedPoint();
            ParticleSystem hitFX = fxInstance.GetComponent<ParticleSystem>();
            hitFX.Play();
            
            if(destroyDelay > 0)
            {
                yield return new WaitForSeconds(destroyDelay);
                hitFX.Stop();
                fxInstance.SetActive(false);
            }
            finished(); 
        }

    }
}