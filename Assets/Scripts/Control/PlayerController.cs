using UnityEngine;
using RPG.Movement;
using RPG.Attributes;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using UnityEngine.AI;
using RPG.Inventories;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }
        [SerializeField] float maxNavMeshProjectionDistance = 1f;
        
        [SerializeField] CursorMapping[] cursorMappings;
        Dictionary<CursorType, CursorMapping> cursorDict;

        [SerializeField] int numberOfAbilities = 6;
        ActionStore actionStore;

        bool isDragging = false;
        
        private void Awake() 
        {
            actionStore = GetComponent<ActionStore>();
        }
        
        private void Start() 
        {
            BuildCursorMappingDict();
        }
        
        private void Update()
        {
            if(InteractWithUI()) return;
            
            if (GetComponent<Health>().IsDead()) 
            {
                SetCursor(CursorType.None);
                return;
            }

            UseAbilities();

            if(InteractWithComponent()) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
            
            SetCursor(CursorType.None);
        }

        
        private bool InteractWithUI()
        {
            if(Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
            if(EventSystem.current.IsPointerOverGameObject()) //IsPointerOverGameObject() is saying if its over a ui object 
            {
                SetCursor(CursorType.UI);
                if(Input.GetMouseButtonDown(0))
                {
                    isDragging = true;
                }
                return true;
            }

            if(isDragging == true) return true;
            return false;
        }

        private void UseAbilities()
        {
            for (int i = 0; i < numberOfAbilities; i++)
            {
                if(Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    actionStore.Use(i, gameObject);
                }
            }
            
        }


        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach(RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach(IRaycastable raycastable in raycastables)
                {
                    if(raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        RaycastHit[] RaycastAllSorted() // Calculates the ray distances and sorts them.
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), 0.5f); // if want a cursor with bigger radius just use SphereCastAll(GetMouseRay(), radius) or Raycastall(GetMouseRay()) for single ray
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;

        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach(RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if(raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }  
            }
            return false;
        }

    
        private bool InteractWithMovement()
        {
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            if (hasHit == true)
            {
                if(!GetComponent<Mover>().CanMoveTo(target)) return false;
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(target, 1f);
                }
                SetCursor(CursorType.Movement);
                return true; // so when we hover around or click if hashit == true then bool returns true
            }
            return false;
        }

        private bool RaycastNavMesh(out Vector3 target) // watch this lesson again 159,160
        {
            target = new Vector3();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if(!hasHit) return false;
           
            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point,out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if(!hasCastToNavMesh) return false;

            target = navMeshHit.position;
        
            return true;
            
        }

        
        private void SetCursor(CursorType type)
        {
            Cursor.SetCursor(cursorDict[type].texture, cursorDict[type].hotspot, CursorMode.Auto);
        }


        private void BuildCursorMappingDict()
        {
            cursorDict = new Dictionary<CursorType, CursorMapping>();
            foreach (CursorMapping cursorMapping in cursorMappings)
            {
                cursorDict[cursorMapping.type] = cursorMapping;
            } 
        }


        public static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
            
        }
    } 
}

