// <copyright file="RealtimeTransformObserver.cs" company="Firoozeh Technology LTD">
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
    
    public class RealtimeTransformObserver : MonoBehaviour,IGsLiveSerializable
    {
        
        [BoxGroup("Config Values")]
        public bool synchronizePosition = true;
        
        [BoxGroup("Config Values")]
        public bool synchronizeRotation = true;
        
        [BoxGroup("Config Values")]
        public bool synchronizeScale;
        
        
        
        private float _mDistance;
        private float _mAngle;

        
        
        private Vector3 _mDirection;
        private Vector3 _mNetworkPosition;
        private Vector3 _mStoredPosition;
        
        private Quaternion _mNetworkRotation;

        private bool _mFirstTake;
        

        public void Awake()
        {
            _mStoredPosition = transform.position;
            _mNetworkPosition = Vector3.zero;
            _mNetworkRotation = Quaternion.identity;
        }
        
        public void Update()
        {
            if(GsLiveRealtime.IsCurrentPlayerObserving(this)) return;

            transform.position = Vector3.MoveTowards(transform.position, _mNetworkPosition, _mDistance * (1.0f / GsLiveRealtime.SerializationRate));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _mNetworkRotation, _mAngle * (1.0f / GsLiveRealtime.SerializationRate));
        }
        
        
        
        public void OnGsLiveRead(GsReadStream readStream)
        {
            try
            {
                if (synchronizePosition)
                {
                    _mNetworkPosition = (Vector3) readStream.ReadNext();
                    _mDirection       = (Vector3) readStream.ReadNext();
                    
                    if (_mFirstTake)
                    {
                        transform.position = _mNetworkPosition;
                        _mDistance = 0f;
                    }
                    else
                    {
                        var lag  = (float) GsLiveRealtime.GetRoundTripTime() / 100;
                        _mNetworkPosition += _mDirection * lag;
                        _mDistance = Vector3.Distance(transform.position, _mNetworkPosition);
                    }


                }

                if (synchronizeRotation)
                {
                    _mNetworkRotation = (Quaternion) readStream.ReadNext();

                    if (_mFirstTake)
                    {
                        _mAngle = 0f;
                        transform.rotation = _mNetworkRotation;
                    }
                    else
                        _mAngle = Quaternion.Angle(transform.rotation, _mNetworkRotation);
                    
                }

                if (synchronizeScale)
                    transform.localScale = (Vector3) readStream.ReadNext();
                

                if (_mFirstTake)
                    _mFirstTake = false;
                
            }
            catch (Exception e)
            {
                Debug.LogError("GSLiveTransformObserver OnGsLiveRead Error : " + e);
            }
        }

        public void OnGsLiveWrite(GsWriteStream writeStream)
        {
             try
             {
                 if (synchronizePosition)
                 {
                     _mDirection = transform.position - _mStoredPosition;
                     _mStoredPosition = transform.position;

                     writeStream.WriteNext(transform.position);
                     writeStream.WriteNext(_mDirection);
                 }

                 if (synchronizeRotation)
                     writeStream.WriteNext(transform.rotation);
                 
                 if (synchronizeScale)
                     writeStream.WriteNext(transform.localScale);
                 
             }
             catch (Exception e)
             {
                 Debug.LogError("GSLiveTransformObserver OnGsLiveWrite Error : " + e);
             }
        }
    }
}