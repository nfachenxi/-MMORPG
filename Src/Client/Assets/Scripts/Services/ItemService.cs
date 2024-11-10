using Common;
using Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemService : Singleton<ItemService> ,IDisposable
{
    public ItemService()
    {
        MessageDistributer.Instance.Subscribe<BuyShopItemResponse>(this.OnBuyItem);
    }

    public void Dispose()
    {
        MessageDistributer.Instance.Unsubscribe<BuyShopItemResponse>(this.OnBuyItem);
    }

    public void SendBuyItem(int shopID, int ShopItemID)
    {
        Log.Info("SendBuyItem");
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.buyShopItem = new BuyShopItemRequest();
        message.Request.buyShopItem.shopId = shopID;
        message.Request.buyShopItem.itemId = ShopItemID;
        NetClient.Instance.SendMessage(message);
    }

    private void OnBuyItem(object sender, BuyShopItemResponse response)
    {
        MessageBox.Show("购买结果：" + response.Result + "\n" + response.Errormsg, "购买完成");
    }

    


}
