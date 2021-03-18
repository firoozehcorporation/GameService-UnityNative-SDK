// <copyright file="PrefabHandler.cs" company="Firoozeh Technology LTD">
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

using System.Collections.Generic;
using Plugins.GameService.Utils.RealTimeUtil.Interfaces;
using UnityEngine;

namespace Plugins.GameService.Utils.RealTimeUtil.Classes.Handlers
{
    internal class PrefabHandler : IPrefabHandler
    {
        
        /// <summary>Contains a GameObject per prefabId, to speed up instantiation.</summary>
        private readonly Dictionary<string, GameObject> _resourceCache = new Dictionary<string, GameObject>();

        /// <summary>Returns an inActive instance of GameObject</summary>
        /// <param name="prefabId">String identifier for instance object.</param>
        /// <param name="position">Location of the new object.</param>
        /// <param name="rotation">Rotation of the new object.</param>
        /// <param name="ownerId"> the Object OwnerId</param>
        /// <param name="isMe"> True if Instantiate Called By Current Player</param>
        public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation,string ownerId = null,bool isMe = false)
        {
            var cached = _resourceCache.TryGetValue(prefabId, out var res);
            if (!cached)
            {
                res = (GameObject)Resources.Load(prefabId, typeof(GameObject));
                if (res == null)
                    Debug.LogError("PrefabHandler failed to load \"" + prefabId + "\" . Make sure it's in a \"Resources\" folder.");
                else
                    _resourceCache.Add(prefabId, res);
            }

            var wasActive = res.activeSelf;
            if (wasActive) res.SetActive(false);
            
            var instance = GameObject.Instantiate(res, position, rotation);
            var observer =  instance.GetComponent<GameServiceMasterObserver>();
            if (observer != null) observer.RegisterObserver(ownerId,isMe);
            
            if (wasActive) res.SetActive(true);

            return instance;
        }

        /// <summary>Simply destroys a GameObject.</summary>
        /// <param name="gameObject">The GameObject to get rid of.</param>
        public void Destroy(GameObject gameObject)
        {
            GameObject.Destroy(gameObject);
        }

        
        /// <summary>Simply destroys a GameObject.</summary>
        /// <param name="gameObjectName">The GameObject to get rid of.</param>
        public bool DestroyWithName(string gameObjectName)
        {
            var obj = GameObject.Find(gameObjectName);
            if(obj != null) GameObject.Destroy(obj);
            return obj != null;
        }

        /// <summary>Simply destroys a GameObject.</summary>
        /// <param name="gameObjectTag">The GameObject to get rid of.</param>
        public bool DestroyWithTag(string gameObjectTag)
        {
            var obj = GameObject.FindWithTag(gameObjectTag);
            if(obj != null) GameObject.Destroy(obj);
            return obj != null;
        }

        public void Dispose()
        {
            _resourceCache?.Clear();
        }
    }
}