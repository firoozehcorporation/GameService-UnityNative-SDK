// <copyright file="UnityModels.cs" company="Firoozeh Technology LTD">
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
using Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer.Helpers;
using UnityEngine;

namespace Plugins.GameService.Utils.RealTimeUtil.Classes.Helpers

{
    internal static class UnityModels
    {

        internal static byte[] Serialize(this Vector3 vector3)
        {
            var packetBuffer = BufferPool.GetBuffer(3 * sizeof(float));
            using (var packetWriter = ByteArrayReaderWriter.Get(packetBuffer))
            {
                packetWriter.Write(BitConverter.GetBytes(vector3.x));
                packetWriter.Write(BitConverter.GetBytes(vector3.y));
                packetWriter.Write(BitConverter.GetBytes(vector3.z));
            }

            return packetBuffer;
        }
        
        internal static Vector3 DeserializeToVector3(byte[] buffer)
        {
            using (var packetReader = ByteArrayReaderWriter.Get(buffer))
            {
                var x = BitConverter.ToSingle(packetReader.ReadBytes(sizeof(float)), 0);
                var y = BitConverter.ToSingle(packetReader.ReadBytes(sizeof(float)), 0);
                var z = BitConverter.ToSingle(packetReader.ReadBytes(sizeof(float)), 0);
                return new Vector3(x,y,z);
            }
        }

        
        internal static byte[] Serialize(this Quaternion rotation)
        {
            var packetBuffer = BufferPool.GetBuffer(4 * sizeof(float));
            using (var packetWriter = ByteArrayReaderWriter.Get(packetBuffer))
            {
                packetWriter.Write(BitConverter.GetBytes(rotation.x));
                packetWriter.Write(BitConverter.GetBytes(rotation.y));
                packetWriter.Write(BitConverter.GetBytes(rotation.z));
                packetWriter.Write(BitConverter.GetBytes(rotation.w));
            }

            return packetBuffer;
        }
        
        
        internal static Quaternion DeserializeToQuaternion(byte[] buffer)
        {
            using (var packetReader = ByteArrayReaderWriter.Get(buffer))
            {
                var x = BitConverter.ToSingle(packetReader.ReadBytes(sizeof(float)), 0);
                var y = BitConverter.ToSingle(packetReader.ReadBytes(sizeof(float)), 0);
                var z = BitConverter.ToSingle(packetReader.ReadBytes(sizeof(float)), 0);
                var w = BitConverter.ToSingle(packetReader.ReadBytes(sizeof(float)), 0);
                return new Quaternion(x,y,z,w);
            }
        }
        
    }
}