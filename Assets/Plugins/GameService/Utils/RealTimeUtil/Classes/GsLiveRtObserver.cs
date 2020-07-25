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


using System.Linq;
using FiroozehGameService.Models;
using FiroozehGameService.Utils.Serializer;
using FiroozehGameService.Utils.Serializer.Interfaces;
using Plugins.GameService.Tools.NaughtyAttributes.Scripts.Core.DrawerAttributes;
using Plugins.GameService.Tools.NaughtyAttributes.Scripts.Core.DrawerAttributes_SpecialCase;
using Plugins.GameService.Tools.NaughtyAttributes.Scripts.Core.MetaAttributes;
using Plugins.GameService.Tools.NaughtyAttributes.Scripts.Core.ValidatorAttributes;
using Plugins.GameService.Utils.RealTimeUtil.Consts;
using Plugins.GameService.Utils.RealTimeUtil.Utils;
using UnityEngine;
using Event = FiroozehGameService.Utils.Event;

namespace Plugins.GameService.Utils.RealTimeUtil.Classes
{
    public class GsLiveRtObserver : MonoBehaviour
    {
        [BoxGroup("Set a unique ID to send data from this observer")]
        [ValidateInput("CheckId", "The ID Must Grater Than Zero And Lower Than 256")]
        public byte id;
        
        [BoxGroup("Observer Infos (Set By Server)")]
        [ReadOnly]
        public string ownerId;

        [BoxGroup("Observer Infos (Set By Server)")]
        [ReadOnly]
        public bool forMe;

        [ReorderableList]
        [BoxGroup("Add Your Components that You Want To Serialize")]
        [ValidateInput("CheckComponents", "The Component Must Implements IGsLiveSerializable")]
        public MonoBehaviour[] serializableComponents;


        private Event _callerEvent;
        
        private void Start()
        {
            if(!GsLiveRealtime.IsAvailable)
                throw new GameServiceException("GsLiveRealtime is Not Available!");

            if (serializableComponents?.Length > Sizes.MaxId)
                throw new GameServiceException("SerializableComponents Count is Too Large!");
        }


        internal void RegisterObserver(string ownerMemberId,bool isMe = false)
        {
            ownerId = ownerMemberId;
            forMe = isMe;
            
            // register Observer
            _callerEvent = ObjectUtil.RegisterObserver(this);
            
            if(!isMe) return;
            _callerEvent.EventHandler += OnUpdate;
            _callerEvent.Start();
        }


        private void OnDestroy()
        {
            ObjectUtil.UnregisterObserver(this);
        }


        private void OnUpdate(object sender, Event e)
        {
            if (serializableComponents == null) return;
            byte idCounter = 0;
            foreach (var component in serializableComponents)
               SenderUtil.NetworkObserver(id,idCounter++,component as IGsLiveSerializable);
        }
        

        internal void ApplyData(byte componentId,string ownerId,byte[] data)
        {
            if (ownerId != this.ownerId) return;
            
            if (componentId > serializableComponents.Length)
                throw new GameServiceException("Cant Apply Data , Because Component With This id Not Found");

            var currentComponent = serializableComponents[componentId];
            if (currentComponent == null)
                throw new GameServiceException("Cant Apply Data , Because Component With This id Not Found");

            GsSerializer.Object.CallReadStream(currentComponent as IGsLiveSerializable, data);
        }
        
        
        
        
        private bool CheckId(byte value) => value > 0;

        private bool CheckComponents(MonoBehaviour[] sc)
        {
            return sc != null && sc.All(component => component is IGsLiveSerializable);
        }
        
    }
}