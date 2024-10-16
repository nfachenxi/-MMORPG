﻿using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainCity : MonoBehaviour {


	public Text avatarName;
	public Text avatarLevel;

	// Use this for initialization
	void Start () {
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
}
