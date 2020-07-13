// <copyright file="GsLive.cs" company="Firoozeh Technology LTD">
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
using Plugins.GameService.Utils.RealTimeUtil.Classes.Handlers;
using Plugins.GameService.Utils.RealTimeUtil.Consts;
using Plugins.GameService.Utils.RealTimeUtil.Interfaces;
using Plugins.GameService.Utils.RealTimeUtil.Models;
using UnityEngine;
using Types = Plugins.GameService.Utils.RealTimeUtil.Consts.Types;

namespace Plugins.GameService.Utils.RealTimeUtil
{
    /// <summary>
    ///     Represents Game Service Realtime MultiPlayer System Helper
    /// </summary>
    public class GsLive
    {

        private static readonly IPrefabHandler PrefabHandler = new PrefabHandler();
        
        
        /// <summary>
        ///  Instantiate the GameObject For All PLayers in Room
        ///  notes : Your prefab Must Save in Resources folder
        /// </summary>
        public static GameObject Instantiate(string prefabName, Vector3 position, Quaternion rotation,byte[] extraData = null)
        {
            var instantiateData = new InstantiateData(prefabName,position,rotation,extraData);
            var gameObject = PrefabHandler.Instantiate(prefabName, position, rotation);
            NetworkInstantiate(instantiateData);
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

            var isDone = PrefabHandler.DestroyWithTag(gameObjTag);
            if (!isDone) 
                throw new GameServiceException("Failed to Destroy GameObject because GameObject With this Tag Not Found!");
            
            var gameObjData = new GameObjectData(objectTag : gameObjTag);
            NetworkDestroy(gameObjData);
        }
        
        
        
        /// <summary>
        ///  Destroy the GameObject For All PLayers in Room
        /// </summary>
        /// <param name="gameObjName"> the Game Object Name You Want To Destroy</param>
        public static void DestroyWithName(string gameObjName)
        {
            if (string.IsNullOrEmpty(gameObjName))
                throw new GameServiceException("Failed to Destroy GameObject because gameObjName Is NullOrEmpty");

            var isDone = PrefabHandler.DestroyWithName(gameObjName);
            if (!isDone) 
                throw new GameServiceException("Failed to Destroy GameObject because GameObject With this Name Not Found!");
            
            var gameObjData = new GameObjectData(gameObjName);
            NetworkDestroy(gameObjData);
        }

        

        private static void NetworkInstantiate(InstantiateData instantiateData)
        {
            if(!FiroozehGameService.Core.GameService.GSLive.IsRealTimeAvailable())
                throw new GameServiceException("RealTime is Not Available");

            var caller = new[] {(byte) Types.ObjectsActions,(byte) ObjectActions.Instantiate ,(byte) 0x0};
            FiroozehGameService.Core.GameService.GSLive.RealTime.SendEvent(caller,instantiateData.Serialize());
        }
        
        
        private static void NetworkDestroy(GameObjectData gameObjectData)
        {
            if(!FiroozehGameService.Core.GameService.GSLive.IsRealTimeAvailable())
                throw new GameServiceException("RealTime is Not Available");

            var caller = new[] {(byte) Types.ObjectsActions,(byte) ObjectActions.Destroy ,(byte) 0x0};
            FiroozehGameService.Core.GameService.GSLive.RealTime.SendEvent(caller,gameObjectData.Serialize());
        }
        
    }
}