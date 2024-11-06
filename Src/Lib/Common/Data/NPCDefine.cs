using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public enum NPCType
    {
        None = 0,
        Functional = 1,
        Task,
    }
    public enum NPCFunction
    {
        None = 0,
        InvokeShop = 1,
        InvokeInsrance,
    }
    public class NPCDefine
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public NPCType Type { get; set; }
        public NPCFunction Function { get; set; }
        public NVector3 Position { get; set; }
        public int Param { get; set; }
    }
}
