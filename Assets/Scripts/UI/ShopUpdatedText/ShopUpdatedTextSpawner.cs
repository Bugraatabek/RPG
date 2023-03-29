using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class ShopUpdatedTextSpawner : MonoBehaviour
    {
        [SerializeField] ShopUpdatedText shopTextPrefab;
            
        public void SpawnText()
        {  
            ShopUpdatedText instance = Instantiate<ShopUpdatedText>(shopTextPrefab, transform);
            instance.ShoptoText();
        }
    }
}