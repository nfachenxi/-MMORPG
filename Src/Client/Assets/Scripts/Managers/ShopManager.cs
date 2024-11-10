using Common.Data;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    class ShopManager : Singleton<ShopManager>
    {

        public void Init()
        {
            NPCManager.Instance.RegisterNPCEvent(NPCFunction.InvokeShop, OnOpenShop);
        }

        private bool OnOpenShop(NPCDefine npc)
        {
            this.ShowShop(npc.Param);
            return true;
        }

        public void ShowShop(int param)
        {
            ShopDefine shop;
            if(DataManager.Instance.Shops.TryGetValue(param, out shop))
            {
                UIShop uiShop = UIManager.Instance.Show<UIShop>();
                if(uiShop != null)
                {
                    uiShop.SetShop(shop);
                }
            }
        }

        public bool BuyItem(int shopID, int shopItemID)
        {
            ItemService.Instance.SendBuyItem(shopID,shopItemID);
            return true;
        }

    }
}

