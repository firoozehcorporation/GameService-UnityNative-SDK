// <copyright file="GsLiveRealtime.cs" company="Firoozeh Technology LTD">
// Copyright (C) 2020 Firoozeh Technology LTD. All Rights Reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>


/**
* @author Alireza Ghodrati
*/


using System;
using System.Collections.Generic;
using FiroozehGameService.Handlers;
using FiroozehGameService.Models;
using FiroozehGameService.Models.GSLive;
using FiroozehGameService.Models.GSLive.Command;
using FiroozehGameService.Models.GSLive.RT;
using FiroozehGameService.Utils.Serializer;
using FiroozehGameService.Utils.Serializer.Models;
using Plugins.GameService.Utils.RealTimeUtil.Classes;
using Plugins.GameService.Utils.RealTimeUtil.Classes.Handlers;
using Plugins.GameService.Utils.RealTimeUtil.Consts;
using Plugins.GameService.Utils.RealTimeUtil.Interfaces;
using Plugins.GameService.Utils.RealTimeUtil.Models;
using Plugins.GameService.Utils.RealTimeUtil.Models.CallbackModels;
using Plugins.GameService.Utils.RealTimeUtil.Models.SendableObjects;
using Plugins.GameService.Utils.RealTimeUtil.Utils;
using UnityEngine;
using Types = Plugins.GameService.Utils.RealTimeUtil.Consts.Types;

namespace Plugins.GameService.Utils.RealTimeUtil
{
    /// <summary>
    ///     Represents Game Service Realtime MultiPlayer System Helper
    /// </summary>
    public static class GsLiveRealtime
    {
        private static IPrefabHandler _prefabHandler;
        private static IFunctionHandler _functionHandler;
        private static IMonoBehaviourHandler _monoBehaviourHandler;
        private static IPropertyHandler _propertyHandler;
        private static IMemberHandler _memberHandler;
        
        public static bool IsAvailable;
        public const string Version = "1.6.0 Stable";
        
        public static string CurrentPlayerMemberId => GsSerializer.Object.GetCurrentPlayerMemberId();
        public static int SerializationRate => GsSerializer.Object.GetSerializationRate();


        public static int GetRoundTripTime()
        {
            if(FiroozehGameService.Core.GameService.GSLive.IsRealTimeAvailable())
               return FiroozehGameService.Core.GameService.GSLive.RealTime().GetRoundTripTime();
            return -1;
        }
        
        public static long GetPacketLost()
        {
            if(FiroozehGameService.Core.GameService.GSLive.IsRealTimeAvailable())
                return FiroozehGameService.Core.GameService.GSLive.RealTime().GetPacketLost();
            return -1;
        }

        public static bool IsCurrentPlayerObserving(MonoBehaviour monoBehaviour)
        {
            if (monoBehaviour == null) return false;
            return monoBehaviour.GetComponent<GameServiceMasterObserver>() != null && monoBehaviour.GetComponent<GameServiceMasterObserver>().isMine;
        }
        
        public static class Callbacks
        {
            public static EventHandler<OnBeforeInstantiate> OnBeforeInstantiateHandler;
            
            public static EventHandler<OnAfterInstantiate> OnAfterInstantiateHandler;
            
            public static EventHandler<OnDestroyObject> OnDestroyObjectHandler;

            public static EventHandler<OnPropertyEvent> OnPropertyEvent;
        }

        internal static void Init()
        {
            
            GsSerializer.OnNewEventHandler += OnNewEventHandler;
            GsSerializer.OnNewSnapShotReceived += OnNewSnapShotReceived;
            GsSerializer.CurrentPlayerLeftRoom += CurrentPlayerLeftRoom;
            GsSerializer.CurrentPlayerJoinRoom += CurrentPlayerJoinRoom;


            RealTimeEventHandlers.JoinedRoom += JoinedRoom;
            RealTimeEventHandlers.LeftRoom += LeftRoom;
            RealTimeEventHandlers.RoomMembersDetailReceived += RoomMembersDetailReceived;
            
            
            _monoBehaviourHandler = new MonoBehaviourHandler();
            _prefabHandler = new PrefabHandler();
            _functionHandler = new FunctionHandler();
            _propertyHandler = new PropertyHandler();
            _memberHandler = new MemberHandler();
            
            
            _monoBehaviourHandler.Init();
            _memberHandler.Init();
            _propertyHandler.Init(_memberHandler);
            
            TypeUtil.Init();
            ObjectUtil.Init();
            IsAvailable = true;
        }

       
        private static void LeftRoom(object sender, Member member)
        {
            _memberHandler?.RemoveMember(member);
        }

        private static void CurrentPlayerLeftRoom(object sender, EventArgs e)
        {
            _propertyHandler?.Dispose();
            _memberHandler?.Dispose();
        }

        private static void RoomMembersDetailReceived(object sender, List<Member> members)
        {
            foreach (var member in members) _memberHandler?.AddMember(member);
        }

        
        private static void CurrentPlayerJoinRoom(object sender, EventArgs e)
        {
            FiroozehGameService.Core.GameService.GSLive.RealTime().GetRoomMembersDetail();
        }
        
        private static void JoinedRoom(object sender, JoinEvent eEvent)
        {
            _memberHandler?.AddMember(eEvent.JoinData.JoinedMember);
        }


        private static void OnNewEventHandler(object sender, EventData eventData)
        {
            var action = (Types) eventData.Caller[0];
            ActionUtil.ApplyData(action,eventData.SenderId,eventData.Caller,eventData.Data,_prefabHandler,_monoBehaviourHandler,_propertyHandler);
        }
        
        private static void OnNewSnapShotReceived(object sender, List<SnapShotData> snapShotData)
        {
            ActionUtil.ApplySnapShot(snapShotData,_prefabHandler,_monoBehaviourHandler,_propertyHandler);
        }
        
        

        internal static void Dispose()
        {
            GsSerializer.OnNewEventHandler = null;
            GsSerializer.OnNewSnapShotReceived = null;
            
            _monoBehaviourHandler?.Dispose();
            _prefabHandler?.Dispose();
            _propertyHandler?.Dispose();
            _memberHandler?.Dispose();
            
            TypeUtil.Dispose();
            ObjectUtil.Dispose();
            IsAvailable = false;
        }
        
        
        /// <summary>
        ///  Instantiate the GameObject For All PLayers in Room
        ///  notes : Your prefab Must Save in Resources folder
        /// </summary>
        public static GameObject Instantiate(string prefabName, Vector3 position, Quaternion rotation)
        {
            if(!IsAvailable)
                throw new GameServiceException("GsLiveRealtime is Not Available");

            if(!FiroozehGameService.Core.GameService.GSLive.IsRealTimeAvailable())
                throw new GameServiceException("RealTime is Not Available");
            
            
            var instantiateData = new InstantiateData(prefabName,position,rotation);
            var gameObject = _prefabHandler.Instantiate(prefabName, position, rotation,CurrentPlayerMemberId,true);
            
            SenderUtil.NetworkInstantiate(instantiateData);
            return gameObject;
        }

        
        
        /// <summary>
        ///  Destroy the GameObject For All PLayers in Room
        /// </summary>
        /// <param name="gameObjTag"> the Game Object Tag You Want To Destroy</param>
        public static bool DestroyWithTag(string gameObjTag)
        {
            if(!IsAvailable)
                throw new GameServiceException("GsLiveRealtime is Not Available");
            
            if(!FiroozehGameService.Core.GameService.GSLive.IsRealTimeAvailable())
                throw new GameServiceException("RealTime is Not Available");

            if (string.IsNullOrEmpty(gameObjTag))
               throw new GameServiceException("Failed to Destroy GameObject because gameObjTag Is NullOrEmpty");

            
            var isDone = _prefabHandler.DestroyWithTag(gameObjTag);
            if (!isDone) 
                throw new GameServiceException("Failed to Destroy GameObject because GameObject With this Tag Not Found!");
            
            var gameObjData = new GameObjectData(true,gameObjTag);
            SenderUtil.NetworkDestroy(gameObjData);

            return true;
        }
        
        
        
        /// <summary>
        ///  Destroy the GameObject For All PLayers in Room
        /// </summary>
        /// <param name="gameObjName"> the Game Object Name You Want To Destroy</param>
        public static bool DestroyWithName(string gameObjName)
        {
            if(!IsAvailable)
                throw new GameServiceException("GsLiveRealtime is Not Available");
            
            if(!FiroozehGameService.Core.GameService.GSLive.IsRealTimeAvailable())
                throw new GameServiceException("RealTime is Not Available");

            if (string.IsNullOrEmpty(gameObjName))
                throw new GameServiceException("Failed to Destroy GameObject because gameObjName Is NullOrEmpty");

            var isDone = _prefabHandler.DestroyWithName(gameObjName);
            if (!isDone) 
                throw new GameServiceException("Failed to Destroy GameObject because GameObject With this Name Not Found!");
            
            var gameObjData = new GameObjectData(false,gameObjName);
            SenderUtil.NetworkDestroy(gameObjData);

            return true;
        }

        
        
        /// <summary>
        /// Run a Function method on remote clients of this room (or on all, including this client).
        /// </summary>
        /// <param name="functionName">The name of a fitting function that was has the GsLiveFunction attribute.</param>
        /// <param name="type">The group of targets and the way the Function gets sent.</param>
        /// <param name="parameters">The Parameters that the Function method has.</param>
        public static void RunFunction<TFrom>(string functionName,FunctionType type, params object[] parameters)
        {
            if(!IsAvailable)
                throw new GameServiceException("GsLiveRealtime is Not Available");
            
            if(!FiroozehGameService.Core.GameService.GSLive.IsRealTimeAvailable())
                throw new GameServiceException("RealTime is Not Available");

            var objType = typeof(TFrom);
            
            _monoBehaviourHandler.RefreshMonoBehaviourCache();
            var isOk = _functionHandler.RunFunction(functionName,objType,type,parameters);
            if (!isOk) return;

            var extraBuffer = GsSerializer.Function.SerializeParams(parameters);
            var functionData = new FunctionData(objType.FullName,functionName,type,extraBuffer);
            
            // run on this Client
            if (type == FunctionType.All || type == FunctionType.AllBuffered)
                ActionUtil.ApplyFunction(functionData : functionData,monoBehaviourHandler : _monoBehaviourHandler);
            
            SenderUtil.NetworkRunFunction(functionData);
        }


        /// <summary>
        /// Set Or Update Member Property ,You Can Add or Edit A Member Property
        /// </summary>
        /// <param name="property">The Property You Want To Add or Edit</param>
        public static void SetOrUpdateMemberProperty(Property property)
        {
            if(!IsAvailable)
                throw new GameServiceException("GsLiveRealtime is Not Available");
            
            if(!FiroozehGameService.Core.GameService.GSLive.IsRealTimeAvailable())
                throw new GameServiceException("RealTime is Not Available");
            
            _propertyHandler.ApplyMemberProperty(CurrentPlayerMemberId,property);
           
            var propertyData = new PropertyData(property.PropertyName,property.PropertyData);
            SenderUtil.NetworkProperty(propertyData,PropertyAction.SetOrUpdateMemberProperty);
        }
        
        
        /// <summary>
        /// Remove a Member Property With propertyName
        /// </summary>
        /// <param name="propertyName">The name of a Property You Want To Remove it</param>
        public static void RemoveMemberProperty(string propertyName)
        {
            if(!IsAvailable)
                throw new GameServiceException("GsLiveRealtime is Not Available");
            
            if(!FiroozehGameService.Core.GameService.GSLive.IsRealTimeAvailable())
                throw new GameServiceException("RealTime is Not Available");
            
            _propertyHandler.RemoveMemberProperty(CurrentPlayerMemberId,propertyName);
            
            var property = new PropertyData(propertyName);
            SenderUtil.NetworkProperty(property,PropertyAction.RemoveMemberProperty);
        }
        
        
        
        /// <summary>
        /// Set Or Update Room Property  ,You Can Add or Edit A Property
        /// </summary>
        /// <param name="property">The Property You Want To Add or Edit</param>
        public static void SetOrUpdateRoomProperty(Property property)
        {
            if(!IsAvailable)
                throw new GameServiceException("GsLiveRealtime is Not Available");
            
            if(!FiroozehGameService.Core.GameService.GSLive.IsRealTimeAvailable())
                throw new GameServiceException("RealTime is Not Available");
            
            _propertyHandler.ApplyRoomProperty(property);
           
            var propertyData = new PropertyData(property.PropertyName,property.PropertyData);
            SenderUtil.NetworkProperty(propertyData,PropertyAction.SetOrUpdateRoomProperty);
        }
        
        
        /// <summary>
        /// Remove a Room Property With propertyName
        /// </summary>
        /// <param name="propertyName">The name of a Property You Want To Remove it</param>
        public static void RemoveRoomProperty(string propertyName)
        {
            if(!IsAvailable)
                throw new GameServiceException("GsLiveRealtime is Not Available");
            
            if(!FiroozehGameService.Core.GameService.GSLive.IsRealTimeAvailable())
                throw new GameServiceException("RealTime is Not Available");
            
            _propertyHandler.RemoveRoomProperty(propertyName);
            
            var property = new PropertyData(propertyName);
            SenderUtil.NetworkProperty(property,PropertyAction.RemoveRoomProperty);
        }
        
        
        /// <summary>
        /// Get All Property For a Member With Id
        /// </summary>
        /// <param name="memberId">The id of a Member You Want To get All Data</param>
        public static Dictionary<string,object> GetMemberProperties(string memberId)
        {
            if(!IsAvailable)
                throw new GameServiceException("GsLiveRealtime is Not Available");
            
            if(!FiroozehGameService.Core.GameService.GSLive.IsRealTimeAvailable())
                throw new GameServiceException("RealTime is Not Available");
            
            return _propertyHandler.GetMemberProperties(memberId);
        }
        
        
        
        /// <summary>
        /// Get All Room Properties
        /// </summary>
        public static Dictionary<string,object> GetRoomProperties()
        {
            if(!IsAvailable)
                throw new GameServiceException("GsLiveRealtime is Not Available");
            
            if(!FiroozehGameService.Core.GameService.GSLive.IsRealTimeAvailable())
                throw new GameServiceException("RealTime is Not Available");
            
            return _propertyHandler.GetRoomProperties();
        }
        
        
        
        /// <summary>
        /// Get room Property With propertyName
        /// </summary>
        /// <param name="propertyName">The name of a Property You Want To Find</param>
        public static object GetRoomProperty(string propertyName)
        {
            if(!IsAvailable)
                throw new GameServiceException("GsLiveRealtime is Not Available");
            
            if(!FiroozehGameService.Core.GameService.GSLive.IsRealTimeAvailable())
                throw new GameServiceException("RealTime is Not Available");
            
            return _propertyHandler.GetRoomProperty(propertyName);
        }
        
        
        /// <summary>
        /// Get All Members have This Property
        /// </summary>
        /// <param name="property">The Property You Want To Find it</param>
        public static List<Member> GetPropertyMembers(Property property)
        {
            if(!IsAvailable)
                throw new GameServiceException("GsLiveRealtime is Not Available");
            
            if(!FiroozehGameService.Core.GameService.GSLive.IsRealTimeAvailable())
                throw new GameServiceException("RealTime is Not Available");
            
            return _propertyHandler.GetPropertyMembers(property);
        }
        
        
        /// <summary>
        /// Get All Members have This Property
        /// </summary>
        /// <param name="propertyName">The name of a Property You Want To Find</param>
        public static List<Member> GetPropertyMembers(string propertyName)
        {
            if(!IsAvailable)
                throw new GameServiceException("GsLiveRealtime is Not Available");
            
            if(!FiroozehGameService.Core.GameService.GSLive.IsRealTimeAvailable())
                throw new GameServiceException("RealTime is Not Available");
            
            return _propertyHandler.GetPropertyMembers(propertyName);
        }
        
        
        /// <summary>
        /// Get All Values have This property Name
        /// </summary>
        /// <param name="propertyName">The name of a Property You Want To Find</param>
        public static List<object> GetPropertyValues(string propertyName)
        {
            if(!IsAvailable)
                throw new GameServiceException("GsLiveRealtime is Not Available");
            
            if(!FiroozehGameService.Core.GameService.GSLive.IsRealTimeAvailable())
                throw new GameServiceException("RealTime is Not Available");
            
            return _propertyHandler.GetPropertyValues(propertyName);
        }
        
        
        /// <summary>
        /// Get Room Members
        /// </summary>
        public static List<Member> GetRoomMembers()
        {
            if(!IsAvailable)
                throw new GameServiceException("GsLiveRealtime is Not Available");
            
            if(!FiroozehGameService.Core.GameService.GSLive.IsRealTimeAvailable())
                throw new GameServiceException("RealTime is Not Available");

            return _memberHandler.GetAllMembers();
        }

    }
}