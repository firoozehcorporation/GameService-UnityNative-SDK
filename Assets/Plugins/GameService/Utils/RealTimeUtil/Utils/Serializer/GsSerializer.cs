// <copyright file="GsSerializer.cs" company="Firoozeh Technology LTD">
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
using Plugins.GameService.Utils.RealTimeUtil.Classes.Helpers;
using Plugins.GameService.Utils.RealTimeUtil.Interfaces;
using Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer.Helpers;
using Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer.Utils;
using UnityEngine;

namespace Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer
{
    public static class GsSerializer
    {

        public static class Transform
        {
            public static byte[] Serialize(Vector3 vector3)
            {
                return vector3.Serialize();
            }
            
            public static byte[] Serialize(Quaternion rotation)
            {
                return rotation.Serialize();
            }
        
            
            
            
            public static Vector3 DeserializeToVector3(byte[] buffer)
            {
                return UnityModels.DeserializeToVector3(buffer);
            }
            
            
            public static Quaternion DeserializeToQuaternion(byte[] buffer)
            {
                return UnityModels.DeserializeToQuaternion(buffer);
            }

            
            
            
        }
       
        
        
        
        
        internal static byte[] GetBuffer(IGsLiveSerializable serializable)
        {
            if(serializable == null)
                throw new GameServiceException("GsSerializer Err -> serializable cant be Null");
            
            var writeStream = new GsWriteStream();
            serializable.OnGsLiveWrite(writeStream);
            return SerializerUtil.Serialize(writeStream);
        }

        internal static void CallReadStream(this IGsLiveSerializable serializable,byte[] buffer)
        {
            serializable?.OnGsLiveRead(GetReadStream(buffer));
        }
        
        private static GsReadStream GetReadStream(byte[] buffer)
        {
            if(buffer == null)
                throw new GameServiceException("GsSerializer Err -> buffer cant be Null");

            return SerializerUtil.Deserialize(buffer);
        }
        
    }
}