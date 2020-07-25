// <copyright file="IPrefabHandler.cs" company="Firoozeh Technology LTD">
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


using UnityEngine;

namespace Plugins.GameService.Utils.RealTimeUtil.Interfaces
{
    public interface IPrefabHandler
    {
        /// <summary>
        /// Called to get an instance of a prefab. Must return valid, Disabled GameObject
        /// </summary>
        /// <param name="prefabId">The id of this prefab.</param>
        /// <param name="position">The position for the instance.</param>
        /// <param name="rotation">The rotation for the instance.</param>
        /// <param name="ownerId"> The Object OwnerID</param>
        /// <param name="isMe"> True if Instantiate Called By Current Player</param>
        /// <returns>A Disabled instance to use by GSLive or null if the prefabId is unknown.</returns>
        GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation,string ownerId = null,bool isMe = false);

        /// <summary>
        /// Called to destroy the instance of a prefab.
        /// </summary>
        /// <param name="gameObject">The instance to destroy.</param>
        void Destroy(GameObject gameObject);
        
        
        /// <summary>
        /// Called to destroy the instance of a prefab.
        /// </summary>
        /// <param name="gameObjectName">The instance Name to destroy.</param>
        bool DestroyWithName(string gameObjectName);
        
        
        /// <summary>
        /// Called to destroy the instance of a prefab.
        /// </summary>
        /// <param name="gameObjectTag">The instance Name to destroy.</param>
        bool DestroyWithTag(string gameObjectTag);


        void Dispose();
    }
}