using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class ShopUpdatedText : MonoBehaviour
    {
        [SerializeField] Text shopUpdatedText = null;
        public void ShoptoText()
        {
            shopUpdatedText.text = "Shops Updated";
        }
    }
}
