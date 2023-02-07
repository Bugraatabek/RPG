using RPG.Inventories;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Shops
{
    public class ShopItem
    {
        InventoryItem item;
        int availability;
        float price;
        int quantityInTransaction;

        // CONSTRUCTOR //
        public ShopItem(InventoryItem item, int availability, float price, int quantityInTransaction)
        {
            this.item = item;
            this.availability = availability;
            this.price = price;
            this.quantityInTransaction = quantityInTransaction;
        }
        // CONSTRUCTOR //

        // GETTERS //
        public string GetName()
        {
            return item.GetDisplayName();
        }

        public Sprite GetIcon()
        {
            return item.GetIcon();
        }

        public int GetAvailability()
        {
            return availability;
        }

        public float GetPrice()
        {
            return price;
        }

        public int GetQuantityInTransaction()
        {
            return quantityInTransaction;
        }

        public InventoryItem GetInventoryItem()
        {
            return item;
        }
        // GETTERS //
    }
}