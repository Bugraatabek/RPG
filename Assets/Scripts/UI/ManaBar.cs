using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;

namespace RPG.UI
{
    public class ManaBar : MonoBehaviour
    {
        [SerializeField] Canvas rootCanvas = null;
        [SerializeField] Mana manaComponent = null;
        [SerializeField] Health healthComponent = null;
        [SerializeField] RectTransform foreground = null;
        
         
        private void Update() 
        {
            float manaFraction = manaComponent.GetFraction();
            foreground.localScale = new Vector3(manaFraction, 1, 1);
            
            if(manaComponent.GetFraction() == 1)
            {
                rootCanvas.enabled = false;
                return;
            }
            if(healthComponent.GetFraction() <= 0)
            {
                rootCanvas.enabled = false;
            }
            else if(healthComponent.GetFraction() > 0)
            {
                rootCanvas.enabled = true;
            }
            
        }
    }
}