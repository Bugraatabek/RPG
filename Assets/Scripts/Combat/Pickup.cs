using System.Collections;
using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public class Pickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfig weaponPickup = null;
        [SerializeField] float respawnTime = 5f;
        [SerializeField] float healAmount = 20;
        [SerializeField] bool isHeal = false;
        
        
        private void OnTriggerEnter(Collider other) 
        {
            if(other.gameObject.tag == "Player")
            {
                PickupSubject(other.gameObject);
            }
        }

        private void PickupSubject(GameObject subject)
        {
            if(weaponPickup != null)
            {
                subject.GetComponent<Fighter>().EquipWeapon(weaponPickup);
            }
            if(isHeal == true)
            {
                subject.GetComponent<Health>().Heal(healAmount);
            }
            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool show)
        {
            int childCount = transform.childCount;
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(show);
            }
            Collider collider = GetComponent<Collider>();
            collider.enabled = show;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if(Input.GetMouseButtonDown(0))
            {
                PickupSubject(callingController.gameObject);
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}
