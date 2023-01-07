using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] Text damageText = null;
        public void DamageToText(float damage)
        {
            damageText.text = damage.ToString();
        }
    }
}
