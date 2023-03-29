using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using TMPro;
using UnityEngine;

namespace RPG.UI.Inventories
{
    public class PickupInfo : MonoBehaviour
    {
        [SerializeField] Pickup pickup;
        [SerializeField] TextMeshProUGUI info;
        private void Start() 
        {
            int number = pickup.GetNumber();
            if(number <= 1)
            {
                info.text = pickup.GetItem().GetDisplayName();
                return;
            }
            
            info.text = pickup.GetNumber() + " " + pickup.GetItem().GetDisplayName();
        }
    
    }
}
