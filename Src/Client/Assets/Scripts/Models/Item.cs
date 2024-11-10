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

        public Item(NItemInfo item):
            this(item.Id,item.Count)
        { 
        }
        public Item(int id, int count)
        {
            this.ID = id;
            this.Count = count;
            this.Define = DataManager.Instance.Items[this.ID];
        }

        public override string ToString()
        {
            return string.Format("ID: {0}, Count: {1}", this.ID, this.Count);
        }
    }
}

