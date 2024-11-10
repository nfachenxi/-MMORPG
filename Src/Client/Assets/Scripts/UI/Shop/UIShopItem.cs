using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIShopItem : MonoBehaviour,ISelectHandler
{
    public Image icon;
	public Image background;
	public Text title;
	public Text price;
	public Text count;
	public Sprite normalBackground;
	public Sprite selectedBackground;

	private bool selected;
	public bool Selected
	{
		get { return selected; }
		set
		{
			selected = value;
			this.background.overrideSprite = selected ? selectedBackground : normalBackground;
		}
	}

	public int shopItemId { get; set; }
	private UIShop shop;
	private ItemDefine item;
	private ShopItemDefine shopItem { get; set; }

	public void SetShopItem(int Id, ShopItemDefine ShopItem,UIShop Shop)
	{
		this.shopItemId = Id;
		this.shopItem = ShopItem;
		this.shop = Shop;

		this.item = DataManager.Instance.Items[this.shopItem.ID];
		this.title.text = this.item.Name;
		this.price.text = this.shopItem.Price.ToString();
		this.count.text = this.shopItem.Count.ToString();
		this.icon.overrideSprite = Resloader.Load<Sprite>(this.item.Icon);
	}


	public void OnSelect(BaseEventData eventData)
	{
		this.Selected = true;
		this.shop.SelectedShopItem(this);

    }
}
