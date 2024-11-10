using Common;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class ItemService : Singleton<ItemService>
    {
        public ItemService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<BuyShopItemRequest>(this.OnBuyItem);
        }

        public void Init()
        {

        }

        void OnBuyItem(NetConnection<NetSession> sender, BuyShopItemRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnBuyItem : : character : {0} : Shop : {1} ShopItem : {2}", character.Id, request.shopId, request.itemId);
            var result = ShopManager.Instance.BuyItem(sender, request.shopId, request.itemId);
            sender.Session.Response.buyShopItem = new BuyShopItemResponse();
            sender.Session.Response.buyShopItem.Result = result;
            sender.SendResponse();


        }
        

        


    }
}
