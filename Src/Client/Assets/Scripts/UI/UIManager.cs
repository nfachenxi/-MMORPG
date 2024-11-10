using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager> {

	class UIElement
	{
		public string Resources;
		public bool CaChe;
		public GameObject Instance;
	}

	private Dictionary<Type, UIElement> UIResources = new Dictionary<Type, UIElement>();

	public UIManager()
	{
		//this.UIResources.Add(typeof(UITest), new UIElement() {Resources = "UI/UITest", CaChe = true });
		this.UIResources.Add(typeof(UIBag), new UIElement() {Resources = "UI/UIBag", CaChe = false });
		this.UIResources.Add(typeof(UIShop), new UIElement() {Resources = "UI/UIShop", CaChe = false });
	}

	~UIManager()
	{
	}

	/// <summary>
	/// UIShow
	/// </summary>
	public T Show<T>()
	{
		Type type = typeof(T);
		if (UIResources.ContainsKey(type)) 
		{
			UIElement info = UIResources[type];
			if(info.Instance != null)
			{
				info.Instance.SetActive(true);
			}
			else
			{
				UnityEngine.Object prefab = Resources.Load(info.Resources);
				if (prefab == null)
					return default(T);
				info.Instance = (GameObject)GameObject.Instantiate(prefab);
			}
			return info.Instance.GetComponent<T>();
		}
		return default(T);
	}

	public void Close(Type type)
	{
		if(UIResources.ContainsKey(type))
		{
            UIElement info = UIResources[type];
			if(info.CaChe)
			{
				info.Instance.SetActive(false);
			}
			else
			{
				GameObject.Destroy(info.Instance);
				info.Instance = null;
            }
        }	
	}
}
