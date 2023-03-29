using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using RPG.Inventories;
using RPG.Shops;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI shopName;
        [SerializeField] TextMeshProUGUI total;
        [SerializeField] Transform content;
        [SerializeField] RowUI row;
        [SerializeField] Button confirmButton;
        [SerializeField] Button switchButton;
        
        Shopper shopper = null;
        Shop currentShop = null;
        Color originalTotalTextColor;


        private void Start() 
        {
            originalTotalTextColor = total.color;
            shopper = GameObject.FindWithTag("Player").GetComponent<Shopper>();
            if(shopper == null) return;

            shopper.activeShopChanged += ShopChanged;
            
            confirmButton.onClick.AddListener(ConfirmTransaction);
            switchButton.onClick.AddListener(SwitchMode);
            

            ShopChanged();
 
        }

        private void ShopChanged()
        {
            if(currentShop != null)
            {
                currentShop.onChange -= RefreshUI;
            }
            currentShop = shopper.GetActiveShop();
            gameObject.SetActive(currentShop != null);

            foreach (FilterButtonUI button in GetComponentsInChildren<FilterButtonUI>())
            {
                button.SetShop(currentShop);
            }
            
            if(currentShop == null) return;
            currentShop.onChange += RefreshUI;
            shopName.text = currentShop.GetShopName();
            
            RefreshUI();
        }

        private void RefreshUI()
        {
            
            foreach (Transform row in content)
            {
                Destroy(row.gameObject);
            }

            foreach (ShopItem item in currentShop.GetFilteredItems())
            {
                if(item != null)
                { 
                    RowUI rowInstance = Instantiate(row, content);
                    rowInstance.SetupRowUI(item, currentShop);
                }
            }
            total.text = $"Total: ${currentShop.TransactionTotal():N2}";
            total.color = currentShop.HasSufficentFunds() ? originalTotalTextColor : Color.red; 
            confirmButton.interactable = currentShop.CanTransact();
            
            TextMeshProUGUI confirmText = confirmButton.GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI switchText = switchButton.GetComponentInChildren<TextMeshProUGUI>();
            if(currentShop.IsBuyingMode())
            {
                switchText.text = "Switch to Selling";
                confirmText.text = "Buy";
            }
            else
            {
                switchText.text = "Switch to Buying";
                confirmText.text = "Sell";
            }
        }

        public void ConfirmTransaction()
        {
            currentShop.ConfirmTransaction();
        }

        public void Close()
        {
            currentShop = null;
            shopper.SetActiveShop(null);
        }

        public void SwitchMode()
        {
            currentShop.SelectMode(!currentShop.IsBuyingMode());
        }   
    }
}