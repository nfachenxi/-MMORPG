using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using SkillBridge.Message;

namespace GameServer.Services
{
    internal class NFAWelcomeService : Singleton<NFAWelcomeService>
    {
        public void Init()
        {

        }
        public void Start()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<NFALoginRequest>(this.OnNFALoginRequest);
        }

        void OnNFALoginRequest(NetConnection<NetSession> sender, NFALoginRequest request)
        {
            Log.InfoFormat("登录信息：{0}", request.Welcome);
        }

        public void Stop()
        {

        }
    }
}
