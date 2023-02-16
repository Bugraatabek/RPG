using System;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.HUD
{
    public class ManaDisplay : MonoBehaviour 
    {
        Mana mana;
        private void Awake() 
        {
            mana = GameObject.FindWithTag("Player").GetComponent<Mana>();
        }
        private void Update() 
        {
            GetComponent<Text>().text = String.Format("{0:0}/{1:0}", mana.GetCurrentMana(), mana.GetMaxMana());
        }
    }
}
