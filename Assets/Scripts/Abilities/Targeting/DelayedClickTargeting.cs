using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Control;
using RPG.Core;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Delayed Click Targeting", menuName = "Abilities/Targeting/DelayedClick", order = 0)]
    public class DelayedClickTargeting : TargetingStrategy
    {
        [SerializeField] Texture2D cursorTexture;
        [SerializeField] Vector2 cursorHotspot;
        [SerializeField] LayerMask layerMask;
        [SerializeField] float areaAffectRadius; 
        [SerializeField] GameObject targetCircle;
        GameObject circleInstance = null;

        public override void StartTargeting(AbilityData data, Action finished)
        {
            PlayerController playerController = data.GetUser().GetComponent<PlayerController>();
            data.StartCoroutine(Targeting(data, playerController, finished));
        }

        private IEnumerator Targeting(AbilityData data, PlayerController playerController, Action finished)
        {
            playerController.enabled = false;
            if(circleInstance == null) 
            { 
                circleInstance = GameObject.Instantiate(targetCircle);
            }
            else
            {
                circleInstance.SetActive(true);
            }
            circleInstance.transform.localScale = new Vector3(areaAffectRadius*2 ,1,areaAffectRadius*2);
            
            while(true)
            {
                Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
                RaycastHit raycastHit;

                if (Physics.Raycast(PlayerController.GetMouseRay(),out raycastHit, 1000, layerMask))
                {
                    circleInstance.transform.position = raycastHit.point;

                    if(data.GetUser().GetComponent<Health>().IsDead()) 
                    {
                        circleInstance.SetActive(false);
                        yield break;
                    }
                    
                    if(Input.GetMouseButton(1))
                    {
                        circleInstance.SetActive(false);
                        playerController.enabled = true;
                        yield break;
                    }

                    if(Input.GetMouseButton(0))
                    {
                        // Absorb the whole mouse click, use ability when released.
                        yield return new WaitWhile(() => Input.GetMouseButton(0));
                        ActionScheduler actionScheduler = data.GetUser().GetComponent<ActionScheduler>();
                        actionScheduler.StartAction(data);
                        circleInstance.SetActive(false);
                        playerController.enabled = true;
                        data.SetTargetedPoint(raycastHit.point);
                        data.SetTargets(GetGameObjectsInRadius(raycastHit.point));
                        finished();
                        yield break; 
                    }
                }
                yield return null;
            }
        }

        private IEnumerable<GameObject> GetGameObjectsInRadius(Vector3 point)
        {
            RaycastHit[] hits = Physics.SphereCastAll(point, areaAffectRadius, Vector3.up, 0);
            foreach (var hit in hits)
            {
                yield return hit.collider.gameObject;
            }
            
        }

        
    }
}