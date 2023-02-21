using System;
using System.Collections;
using Cinemachine;
using RPG.Attributes;
using RPG.Scenemanagement;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class Respawner : MonoBehaviour
    {
        [SerializeField] Transform respawnLocation;
        [SerializeField] float respawnDelay = 3;
        [SerializeField] float fadeInTime = 1;
        [SerializeField] float fadeOutTime = 2;
        [SerializeField] float respawnHealthPercentage = 100;
        [SerializeField] float enemyRespawnHealPercentage = 20;
        Health health;

        private void Awake()
        {
            health = GetComponent<Health>();
            health.die.AddListener(() => StartCoroutine(Respawn()));
        }

        private void Start() 
        {
            if(health.IsDead())
            {
                StartCoroutine(Respawn());
            }
        }

        private IEnumerator Respawn()
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();
            yield return new WaitForSeconds(respawnDelay);
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            RespawnPlayer();
            ResetEnemies();
            savingWrapper.Save();
            yield return fader.FadeIn(fadeInTime);
            yield break;
        }

        private void ResetEnemies()
        {
            foreach (AIController enemyController in FindObjectsOfType<AIController>())
            {
                enemyController.Reset();
                Health enemyHealth = enemyController.GetComponent<Health>();
                if(enemyHealth && !enemyHealth.IsDead())
                {
                    float enemyHealAmount = enemyHealth.GetMaxHealth() * enemyRespawnHealPercentage/100;
                    enemyHealth.Heal(enemyHealAmount);
                }
            }
        }

        private void RespawnPlayer()
        {
            Vector3 positionDelta = respawnLocation.position - transform.position;
            var healthToRespawn = health.GetMaxHealth() * (respawnHealthPercentage / 100);
            health.Heal(healthToRespawn);
            GetComponent<NavMeshAgent>().Warp(respawnLocation.position);
            ICinemachineCamera activeVirtualCamera = FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera;
            // safeguarding against if(used Lookahead Time on camera) //
            if(activeVirtualCamera.Follow == transform)
            {
                activeVirtualCamera.OnTargetObjectWarped(transform, positionDelta);
            }
        }
    }
}