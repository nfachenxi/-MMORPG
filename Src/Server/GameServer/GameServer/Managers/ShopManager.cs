using Common;
using Common.Data;
using GameServer.Services;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class ShopManager : Singleton<ShopManager>
    {
        public Result BuyItem(NetConnection<NetSession> sender , int shopID, int shopItemID)
        {
            if (!DataManager.Instance.Shops.ContainsKey(shopID))
                return Result.Failed;

            ShopItemDefine shopItem;
            if (DataManager.Instance.ShopItems[shopID].TryGetValue(shopItemID,out shopItem))
            {
                Log.InfoFormat("BuyItem : : character: {0} : Item : {1} Count : {2} Price : {3}",sender.Session.Character.Id, shopItem.ID, shopItem.Count, shopItem.Price);
                if(sender.Session.Character.Gold >= shopItem.Price)
                {
                    sender.Session.Character.ItemManager.AddItem(shopItem.ID, shopItem.Count);
                    sender.Session.Character.Gold -= shopItem.Price;
                    DBService.Instance.Save();
                    return Result.Success;
                }
            }
            return Result.Failed;
        }



    }
}
