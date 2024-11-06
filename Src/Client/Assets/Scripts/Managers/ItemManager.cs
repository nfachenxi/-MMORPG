using Common.Data;
using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Managers
{
    public class ItemManager : Singleton<ItemManager>
    {
        public Dictionary<int, Item> Items = new Dictionary<int, Item>();

        public void Init(List<NItemInfo> items)
        {
            this.Items.Clear();
            foreach(var info in items)
            {
                Item item = new Item(info);
                this.Items.Add(item.ID,item);

                Debug.LogFormat("ItemManager : init[{0}]", item);
            }
        }
        
        public ItemDefine GetItem(int itemID)
        {

            return null;
        }


        public bool UseItem(int itemID)
        {
            return false;
        }
        
        public bool UseItem(ItemDefine item)
        {
            return false;
        }

    }
}

