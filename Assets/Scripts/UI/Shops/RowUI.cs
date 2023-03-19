using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using RPG.Shops;
using RPG.UI.Inventories;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
    public class RowUI : MonoBehaviour, IItemHolder
    {
        [SerializeField] Image icon = null;
        [SerializeField] TextMeshProUGUI itemName = null;
        [SerializeField] TextMeshProUGUI availability = null;
        [SerializeField] TextMeshProUGUI price = null;
        [SerializeField] TextMeshProUGUI quantity = null;

        ShopItem shopItem;
        Shop shop;

        // SETTERS //
        
        public void SetupRowUI(ShopItem shopItem, Shop shop)
        {
            this.shop = shop;
            this.shopItem = shopItem;

            icon.sprite = shopItem.GetIcon();
            itemName.text = shopItem.GetName();
            availability.text = $"{shopItem.GetAvailability()}";
            price.text = $"${shopItem.GetPrice():N2}";
            quantity.text = $"+ {shopItem.GetQuantityInTransaction()} -";
        }

        public void Add()
        {
            shop.ChangeTransactionQuantity(shopItem.GetInventoryItem(), 1);
        }

        public void Remove()
        {
            shop.ChangeTransactionQuantity(shopItem.GetInventoryItem(), -1);
        }

        public InventoryItem GetItem()
        {
            return shopItem.GetInventoryItem();
        }

        // SETTERS //
    }
}
