using System.Collections;
using System.Collections.Generic;
using RPG.Dialogue;
using RPG.Shops;
using RPG.Stats;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class MinimapInfo : MonoBehaviour
    {
        [SerializeField] GameObject character;
        [SerializeField] TextMeshProUGUI info;
        
        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            BaseStats baseStats = character.GetComponent<BaseStats>();   
            Shop shop = character.GetComponent<Shop>();
            AIConversant aiConversant = character.GetComponent<AIConversant>();
            if (aiConversant != null)
            {
                info.text = "Quest";
                return;
            }

            if (shop != null)
            {
                info.text ="Shop";
                return;
            }

            info.text = baseStats.GetCharacterClass().ToString();
        }
    }
}