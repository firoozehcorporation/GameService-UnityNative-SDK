// <copyright file="RealtimeRigidbodyObserver.cs" company="Firoozeh Technology LTD">
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
    
    public class RealtimeRigidbodyObserver : MonoBehaviour,IGsLiveSerializable
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

        
        
        private Rigidbody _rBody;
        private Vector3 _mNetworkPosition;
        private Quaternion _mNetworkRotation;

        private bool _mFirstTake;
        

        public void Awake()
        {
            _rBody = GetComponent<Rigidbody>();
            _mNetworkPosition = Vector3.zero;
            _mNetworkRotation = Quaternion.identity;
        }
        
        public void FixedUpdate()
        {
            if(GsLiveRealtime.IsCurrentPlayerObserving(this)) return;

            _rBody.position = Vector3.MoveTowards(_rBody.position, _mNetworkPosition, _mDistance * (1.0f / GsLiveRealtime.SerializationRate));
            _rBody.rotation = Quaternion.RotateTowards(_rBody.rotation, _mNetworkRotation, _mAngle * (1.0f / GsLiveRealtime.SerializationRate));
        }
        
        
        
        public void OnGsLiveRead(GsReadStream readStream)
        {
            try
            {
                _mNetworkPosition = (Vector3)    readStream.ReadNext();
                _mNetworkRotation = (Quaternion) readStream.ReadNext();

                if (teleportEnabled)
                {
                    if (Vector3.Distance(_rBody.position, _mNetworkPosition) > teleportIfDistanceGreaterThan)
                        _rBody.position = _mNetworkPosition;
                }
                
                if (synchronizeVelocity || synchronizeAngularVelocity)
                {
                    var lag  = (float) GsLiveRealtime.GetRoundTripTime() / 100;
                    
                    if (synchronizeVelocity)
                    {
                        _rBody.velocity = (Vector3) readStream.ReadNext();
                        _mNetworkPosition += _rBody.velocity * lag;
                        _mDistance = Vector3.Distance(_rBody.position, _mNetworkPosition);
                    }

                    if (synchronizeAngularVelocity)
                    {
                        _rBody.angularVelocity = (Vector3) readStream.ReadNext();
                        _mNetworkRotation = Quaternion.Euler(_rBody.angularVelocity * lag) * _mNetworkRotation;
                        _mAngle = Quaternion.Angle(_rBody.rotation, _mNetworkRotation);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("RealtimeRigidbodyObserver OnGsLiveRead Error : " + e);
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
                 Debug.LogError("RealtimeRigidbodyObserver OnGsLiveWrite Error : " + e);
             }
        }
    }
}