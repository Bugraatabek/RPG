using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    
    public class UISwitcher : MonoBehaviour
    {
        [SerializeField] GameObject entryPoint;

        private void Awake()
        {
            SwitchTo(entryPoint);
        }

        public void SwitchTo(GameObject toDisplay)
        {
            if(toDisplay.transform.parent != transform) return;

            foreach (Transform menu in transform)
            {
                if(menu.gameObject == toDisplay) continue;
                menu.gameObject.SetActive(false);
            }
            toDisplay.SetActive(true);
        }
    }
}
