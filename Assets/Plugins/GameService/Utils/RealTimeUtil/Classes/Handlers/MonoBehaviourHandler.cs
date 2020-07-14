// <copyright file="MonoBehaviourHandler.cs" company="Firoozeh Technology LTD">
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


using Plugins.GameService.Utils.RealTimeUtil.Interfaces;
using UnityEngine;

namespace Plugins.GameService.Utils.RealTimeUtil.Classes.Handlers
{
    internal class MonoBehaviourHandler : IMonoBehaviourHandler
    {
        
        private MonoBehaviour _rootMono;
        internal static MonoBehaviour[] MonoBehaviours;


        public void Init(MonoBehaviour monoBehaviour)
        {
            _rootMono = monoBehaviour;
            RefreshMonoBehaviourCache();
        }
        
        /// <summary>
        /// Can be used to refresh the list of MonoBehaviours on this GameObject
        /// </summary>
        public void RefreshMonoBehaviourCache()
        {
            MonoBehaviours = _rootMono.GetComponents<MonoBehaviour>();
        }
        
        
    }
}