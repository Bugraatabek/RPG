using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using UnityEngine;
using UnityEngine.UI;
using RPG.Shops;

namespace RPG.UI.Shops
{
    public class FilterButtonUI : MonoBehaviour
    {
        
        [SerializeField] ItemCategory category = ItemCategory.None;

        Button button;
        Shop currentShop;

        private void Awake() 
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(SelectFilter);  
        }

        public void SetShop(Shop currentShop)
        {
            this.currentShop = currentShop;
            if(currentShop != null)
            {
                currentShop.onChange += RefreshButton;
                RefreshButton();
            }
        }

        public void RefreshButton()
        {
            button.interactable = currentShop.GetFilter() != category;
        }

        private void SelectFilter()
        {
            currentShop.SelectFilter(category);
        }
    }
}

