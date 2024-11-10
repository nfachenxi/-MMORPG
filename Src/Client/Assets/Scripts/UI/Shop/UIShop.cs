using Common.Data;
using Managers;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : UIWindow {


    public Text title;
	public Text money;

	public GameObject uiShopItem;

	private ShopDefine shop;
	public Transform[] itemRoots;

	void Start () {
		StartCoroutine(InitItems());
	}

	IEnumerator InitItems()
	{
		foreach(var kv in DataManager.Instance.ShopItems[shop.ID])
		{
			if(kv.Value.Status > 0)
			{
				GameObject go = Instantiate(uiShopItem, itemRoots[0]);
				UIShopItem shopItem = go.GetComponent<UIShopItem>();
				shopItem.SetShopItem(kv.Key, kv.Value, this);
			}
		}
		yield return null;
    }
	
	public void SetShop(ShopDefine Shop)
	{
		this.shop = Shop;
		this.title.text = Shop.Name;
		this.money.text = User.Instance.CurrentCharacter.Gold.ToString();
	}

	private UIShopItem SelectedItem;
	public void SelectedShopItem(UIShopItem ShopItem)
	{
		if (this.SelectedItem != null)
			this.SelectedItem.Selected = false;
		this.SelectedItem = ShopItem;
	}

	public void OnClickBuy()
	{
		if(SelectedItem == null)
		{
			MessageBox.Show("请选择购买物品","购买提示");
			return;
		}
		if(ShopManager.Instance.BuyItem(this.shop.ID, this.SelectedItem.shopItemId))
		{

        }


	}


}
