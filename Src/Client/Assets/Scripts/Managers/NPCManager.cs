using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers {
    class NPCManager : Singleton<NPCManager>
    {
        public delegate bool NPCActionHandler(NPCDefine npc);

        Dictionary<NPCFunction, NPCActionHandler> eventMap = new Dictionary<NPCFunction, NPCActionHandler>();
        public void RegisterNPCEvent(NPCFunction function, NPCActionHandler action)
        {
            if(!eventMap.ContainsKey(function))
            {
                eventMap[function] = action;
            }
            else
            {
                eventMap[function] += action;
            }
        }

        public NPCDefine GetNPCDefine(int NPCId)
        {
            NPCDefine npc = null;
            DataManager.Instance.NPCs.TryGetValue(NPCId,out npc);
            return npc;
        }


        public bool Interactive(int NPCId)
        {
            if(DataManager.Instance.NPCs.ContainsKey(NPCId))
            {
                var npc = DataManager.Instance.NPCs[NPCId];
                return Interactive(npc);
            }
            return false;
        }

        public bool Interactive(NPCDefine npc)
        {
            if(npc.Type == NPCType.Task)
            {
                return OnTaskInteractive(npc);
            }
            else if(npc.Type == NPCType.Functional)
            {
                return OnFunctionInteractive(npc);
            }
            return false;
        }

        private bool OnTaskInteractive(NPCDefine npc)
        {
            MessageBox.Show("点击了:" + npc.Name + "NPC对话");
            return true;
        }
        private bool OnFunctionInteractive(NPCDefine npc)
        {
            if (npc.Type != NPCType.Functional)
                return false;
            if (!eventMap.ContainsKey(npc.Function))
                return false;
            return eventMap[npc.Function](npc);
        }

    }
}

