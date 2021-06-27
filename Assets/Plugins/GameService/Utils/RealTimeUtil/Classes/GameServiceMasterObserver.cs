// <copyright file="GameServiceMasterObserver.cs" company="Firoozeh Technology LTD">
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
using FiroozehGameService.Utils;
using FiroozehGameService.Utils.Serializer;
using FiroozehGameService.Utils.Serializer.Abstracts;
using FiroozehGameService.Utils.Serializer.Interfaces;
using Plugins.GameService.Tools.NaughtyAttributes.Scripts.Core.DrawerAttributes;
using Plugins.GameService.Tools.NaughtyAttributes.Scripts.Core.MetaAttributes;
using Plugins.GameService.Tools.NaughtyAttributes.Scripts.Core.ValidatorAttributes;
using Plugins.GameService.Utils.RealTimeUtil.Utils;
using UnityEngine;

namespace Plugins.GameService.Utils.RealTimeUtil.Classes
{
    public class GameServiceMasterObserver : MonoBehaviour
    {
        [BoxGroup("Set a unique ID to send data from this observer")]
        [ValidateInput("CheckId", "The ID Must Grater Than Zero And Lower Than 256")]
        public byte id;
        
        [BoxGroup("Observer Info (Set Automatically By Server)")]
        [ReadOnly]
        public string ownerId;

        [BoxGroup("Observer Info (Set Automatically By Server)")]
        [ReadOnly]
        public bool isMine;
        
        [BoxGroup("Observer Info (Set Automatically By Server)")]
        [ReadOnly]
        public bool isAvailable;

        [BoxGroup("Add Your Component that You Want To Serialize")]
        [ValidateInput("CheckComponent", "The Component Must Implements IGsLiveSerializable")]
        public MonoBehaviour serializableComponent;


        private EventUtil _callerEvent;
        
        private void Start()
        {
            if(!GsLiveRealtime.IsAvailable)
                throw new GameServiceException("GsLiveRealtime is Not Available!");
        }


        internal void RegisterObserver(string ownerMemberId,bool isMe = false)
        {
            ownerId = ownerMemberId;
            isMine = isMe;
            
            // register Observer
            _callerEvent = ObjectUtil.RegisterObserver(this);
            
            if(!isMe) return;
            _callerEvent.EventHandler += OnUpdate;
            _callerEvent.Start();
        }


        private void OnDestroy()
        {
            ObjectUtil.UnregisterObserver(this);
            _callerEvent?.Dispose();
        }


        private void OnUpdate(object sender, EventUtil e)
        {
            if (serializableComponent == null) return;
            if (FiroozehGameService.Core.GameService.GSLive.IsRealTimeAvailable())
            {
                SenderUtil.NetworkObserver(id, serializableComponent as IGsLiveSerializable);
                isAvailable = true;
            }
            else isAvailable = false;
        }
        

        internal void ApplyData(string ownerMemberId,byte[] data)
        {
            if(ownerId != null && ownerId != ownerMemberId)  return;
            
            if (serializableComponent == null)
                throw new GameServiceException("Cant Apply Data , Because Component is Null!");

            GsSerializer.Object.CallReadStream(serializableComponent as IGsLiveSerializable, data);
        }
        
        
        
        
        private bool CheckId(byte value) => value > 0;

        private bool CheckComponent(MonoBehaviour sc)
        {
            return sc != null &&  sc is IGsLiveSerializable;
        }
        
    }
}