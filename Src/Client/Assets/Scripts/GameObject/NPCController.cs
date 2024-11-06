using Common.Data;
using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour {
	public int NPCId;
	SkinnedMeshRenderer renderer;
	Color orignColor;
	private bool inInteractive = false;
	NPCDefine npc;
	Animator anim;
	void Start () {
		renderer = this.GetComponentInChildren<SkinnedMeshRenderer>();
        anim = this.GetComponentInChildren<Animator>();
        orignColor = renderer.sharedMaterial.color;
		npc = NPCManager.Instance.GetNPCDefine(NPCId);
		this.StartCoroutine(Actions());
    }

	IEnumerator Actions()
	{
		while(true)
		{
			if (inInteractive)
				yield return new WaitForSeconds(2f);
			else
                yield return new WaitForSeconds(Random.Range(10f,15f));
			this.Relax();
        }
	}

	void Relax()
	{
		this.anim.SetTrigger("Relax");
	}

	void Interactive()
	{
		if(!inInteractive)
		{
			inInteractive = true;
			this.StartCoroutine(DoInteractive());
		}
	}
	IEnumerator DoInteractive()
	{
		yield return FaceToPlayer();
		if(NPCManager.Instance.Interactive(npc))
		{
			this.anim.SetTrigger("Talk");
		}
		yield return new WaitForSeconds(3f);
		inInteractive = false;
	}

    IEnumerator FaceToPlayer()
	{
		Vector3 faceTo = (User.Instance.CurrentCharacterObject.transform.position - this.transform.position).normalized;
		while(Mathf.Abs(Vector3.Angle(this.gameObject.transform.forward, faceTo)) > 5)
		{
			this.gameObject.transform.forward = Vector3.Lerp(this.gameObject.transform.forward, faceTo, Time.deltaTime * 5f);
			yield return null;
		}
	}

	void OnMouseDown()
	{
		Interactive();
	}

    void OnMouseOver()
    {
        Highlight(true);
    }

    void OnMouseEnter()
    {
        Highlight(true);
    }

    void OnMouseExit()
    {
        Highlight(false);
    }

    void Highlight(bool highlight)
	{
		if(highlight)
		{
			if (renderer.sharedMaterial.color != Color.white)
				renderer.sharedMaterial.color = Color.white;
        }
		else
		{
			if (renderer.sharedMaterial.color != orignColor)
				renderer.sharedMaterial.color = orignColor;

        }
	}
}
