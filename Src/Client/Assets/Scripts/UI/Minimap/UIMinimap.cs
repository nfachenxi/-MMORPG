using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIMinimap : MonoBehaviour {


	public Collider MinimapBoundingBox;
	public Image minimap;
	public Image arrow;
	public Text mapName;

	private Transform playerTransform;

	void Start () {
		InitMap();
    }
	
	void InitMap()
	{
        this.mapName.text = User.Instance.CurrentMapData.Name;
		if(this.minimap.overrideSprite == null)
			this.minimap.overrideSprite = MinimapManager.Instance.LoadCurrentMinimap();

		this.minimap.SetNativeSize();
		this.minimap.transform.localPosition = Vector3.zero;
		this.playerTransform = User.Instance.CurrentCharacterObject.transform;
    }


	void Update () {
		if (MinimapBoundingBox == null || playerTransform == null) return;

		float realWidth = MinimapBoundingBox.bounds.size.x;
		float realHeight = MinimapBoundingBox.bounds.size.z;

		float relaX = playerTransform.position.x - MinimapBoundingBox.bounds.min.x;
		float relaY = playerTransform.position.z - MinimapBoundingBox.bounds.min.z;

		float pivotX = relaX / realWidth;
		float pivotY = relaY / realHeight;

		this.minimap.rectTransform.pivot = new Vector2(pivotX, pivotY);
		this.minimap.rectTransform.localPosition = Vector2.zero;

		this.arrow.transform.eulerAngles = new Vector3(0, 0, -playerTransform.eulerAngles.y);
	}
}
