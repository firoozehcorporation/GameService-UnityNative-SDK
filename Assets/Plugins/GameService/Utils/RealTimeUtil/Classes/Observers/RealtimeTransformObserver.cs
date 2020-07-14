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
using Plugins.GameService.Utils.GSLiveRT.Classes.Attributes;
using Plugins.GameService.Utils.GSLiveRT.Utils.IO;
using UnityEngine;
using GsLiveSerializable = Plugins.GameService.Utils.GSLiveRT.Classes.Abstracts.GsLiveSerializable;

namespace Plugins.GameService.Utils.GSLiveRT.Classes.Observers
{
    
    public class RealtimeTransformObserver : GsLiveSerializable
    {

        [Header("Config Values")]
        public float posThreshold = 0.5f;
        public float rotThreshold = 5;
        public float scaleThreshold = 0.5f;
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


        internal override byte[] Serialize()
        {
            try
            {
                // send to server
                var newPos = _transform.position;
                var newRot = _transform.rotation;
                var newScale = _transform.localScale;

                // Check Buffer Size
                byte havePos = 0x0, haveRot = 0x0, haveScale = 0x0;
                var bufferSize = 3 * sizeof(byte);

                if (Vector3.Distance(_oldPosition, newPos) > posThreshold)
                {
                    _oldPosition = newPos;
                    if (synchronizePosition)
                    {
                        havePos = 0x1;
                        bufferSize += 3 * sizeof(float);
                        NeedsToUpdate = true;
                    }
                }

                if (Quaternion.Angle(_oldRotation, newRot) > rotThreshold)
                {
                    _oldRotation = newRot;
                    if (synchronizeRotation)
                    {
                        haveRot = 0x1;
                        bufferSize += 4 * sizeof(float);
                        NeedsToUpdate = true;
                    }
                }

                if (synchronizeScale)
                {
                    haveScale = 0x1;
                    bufferSize += 3 * sizeof(float);
                    NeedsToUpdate = true;
                }

                
                if (!NeedsToUpdate) return null;
                
                // Get Binary Buffer
                var packetBuffer = BufferPool.GetBuffer(bufferSize);
                using (var packetWriter = ByteArrayReaderWriter.Get(packetBuffer))
                {
                    // Write Headers
                    packetWriter.Write(havePos);
                    packetWriter.Write(haveRot);
                    packetWriter.Write(haveScale);

                    if (havePos == 0x1)
                    {
                        packetWriter.Write(BitConverter.GetBytes(newPos.x));
                        packetWriter.Write(BitConverter.GetBytes(newPos.y));
                        packetWriter.Write(BitConverter.GetBytes(newPos.z));
                    }

                    if (haveRot == 0x1)
                    {
                        packetWriter.Write(BitConverter.GetBytes(newRot.x));
                        packetWriter.Write(BitConverter.GetBytes(newRot.y));
                        packetWriter.Write(BitConverter.GetBytes(newRot.z));
                        packetWriter.Write(BitConverter.GetBytes(newRot.w));
                    }

                    if (haveScale == 0x1)
                    {
                        packetWriter.Write(BitConverter.GetBytes(newScale.x));
                        packetWriter.Write(BitConverter.GetBytes(newScale.y));
                        packetWriter.Write(BitConverter.GetBytes(newScale.z));
                    }

                }

                return packetBuffer;
            }
            catch (Exception e)
            {
               Debug.LogError("GSLiveTransformObserver Serialize Error : " + e);
               return null;
            }
        }
        

        internal override void Deserialize(byte[] buffer)
        {
            try
            {
                using (var packetWriter = ByteArrayReaderWriter.Get(buffer))
                {
                    var havePos = packetWriter.ReadByte();
                    var haveRot = packetWriter.ReadByte();
                    var haveScale = packetWriter.ReadByte();
                    
                    
                    if (havePos == 0x1)
                    {
                        var x = (float) BitConverter.ToDouble(packetWriter.ReadBytes(sizeof(float)),0);
                        var y = (float) BitConverter.ToDouble(packetWriter.ReadBytes(sizeof(float)),0);
                        var z = (float) BitConverter.ToDouble(packetWriter.ReadBytes(sizeof(float)),0);
                        _mNetworkPosition = new Vector3(x,y,z);
                    }

                    if (haveRot == 0x1)
                    {
                        var x = (float) BitConverter.ToDouble(packetWriter.ReadBytes(sizeof(float)),0);
                        var y = (float) BitConverter.ToDouble(packetWriter.ReadBytes(sizeof(float)),0);
                        var z = (float) BitConverter.ToDouble(packetWriter.ReadBytes(sizeof(float)),0);
                        var w = (float) BitConverter.ToDouble(packetWriter.ReadBytes(sizeof(float)),0);
                        _mNetworkRotation = new Quaternion(x,y,z,w);
                    }

                    if (haveScale == 0x1)
                    {
                        var x = (float) BitConverter.ToDouble(packetWriter.ReadBytes(sizeof(float)),0);
                        var y = (float) BitConverter.ToDouble(packetWriter.ReadBytes(sizeof(float)),0);
                        var z = (float) BitConverter.ToDouble(packetWriter.ReadBytes(sizeof(float)),0);
                        _mNetworkScale = new Vector3(x,y,z);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("GSLiveTransformObserver Deserialize Error : " + e);
            }
        }
    }
}