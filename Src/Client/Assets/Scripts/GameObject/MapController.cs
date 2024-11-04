using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {
	public Collider MinimapBoundingBox;
    // Use this for initialization
    void Start () {
		MinimapManager.Instance.UpdateMiniMap(MinimapBoundingBox);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
