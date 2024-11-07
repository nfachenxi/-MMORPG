using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillBridge.Message;
using Common.Data;

namespace Models
{
    public class Item
    {
        public int ID;
        public int Count;
        public ItemDefine Define;

        public Item(NItemInfo item)
        {
            this.ID = item.Id;
            this.Count = item.Count;
            this.Define = DataManager.Instance.Items[item.Id];
        }

        public override string ToString()
        {
            return string.Format("ID: {0}, Count: {1}", this.ID, this.Count);
        }
    }
}

