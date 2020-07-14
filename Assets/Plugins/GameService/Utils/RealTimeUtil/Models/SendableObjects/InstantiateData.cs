// <copyright file="InstantiateData.cs" company="Firoozeh Technology LTD">
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
using FiroozehGameService.Models;
using Plugins.GameService.Utils.RealTimeUtil.Classes.Abstracts;
using Plugins.GameService.Utils.RealTimeUtil.Consts;
using Plugins.GameService.Utils.RealTimeUtil.Utils.IO;
using UnityEngine;

namespace Plugins.GameService.Utils.RealTimeUtil.Models.SendableObjects
{
    internal class InstantiateData : GsLiveSerializable
    {
        internal string PrefabName;
        internal Vector3 Position;
        internal Quaternion Rotation;
        
        private int _prefabLen;

        public InstantiateData(byte[] buffer)
        {
            Deserialize(buffer);
        }
        
        public InstantiateData(string prefabName, Vector3 position, Quaternion rotation)
        {
            if (string.IsNullOrEmpty(prefabName))
                throw new GameServiceException("prefabName Cant Be NullOrEmpty");
            if (Position == null)
                throw new GameServiceException("Position Cant Be Null");
            if (Rotation == null)
                throw new GameServiceException("Rotation Cant Be Null");
            
            PrefabName = prefabName;
            Position = position;
            Rotation = rotation;
        }


        internal override byte[] Serialize()
        {
            try
            {
                // Check Buffer Size
                var prefabName = GetBuffer(PrefabName,false);
                _prefabLen = prefabName.Length;
                
                if(_prefabLen > Sizes.MaxPrefabName)
                    throw new GameServiceException("Prefab Name is Too Large!");
                
                

                var bufferSize = sizeof(byte) + 7 * sizeof(float) + sizeof(ushort) + 
                                                  + _prefabLen;

                // Get Binary Buffer
                var packetBuffer = BufferPool.GetBuffer(bufferSize);
                using (var packetWriter = ByteArrayReaderWriter.Get(packetBuffer))
                {
                    // Write Headers
                    packetWriter.Write((byte)_prefabLen);

                    // Write Data
                    // Write Position
                    packetWriter.Write(BitConverter.GetBytes(Position.x));
                    packetWriter.Write(BitConverter.GetBytes(Position.y));
                    packetWriter.Write(BitConverter.GetBytes(Position.z));
                    
                    // Write Rotation
                    packetWriter.Write(BitConverter.GetBytes(Rotation.x));
                    packetWriter.Write(BitConverter.GetBytes(Rotation.y));
                    packetWriter.Write(BitConverter.GetBytes(Rotation.z));
                    packetWriter.Write(BitConverter.GetBytes(Rotation.w));
                    

                   packetWriter.Write(prefabName);
                }

                return packetBuffer;
            }
            catch (Exception e)
            {
               Debug.LogError("InstantiateData Serialize Error : " + e);
               return null;
            }
        }


        internal sealed override void Deserialize(byte[] buffer)
        {
            try
            {
                using (var packetWriter = ByteArrayReaderWriter.Get(buffer))
                {
                    var prefabLen = packetWriter.ReadByte();

                    var x = BitConverter.ToSingle(packetWriter.ReadBytes(sizeof(float)),0);
                    var y = BitConverter.ToSingle(packetWriter.ReadBytes(sizeof(float)),0);
                    var z = BitConverter.ToSingle(packetWriter.ReadBytes(sizeof(float)),0);
                    Position = new Vector3(x,y,z);

                    x = BitConverter.ToSingle(packetWriter.ReadBytes(sizeof(float)),0);
                     y = BitConverter.ToSingle(packetWriter.ReadBytes(sizeof(float)),0);
                     z = BitConverter.ToSingle(packetWriter.ReadBytes(sizeof(float)),0);
                     var w = BitConverter.ToSingle(packetWriter.ReadBytes(sizeof(float)),0);
                    Rotation = new Quaternion(x,y,z,w);

                    PrefabName = GetStringFromBuffer(packetWriter.ReadBytes(prefabLen),false);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("InstantiateData Deserialize Error : " + e);
            }
        }
    }
}