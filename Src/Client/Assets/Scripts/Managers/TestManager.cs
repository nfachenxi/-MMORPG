using Common.Data;
using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : Singleton<TestManager> {
    public void Init()
    {
        NPCManager.Instance.RegisterNPCEvent(Common.Data.NPCFunction.InvokeShop, OnNPCInvokeShop);
        NPCManager.Instance.RegisterNPCEvent(Common.Data.NPCFunction.InvokeInsrance, OnNPCInvokeInsrance);
    }

    private bool OnNPCInvokeInsrance(NPCDefine npc)
    {
        Debug.LogFormat("TestManager.OnNPCInvokeInsrance : NPC : [{0} : {1}] type: {2} Func:{3} ", npc.ID, npc.Name, npc.Type, npc.Function);
        MessageBox.Show("点击了:" + npc.Name + "NPC对话");
        return true;
    }

    private bool OnNPCInvokeShop(NPCDefine npc)
    {
        Debug.LogFormat("TestManager.OnNPCInvokeShop : NPC : [{0} : {1}] type: {2} Func:{3} ", npc.ID, npc.Name, npc.Type, npc.Function);
        UIManager.Instance.Show<UITest>();
        return true;
    }
}
