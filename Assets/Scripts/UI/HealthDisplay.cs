using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RPG.Attributes;
using System;

namespace RPG.UI
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;
        private void Awake() 
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }
        private void Update() 
        {
            // GetComponent<Text>().text = String.Format("{0:0}", health.GetPercentage()); without Decimals // 0:0.0 with decimals
            GetComponent<Text>().text = String.Format("{0:0}/{1:0}", health.GetCurrentHealth(), health.GetMaxHealth());
        }
    }
}
