using Models;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoSingleton<UIMain> {


	public Text avatarName;
	public Text avatarLevel;

	// Use this for initialization
	protected override void OnStart () {
		this.UpdateAvatar();
	}
	

	void UpdateAvatar()
	{
		this.avatarName.text = User.Instance.CurrentCharacter.Name;
		this.avatarLevel.text = User.Instance.CurrentCharacter.Level.ToString();
	}


	// Update is called once per frame
	void Update () {
		
	}

	public void BackToCharSelect()
	{
		SceneManager.Instance.LoadScene("CharSelect");
		Services.UserService.Instance.SendGameLeave();
	}

	public void OnBagButtonClick()
	{
		UIManager.Instance.Show<UIBag>();
	}
	
}
