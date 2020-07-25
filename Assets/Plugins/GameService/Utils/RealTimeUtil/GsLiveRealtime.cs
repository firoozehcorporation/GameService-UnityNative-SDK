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
using FiroozehGameService.Models;
using FiroozehGameService.Models.GSLive.RT;
using FiroozehGameService.Utils.Serializer;
using FiroozehGameService.Utils.Serializer.Models;
using Plugins.GameService.Utils.RealTimeUtil.Classes.Handlers;
using Plugins.GameService.Utils.RealTimeUtil.Consts;
using Plugins.GameService.Utils.RealTimeUtil.Interfaces;
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
        public static bool IsAvailable;

        public const string Version = "Alpha 1.0.0";
        
        public static string CurrentPlayerMemberId => GsSerializer.Object.GetCurrentPlayerMemberId();

        public static short SerializationRate => 10;
        
        public class Callbacks
        {
            public static EventHandler<OnBeforeInstantiate> OnBeforeInstantiateHandler;
            public static EventHandler<OnAfterInstantiate> OnAfterInstantiateHandler;
            
            public static EventHandler<OnDestroyObject> OnDestroyObjectHandler;
        }

        internal static void Init()
        {
            
            GsSerializer.OnNewEventHandler += OnNewEventHandler;
            GsSerializer.OnNewSnapShotReceived += OnNewSnapShotReceived;
            
            _monoBehaviourHandler = new MonoBehaviourHandler();
            _prefabHandler = new PrefabHandler();
            _functionHandler = new FunctionHandler();
            
            _monoBehaviourHandler.Init();
            TypeUtil.Init();
            ObjectUtil.Init();
            IsAvailable = true;
        }

        
        private static void OnNewEventHandler(object sender, EventData eventData)
        {
            var action = (Types) eventData.Caller[0];
            ActionUtil.ApplyData(action,eventData.SenderId,eventData.Caller,eventData.Data,_prefabHandler,_monoBehaviourHandler);
        }
        
        private static void OnNewSnapShotReceived(object sender, List<SnapShotData> snapShotDatas)
        {
            ActionUtil.ApplySnapShot(snapShotDatas,_prefabHandler,_monoBehaviourHandler);
        }
        
        

        internal static void Dispose()
        {
            GsSerializer.OnNewEventHandler = null;
            GsSerializer.OnNewSnapShotReceived = null;
            
            _monoBehaviourHandler?.Dispose();
            _prefabHandler?.Dispose();
            
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
        public static void DestroyWithTag(string gameObjTag)
        {
            if (string.IsNullOrEmpty(gameObjTag))
               throw new GameServiceException("Failed to Destroy GameObject because gameObjTag Is NullOrEmpty");

            var isDone = _prefabHandler.DestroyWithTag(gameObjTag);
            if (!isDone) 
                throw new GameServiceException("Failed to Destroy GameObject because GameObject With this Tag Not Found!");
            
            var gameObjData = new GameObjectData(true,gameObjTag);
            SenderUtil.NetworkDestroy(gameObjData);
        }
        
        
        
        /// <summary>
        ///  Destroy the GameObject For All PLayers in Room
        /// </summary>
        /// <param name="gameObjName"> the Game Object Name You Want To Destroy</param>
        public static void DestroyWithName(string gameObjName)
        {
            if (string.IsNullOrEmpty(gameObjName))
                throw new GameServiceException("Failed to Destroy GameObject because gameObjName Is NullOrEmpty");

            var isDone = _prefabHandler.DestroyWithName(gameObjName);
            if (!isDone) 
                throw new GameServiceException("Failed to Destroy GameObject because GameObject With this Name Not Found!");
            
            var gameObjData = new GameObjectData(false,gameObjName);
            SenderUtil.NetworkDestroy(gameObjData);
        }

        
        
        /// <summary>
        /// Run a Function method on remote clients of this room (or on all, including this client).
        /// </summary>
        /// <param name="functionName">The name of a fitting function that was has the GsLiveFunction attribute.</param>
        /// <param name="type">The group of targets and the way the Function gets sent.</param>
        /// <param name="parameters">The Parameters that the Function method has.</param>
        public static void RunFunction<TFrom>(string functionName,FunctionType type, params object[] parameters)
        {
            if(!FiroozehGameService.Core.GameService.GSLive.IsRealTimeAvailable())
                throw new GameServiceException("RealTime is Not Available");
            
            var objType = typeof(TFrom);
            
            _monoBehaviourHandler.RefreshMonoBehaviourCache();
            var isOk = _functionHandler.RunFunction(functionName,objType,type,parameters);
            if (!isOk) return;


            var extraBuffer = GsSerializer.Function.SerializeParams(parameters);
            var functionData = new FunctionData(objType.FullName,functionName,type,extraBuffer);
           
            // run on this Client
            if (type == FunctionType.All || type == FunctionType.Buffered)
                ActionUtil.ApplyFunction(functionData : functionData,monoBehaviourHandler : _monoBehaviourHandler);

            SenderUtil.NetworkRunFunction(functionData);
        }
        
    }
}