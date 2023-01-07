using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;

namespace RPG.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Canvas rootCanvas = null;
        [SerializeField] Health healthComponent = null;
        [SerializeField] RectTransform foreground = null;
        
         
        private void Update() 
        {
            float healthFraction = healthComponent.GetFraction();
            foreground.localScale = new Vector3(healthFraction, 1, 1);
            
            if(healthComponent.GetFraction() == 1)
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
