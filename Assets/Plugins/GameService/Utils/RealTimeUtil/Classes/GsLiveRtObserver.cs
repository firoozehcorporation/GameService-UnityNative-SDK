// <copyright file="GsLiveRtObserver.cs" company="Firoozeh Technology LTD">
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
using FiroozehGameService.Models;
using FiroozehGameService.Utils;
using Plugins.GameService.Utils.RealTimeUtil.Consts;
using Plugins.GameService.Utils.RealTimeUtil.Interfaces;
using Plugins.GameService.Utils.RealTimeUtil.Utils;
using Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer;
using UnityEngine;
using Event = FiroozehGameService.Utils.Event;

namespace Plugins.GameService.Utils.RealTimeUtil.Classes
{
    public class GsLiveRtObserver : MonoBehaviour
    {
        
        [Header("Set a unique ID to send data from this observer.")]
        public byte id;
        
        [Header("Add Your Components that You Want To Observe And Serializable")]
        public List<IGsLiveSerializable> serializableComponents;


        private Event _callerEvent;
        
        private void OnEnable()
        {
            if (serializableComponents?.Count > Sizes.MaxId)
                throw new GameServiceException("observableComponents Count is Too Large!");
            
            // register Observer
            ObjectUtil.RegisterObserver(this);
            
            // add Event
            _callerEvent = EventCallerUtil.AddEvent(id, 100);
            _callerEvent.EventHandler += OnUpdate;
        }

        
        
        private void OnUpdate(object sender, Event e)
        {
            if (serializableComponents == null) return;
            byte idCounter = 0;
            foreach (var component in serializableComponents)
                SenderUtil.NetworkObserver(id,idCounter++,component);
        }
        

        internal void ApplyData(byte componentId,byte[] data)
        {
            if (componentId > serializableComponents.Count) 
                throw new GameServiceException("Cant Apply Data , Because Component With This id Not Found");
            
            var currentComponent = serializableComponents[componentId];
            if (currentComponent == null)
                throw new GameServiceException("Cant Apply Data , Because Component With This id Not Found");

            currentComponent.CallReadStream(data);
        }
    }
}