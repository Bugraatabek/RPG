using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class PurseUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI balance;

        Purse playerPurse = null;

        private void Start() 
        {
            playerPurse = GameObject.FindWithTag("Player").GetComponent<Purse>();
            RefreshUI();
            if(playerPurse != null)
            {
                playerPurse.balanceChanged += RefreshUI;  
            }  
        }

        private void RefreshUI()
        {
            balance.text = $"{playerPurse.GetBalance():N2}";
        }
    }
}
