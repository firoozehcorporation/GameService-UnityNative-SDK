// <copyright file="RealtimeRigidbody2DObserver.cs.cs" company="Firoozeh Technology LTD">
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
using FiroozehGameService.Utils.Serializer.Helpers;
using FiroozehGameService.Utils.Serializer.Interfaces;
using Plugins.GameService.Tools.NaughtyAttributes.Scripts.Core.MetaAttributes;
using UnityEngine;

namespace Plugins.GameService.Utils.RealTimeUtil.Classes.Observers
{
    
    public class RealtimeRigidbody2DObserver : MonoBehaviour,IGsLiveSerializable
    {
        
        [BoxGroup("Config Values")]
        public bool synchronizeVelocity = true;
        
        [BoxGroup("Config Values")]
        public bool synchronizeAngularVelocity = true;
        
        [BoxGroup("Config Values")]
        public bool teleportEnabled;
        
        [ShowIf("teleportEnabled")]
        [BoxGroup("Config Values")]
        public float teleportIfDistanceGreaterThan = 3.0f;        
        
        
        private float _mDistance;
        private float _mAngle;

        
        
        private Rigidbody2D _rBody;
        private Vector2 _mNetworkPosition;
        private float _mNetworkRotation;

        private bool _mFirstTake;
        

        public void Awake()
        {
            _rBody = GetComponent<Rigidbody2D>();
            _mNetworkPosition = Vector2.zero;
        }
        
        public void FixedUpdate()
        {
            if(GsLiveRealtime.IsCurrentPlayerObserving(this)) return;

            _rBody.position = Vector2.MoveTowards(_rBody.position, _mNetworkPosition, _mDistance * (1.0f / GsLiveRealtime.SerializationRate));
            _rBody.rotation = Mathf.MoveTowards(_rBody.rotation, _mNetworkRotation, _mAngle * (1.0f / GsLiveRealtime.SerializationRate));
        }
        
        
        
        public void OnGsLiveRead(GsReadStream readStream)
        {
            try
            {
                _mNetworkPosition = (Vector2)    readStream.ReadNext();
                _mNetworkRotation = (float)      readStream.ReadNext();

                if (teleportEnabled)
                {
                    if (Vector2.Distance(_rBody.position, _mNetworkPosition) > teleportIfDistanceGreaterThan)
                        _rBody.position = _mNetworkPosition;
                }
                
                if (synchronizeVelocity || synchronizeAngularVelocity)
                {
                    var lag  = (float) GsLiveRealtime.GetRoundTripTime() / 100;
                    
                    if (synchronizeVelocity)
                    {
                        _rBody.velocity = (Vector2) readStream.ReadNext();
                        _mNetworkPosition += _rBody.velocity * lag;
                        _mDistance = Vector2.Distance(_rBody.position, _mNetworkPosition);
                    }

                    if (synchronizeAngularVelocity)
                    {
                        _rBody.angularVelocity = (float) readStream.ReadNext();
                        _mNetworkRotation += _rBody.angularVelocity * lag;
                        _mAngle = Mathf.Abs(_rBody.rotation - _mNetworkRotation);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("RealtimeRigidbody2DObserver OnGsLiveRead Error : " + e);
            }
        }

        public void OnGsLiveWrite(GsWriteStream writeStream)
        {
             try
             {
                 writeStream.WriteNext(_rBody.position);
                 writeStream.WriteNext(_rBody.rotation);

                 if (synchronizeVelocity)
                     writeStream.WriteNext(_rBody.velocity);
                 
                 if (synchronizeAngularVelocity)
                     writeStream.WriteNext(_rBody.angularVelocity);
             }
             catch (Exception e)
             {
                 Debug.LogError("RealtimeRigidbody2DObserver OnGsLiveWrite Error : " + e);
             }
        }
    }
}