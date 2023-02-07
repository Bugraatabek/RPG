using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using RPG.Inventories;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Shops
{
    public class Shop : MonoBehaviour, IRaycastable, ISaveable
    {
        [SerializeField] string shopName = "";
        [SerializeField] StockItemConfig[] stockConfig;
        [Tooltip("buyingPrice - (buyingPrice * sellingDiscount/100)")]
        [SerializeField] float sellingDiscountPercentage = 10;

        [System.Serializable]
        class StockItemConfig
        {
            public InventoryItem item;
            public int initialStock;
            [Range(0,100)] public float buyingDiscountPercentage;
            public int levelToUnlock = 0;
        }

        Dictionary<InventoryItem, int> transaction = new Dictionary<InventoryItem, int>();
        Dictionary<InventoryItem, int> stockSold = new Dictionary<InventoryItem, int>();
        Shopper currentShopper = null;
        bool isBuyingMode = true;
        ItemCategory currentFilter = ItemCategory.None;
        
        public event Action onChange;

        

        // SETTERS //

        private void Awake() 
        {
            foreach (StockItemConfig config in stockConfig)
            {
                stockSold[config.item] = 0;
            }    
        }

        public void SetShopper(Shopper shopper)
        {
            this.currentShopper = shopper;
        }

        public void SelectFilter(ItemCategory category) 
        {
            currentFilter = category;
            if(onChange != null)
            {
                onChange();
            }
        }
        
        public void ConfirmTransaction() 
        {
            Purse shopperPurse = currentShopper.GetComponent<Purse>();
            Inventory shopperInventory = currentShopper.GetComponent<Inventory>();
            if(shopperInventory == null || shopperPurse == null) return;
            
            // Transfer to or from the inventory
            foreach (ShopItem shopItem in GetAllItems())
            {
                InventoryItem item = shopItem.GetInventoryItem();
                int quantity = shopItem.GetQuantityInTransaction();
                
                float price = shopItem.GetPrice();
   
                for (int i = 0; i < quantity; i++)
                {
                    if(IsBuyingMode())
                    {
                        BuyItem(shopperPurse, shopperInventory, item, price);
                    }
                    else
                    {
                        SellItem(shopperPurse, shopperInventory, item, price);
                    }

                }
            }
        }

        private void BuyItem(Purse shopperPurse, Inventory shopperInventory, InventoryItem item, float price)
        {
            var availabilities = GetAvailabilities();

            bool notEnoughCoins = shopperPurse.GetBalance() < price;
            bool noStockLeft = availabilities[item] <= 0;

            if(notEnoughCoins) return;
            if(noStockLeft) return;

            bool success = shopperInventory.AddToFirstEmptySlot(item, 1);
            if (success)
            {
                ChangeTransactionQuantity(item, -1);
                shopperPurse.UpdateBalance(-price);
                ChangeStock(item, 1);
            }
        }

        private void SellItem(Purse shopperPurse, Inventory shopperInventory, InventoryItem item, float price)
        {
            int slot = FindItemInSlot(shopperInventory, item);
            if(slot == -1) return;
            
            shopperInventory.RemoveFromSlot(slot, 1);
            ChangeTransactionQuantity(item, -1);
            ChangeStock(item, -1);
            shopperPurse.UpdateBalance(price);
        }

        
        public void ChangeTransactionQuantity(InventoryItem item, int quantity) 
        {
            
            if(!transaction.ContainsKey(item))
            {
                transaction[item] = 0;
            }
            
            var availabilities = GetAvailabilities();
            if(transaction[item] + quantity <= availabilities[item])
            {
                transaction[item] += quantity;
            }
            
            if(transaction[item] <= 0)
            {
                transaction.Remove(item);
            }
            
            if(onChange != null)
            {
                onChange();
            }
        }

        public void ChangeStock(InventoryItem item, int changedStock)
        {
            stockSold[item] += changedStock;
            
            if(onChange != null)
            {
                onChange();
            }
        }

        public void SelectMode(bool isBuying) 
        {
            isBuyingMode = isBuying;
            if(onChange != null)
            {
                onChange();
            }
        }

        // SETTERS //
        
        
        // GETTERS //
            // Public Getters //
        
        public IEnumerable<ShopItem> GetFilteredItems() 
        {
            foreach (var shopItem in GetAllItems())
            {
                var item = shopItem.GetInventoryItem();
                if(currentFilter == item.GetItemCategory() || currentFilter == ItemCategory.None)
                {
                    yield return shopItem;
                }
            } 
        }

        public IEnumerable<ShopItem> GetAllItems()
        {
            int shopperLevel = GetShopperLevel();

            Dictionary<InventoryItem, float> prices = GetPrices();
            Dictionary<InventoryItem, int> availabilities = GetAvailabilities();

            foreach (InventoryItem item in availabilities.Keys)
            {
                if(availabilities[item] <= 0) continue;
                
                int quantityInTransaction = 0;
                transaction.TryGetValue(item, out quantityInTransaction);
                
                float shopPrice = prices[item];
                int availability = availabilities[item];
                yield return new ShopItem(item, availability, shopPrice, quantityInTransaction);
            }
        }

        private Dictionary<InventoryItem, int> GetAvailabilities()
        {
            Dictionary<InventoryItem, int> availabilities = new Dictionary<InventoryItem, int>();
            foreach (StockItemConfig config in GetAvailableConfigs())
            {
                if(!availabilities.ContainsKey(config.item))
                {
                    int sold = 0;
                    stockSold.TryGetValue(config.item, out sold);
                    availabilities[config.item] = -sold;
                }
                availabilities[config.item] += config.initialStock;

                if(!IsBuyingMode())
                {
                    print(CountItemsInInventory(config.item));
                    availabilities[config.item] = CountItemsInInventory(config.item);
                }
            }
            return availabilities;
        }

        private Dictionary<InventoryItem, float> GetPrices()
        {
            Dictionary<InventoryItem, float> prices = new Dictionary<InventoryItem, float>();
            foreach (StockItemConfig config in GetAvailableConfigs())
            {
                float defaultPrice = config.item.GetPrice();
                float shopPrice = defaultPrice - (defaultPrice * config.buyingDiscountPercentage / 100);
                
                if(!prices.ContainsKey(config.item))
                {
                    prices[config.item] = defaultPrice;
                }
                prices[config.item] = shopPrice;
                
                
                if(!IsBuyingMode())
                {
                    shopPrice = shopPrice - (shopPrice * sellingDiscountPercentage / 100);
                    prices[config.item] = shopPrice;
                }
            }
            return prices;
        }

        private IEnumerable<StockItemConfig> GetAvailableConfigs()
        {
            int shopperLevel = GetShopperLevel();
            foreach (var config in stockConfig)
            {
                if(config.levelToUnlock <= shopperLevel)
                {
                    yield return config;
                }
            }
        }

        public bool IsBuyingMode()
        {
            return isBuyingMode;
        }

        public bool CanTransact() 
        {
            if(IsTransanctionEmpty()) return false;
            if(!HasSufficentFunds()) return false; 
            if(!InventoryHasSpace()) return false;
            return true;
        }

        public bool HasSufficentFunds()
        {   
            if(IsBuyingMode())
            {
                Purse playerPurse = currentShopper.GetComponent<Purse>();
                if(playerPurse == null) return false;
            
                float balance = playerPurse.GetBalance();
                return balance >= TransactionTotal();
            }
            return true;
        }

        public float TransactionTotal() 
        {
            float total = 0;
            foreach (ShopItem shopItem in GetAllItems())
            {
                total += shopItem.GetPrice() * shopItem.GetQuantityInTransaction();
            }
            return total;
        }
        
        public string GetShopName()
        {
            return shopName;
        }

        public ItemCategory GetFilter() 
        {
            return currentFilter;
        }
            // Public Getters //

            // Private Getters //
        private int CountItemsInInventory(InventoryItem item)
        {
            Inventory shopperInventory = currentShopper.GetComponent<Inventory>();
            if(shopperInventory == null) return 0;
            int count = 0;
            for (int i = 0; i < shopperInventory.GetSize(); i++)
            {
                if(shopperInventory.GetItemInSlot(i) == item)
                {
                    count += shopperInventory.GetNumberInSlot(i);
                }
            }
            return count;
        }
                
        private bool IsTransanctionEmpty()
        {
            return transaction.Count <= 0;
        }

        private bool InventoryHasSpace()
        {
            if(IsBuyingMode())
            {
                Inventory shopperInventory = currentShopper.GetComponent<Inventory>();
                if(shopperInventory == null) return false;
                List<InventoryItem> flatItems = new List<InventoryItem>();
                foreach (ShopItem shopItem in GetAllItems())
                {
                    InventoryItem item = shopItem.GetInventoryItem();
                    if(shopItem.GetInventoryItem().IsStackable())
                    {
                        flatItems.Add(item);
                    }
                    else
                    {
                        int quantity = shopItem.GetQuantityInTransaction();
                        for (int i = 0; i < quantity; i++)
                        {
                            flatItems.Add(item);
                        }
                    } 
                }
                return shopperInventory.HasSpaceFor(flatItems); 
            }
            return true;
        }

        private int FindItemInSlot(Inventory shopperInventory, InventoryItem item)
        {
            for (int i = 0; i < shopperInventory.GetSize(); i++)
            {
                if(shopperInventory.GetItemInSlot(i) == item)
                {
                    return i;
                }  
            }
            return -1;
        }

        private int GetShopperLevel()
        {
            if(currentShopper != null)
            {
                BaseStats stats = currentShopper.GetComponent<BaseStats>();
                if(stats == null) return 0;
                print(stats.GetLevel());
                return stats.GetLevel();
            }
            return 0;
        }

            // Private Getters //
        // GETTERS //

        
        // IRaycastable //
        public CursorType GetCursorType()
        {
            return CursorType.Shop;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if(Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<Shopper>().SetActiveShop(this);
            }
            return true;
        }
        // IRaycastable //
        
        // ISaveable //
        [System.Serializable]
        private struct StockRecord
        {
            public string itemID;
            public int stockSold;
        }
        
        public object CaptureState()
        {
            //My Solution
            var stockData = new StockRecord[stockSold.Count];
            int i = 0;
            foreach (var item in stockSold.Keys)
            {
                stockData[i].itemID = item.GetItemID();
                stockData[i].stockSold = stockSold[item];
                i++;
            }
            return stockData;

            // OR
            // Dictionary<string, int> stockData = new Dictionary<string, int>();
            // foreach (var pair in stockSold)
            // {
            //      stockData[pair.Key.GetItemID()] = pair.Value;
            // }
        }

        public void RestoreState(object state)
        {
            var StockData = state as StockRecord[];
            stockSold = new Dictionary<InventoryItem, int>();
            for (int i = 0; i < StockData.Length; i++)
            {
                stockSold[InventoryItem.GetFromID(StockData[i].itemID)] = StockData[i].stockSold; 
            }
        }
        // ISaveable //

    }
}