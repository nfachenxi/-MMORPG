using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterSelect : MonoBehaviour {

	public GameObject panelCreate;
	public GameObject panelSelect;

	public GameObject EnterGameButton;

	public InputField charName;
	CharacterClass charClass;

	public Transform uiCharList;
	public GameObject uiCharInfo;

	public List<GameObject> uiChars = new List<GameObject>();

	public Image[] titles;
	public Image[] UnSelects;
	public Image[] Selects;

	public Text descs;

	public Text[] names;

	private int selectCharacteridx = -1;

	public UICharacterView characterView;

	// Use this for initialization
	void Start () {
		InitCharacterSelect(true);
	}
	public void InitCharacterSelect(bool init)
	{
		panelCreate.SetActive(false);
		panelSelect.SetActive(true);

		if(init)
		{
			foreach(var old in uiChars)
			{
				Destroy(old);
			}
			uiChars.Clear();
		}
	}
	
	public void InitCharacterCreate()
	{
		panelCreate.SetActive(true);
		panelSelect.SetActive(false);
	}


	// Update is called once per frame
	void Update () {
		
	}

	public void OnClickCreate()
	{

	}

	public void OnSelectClass(int charClass)
	{
		this.charClass = (CharacterClass)charClass;

		characterView.CurrentCharacter = charClass - 1;

		for(int i = 0; i < 3; i ++)
		{
			titles[i].gameObject.SetActive(i == charClass - 1);
			Selects[i].gameObject.SetActive(i == charClass - 1);
            UnSelects[i].gameObject.SetActive(i != charClass - 1);
			names[i].text = DataManager.Instance.Characters[i + 1].Name;
		}

		descs.text = DataManager.Instance.Characters[charClass].Description;
	}

	void OnCharacterCreate(Result result, string message)
	{
		if (result == Result.Success)
		{
			InitCharacterSelect(true);
		}
		else
			MessageBox.Show(message, "错误", MessageBoxType.Error);
	}

	public void OnClickPlay()
	{
		if(selectCharacteridx >= 0)
		{
			MessageBox.Show("进入游戏", "进入游戏", MessageBoxType.Confirm);
		}
	}

}
