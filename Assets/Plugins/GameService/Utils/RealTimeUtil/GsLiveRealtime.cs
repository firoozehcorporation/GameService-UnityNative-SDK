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


using FiroozehGameService.Models;
using FiroozehGameService.Models.GSLive.RT;
using Plugins.GameService.Utils.GSLiveRT.Classes.Handlers;
using Plugins.GameService.Utils.GSLiveRT.Consts;
using Plugins.GameService.Utils.GSLiveRT.Interfaces;
using Plugins.GameService.Utils.GSLiveRT.Models.SendableObjects;
using Plugins.GameService.Utils.GSLiveRT.Utils;
using UnityEngine;
using Types = Plugins.GameService.Utils.GSLiveRT.Consts.Types;

namespace Plugins.GameService.Utils.GSLiveRT
{
    /// <summary>
    ///     Represents Game Service Realtime MultiPlayer System Helper
    /// </summary>
    public static class GsLiveRealtime
    {

        private static IPrefabHandler _prefabHandler;
        private static IFunctionHandler _functionHandler;
        private static IMonoBehaviourHandler _monoBehaviourHandler;


        internal static void Init()
        {
            _monoBehaviourHandler = new MonoBehaviourHandler();
            _prefabHandler = new PrefabHandler();
            _functionHandler = new FunctionHandler();
            
            _monoBehaviourHandler.RefreshMonoBehaviourCache();
            ObjectUtil.Init();
        }
        
        internal static void NewEventReceived(object sender, EventData eventData)
        {
            var action = (Types) eventData.Caller[0];
            ActionUtil.ApplyData(action,eventData.Caller,eventData.Data,_prefabHandler);
        }
        
        
        /// <summary>
        ///  Instantiate the GameObject For All PLayers in Room
        ///  notes : Your prefab Must Save in Resources folder
        /// </summary>
        public static GameObject Instantiate(string prefabName, Vector3 position, Quaternion rotation,byte[] extraData = null)
        {
            var instantiateData = new InstantiateData(prefabName,position,rotation,extraData);
            var gameObject = _prefabHandler.Instantiate(prefabName, position, rotation);
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
            
            var gameObjData = new GameObjectData(objectTag : gameObjTag);
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
            
            var gameObjData = new GameObjectData(gameObjName);
            SenderUtil.NetworkDestroy(gameObjData);
        }

        
        
        #if UNITY_2019_2_OR_NEWER
        /// <summary>
        /// Run a Function method on remote clients of this room (or on all, including this client).
        /// </summary>
        /// <param name="methodName">The name of a fitting method that was has the GsLiveFunction attribute.</param>
        /// <param name="type">The group of targets and the way the Function gets sent.</param>
        /// <param name="extraData">The Extra Data that the Function method has.</param>
        public static void RunFunction(string methodName, FunctionType type, byte[] extraData = null)
        {
            if(!FiroozehGameService.Core.GameService.GSLive.IsRealTimeAvailable())
                throw new GameServiceException("RealTime is Not Available");
            
            var isOk = _functionHandler.RunFunction(methodName,type,extraData);
            if (!isOk) return;
            
            var functionData = new FunctionData(methodName,type,extraData);
            SenderUtil.NetworkRunFunction(functionData);
        }
        #endif

        
    }
}