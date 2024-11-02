using Common.Data;
using Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using System.Text;

namespace Services
{
    public class MapService : Singleton<MapService>, IDisposable
    {
        // 构造函数订阅消息
        public MapService()
        {
            MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Subscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
            MessageDistributer.Instance.Subscribe<MapEntitySyncResponse>(this.OnMapEntitySync);
            
        }

        // 当前地图ID
        public int CurrentMapId { get; set; }

        // 释放资源
        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Unsubscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
            MessageDistributer.Instance.Unsubscribe<MapEntitySyncResponse>(this.OnMapEntitySync);
        }

        // 初始化方法
        public void Init()
        {
            // 可以在这里进行一些初始化操作
        }

        // 处理角色进入地图的消息
        private void OnMapCharacterEnter(object sender, MapCharacterEnterResponse response)
        {
            Debug.LogFormat("OnMapCharacterEnter:Map{0} Count:{1}", response.mapId, response.Characters.Count);
            foreach (var cha in response.Characters)
            {
                if (User.Instance.CurrentCharacter == null || User.Instance.CurrentCharacter.Id == cha.Id  )
                {
                    // 当前角色切换地图
                    User.Instance.CurrentCharacter = cha;
                }
                CharacterManager.Instance.AddCharacter(cha);
            }
            if (CurrentMapId != response.mapId)
            {
                this.EnterMap(response.mapId);
                this.CurrentMapId = response.mapId;
            }
        }

        // 处理角色离开地图的消息
        private void OnMapCharacterLeave(object sender, MapCharacterLeaveResponse response)
        {
            Debug.LogFormat("OnMapCharacterLeave: CharID: {0}", response.characterId);
            if (response.characterId != User.Instance.CurrentCharacter.Id)
                CharacterManager.Instance.RemoveCharacter(response.characterId);
            else
                CharacterManager.Instance.Clear();
        }

        // 进入指定的地图
        private void EnterMap(int mapId)
        {
            if (DataManager.Instance.Maps.ContainsKey(mapId))
            {
                MapDefine map = DataManager.Instance.Maps[mapId];
                User.Instance.CurrentMapData = map;
                SceneManager.Instance.LoadScene(map.Resource);
            }
            else
                Debug.LogErrorFormat("EnterMap: Map{0} not existed", mapId);
        }

        // 同步实体到服务器
        public void SendMapEntitySync(EntityEvent entityEvent, NEntity entity)
        {
            Debug.LogFormat("MapEntityUpdateRequest : ID: {0} POS: {1} DIR: {2} SPD: {3}", entity.Id, entity.Position.ToString(), entity.Direction.String(), entity.Speed);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapEntitySync = new MapEntitySyncRequest()
            {
                entitySync = new NEntitySync()
                {
                    Id = entity.Id,
                    Event = entityEvent,
                    Entity = entity
                }
            };
            NetClient.Instance.SendMessage(message);
        }

        // 处理实体同步响应
        private void OnMapEntitySync(object sender, MapEntitySyncResponse response)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("MapEntityUpdateResponse: Entitys: {0}", response.entitySyncs.Count);
            sb.AppendLine();
            foreach (var entity in response.entitySyncs)
            {
                EntityManager.Instance.OnEntitySync(entity);
                sb.AppendFormat("[{0}]evt : {1} entity: {2}", entity.Id, entity.Event, entity.Entity.String());
                sb.AppendLine();
            }
            Debug.Log(sb.ToString());
        }

        public void SendMapTeleport(int teleporterID)
        {
            Debug.LogFormat("MapTeleportRequest : teleporterID : {0}", teleporterID);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapTeleport = new MapTeleportRequest();
            message.Request.mapTeleport.teleporterId = teleporterID;
            NetClient.Instance.SendMessage(message);
        }
    }
}