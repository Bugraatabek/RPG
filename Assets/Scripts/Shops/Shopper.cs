using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Shops
{
    public class Shopper : MonoBehaviour
    {
        Shop currentShop = null;

        public event Action activeShopChanged;

        public void SetActiveShop(Shop shop) // this creates a circle dependency. shop to shopper, shopeper to shop.
        {
            if(currentShop != null)
            {
                currentShop.SetShopper(null);
            }

            currentShop = shop;
            
            if(currentShop != null)
            {
                currentShop.SetShopper(this);
            }

            
            if(activeShopChanged != null)
            {
                activeShopChanged();
            }
        }

        public Shop GetActiveShop()
        {
            return currentShop;
        }
    }
}
