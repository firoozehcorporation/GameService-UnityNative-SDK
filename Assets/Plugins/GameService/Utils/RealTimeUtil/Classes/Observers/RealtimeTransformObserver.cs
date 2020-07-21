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
using Plugins.GameService.Utils.RealTimeUtil.Interfaces;
using Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer;
using Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer.Helpers;
using UnityEngine;

namespace Plugins.GameService.Utils.RealTimeUtil.Classes.Observers
{
    
    public class RealtimeTransformObserver : MonoBehaviour,IGsLiveSerializable
    {

        [Header("Config Values")]
        public float posThreshold = 0.5f;
        public float rotThreshold = 5;
        public float lerpRate = 10;
        public bool synchronizePosition = true;
        public bool synchronizeRotation = true;
        public bool synchronizeScale = false;


        private Transform _transform;

        
        private Vector3 _mNetworkPosition;
        private Quaternion _mNetworkRotation;
        private Vector3 _mNetworkScale;

        private Vector3 _oldPosition;
        private Quaternion _oldRotation;


        public void Start()
        {
            _transform = GetComponent<Transform>();
            var position = _transform.position;
            var rotation = _transform.rotation;
            var scale = _transform.localScale;
            
            _oldPosition = position;
            _oldRotation = rotation;

            _mNetworkPosition = position;
            _mNetworkRotation = rotation;
            _mNetworkScale = scale;
        }
        
        public void Update()
        {
            _transform.position = Vector3.Lerp(_transform.position, _mNetworkPosition, Time.deltaTime * lerpRate);
            _transform.rotation = Quaternion.Lerp(_transform.rotation,_mNetworkRotation, Time.deltaTime * lerpRate);
            _transform.localScale = _mNetworkScale;
        }
        
        public void OnGsLiveRead(GsReadStream readStream)
        {
            bool havePos, haveRot, haveScale;

            havePos   = (bool) readStream.ReadNext();
            haveRot   = (bool) readStream.ReadNext();
            haveScale = (bool) readStream.ReadNext();

            if (havePos) _mNetworkPosition = GsSerializer.Transform.DeserializeToVector3(readStream.ReadNext() as byte[]);
            if (haveRot) _mNetworkRotation = GsSerializer.Transform.DeserializeToQuaternion(readStream.ReadNext() as byte[]);
            if (haveScale) _mNetworkScale = GsSerializer.Transform.DeserializeToVector3(readStream.ReadNext() as byte[]);
        }

        public void OnGsLiveWrite(GsWriteStream writeStream)
        {
             try
             {
                 // send to server
                 var newPos = _transform.position;
                 var newRot = _transform.rotation;
                 var newScale = _transform.localScale;

                 bool havePos, haveRot, haveScale;
                 
                 if (Vector3.Distance(_oldPosition, newPos) > posThreshold)
                 {
                     _oldPosition = newPos;
                     if (synchronizePosition) havePos = true;
                 }

                 if (Quaternion.Angle(_oldRotation, newRot) > rotThreshold)
                 {
                     _oldRotation = newRot;
                     if (synchronizeRotation) haveRot = true;
                 }

                 if (synchronizeScale)
                     haveScale = true;
                 
                 writeStream.WriteNext(havePos);
                 writeStream.WriteNext(haveRot);
                 writeStream.WriteNext(haveScale);
                 
                 if(havePos)   writeStream.WriteNext(GsSerializer.Transform.Serialize(newPos));
                 if(haveRot)   writeStream.WriteNext(GsSerializer.Transform.Serialize(newRot));
                 if(haveScale) writeStream.WriteNext(GsSerializer.Transform.Serialize(newScale));

             }
             catch (Exception e)
             {
                 Debug.LogError("GSLiveTransformObserver OnGsLiveWrite Error : " + e);
             }
        }
    }
}