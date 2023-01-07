using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageTextPrefab;
            
        public void SpawnText(float damage)
        {  
            DamageText instance = Instantiate<DamageText>(damageTextPrefab, transform);
            instance.DamageToText(damage);
        }
    }
}
