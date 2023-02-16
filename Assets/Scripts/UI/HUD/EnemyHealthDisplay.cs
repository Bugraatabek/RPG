using UnityEngine;
using UnityEngine.UI;
using RPG.Attributes;
using RPG.Combat;
using System;

namespace RPG.UI.HUD
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Health target;

        private void Update() 
        {
            target = GameObject.FindWithTag("Player").GetComponent<Fighter>().GetTarget();
            // GetComponent<Text>().text = String.Format("{0:0}", health.GetPercentage()); without Decimals // 0:0.0 with decimals
            if(target != null)
            {
                GetComponent<Text>().text = String.Format("{0:0}/{1:0}", target.GetCurrentHealth(), target.GetMaxHealth());
            }
            else
            {
                GetComponent<Text>().text = "N/A";
            }
        }
    }
}
