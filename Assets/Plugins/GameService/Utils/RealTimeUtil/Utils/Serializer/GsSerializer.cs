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

using System;
using System.Collections.Generic;
using FiroozehGameService.Models;
using Plugins.GameService.Utils.RealTimeUtil.Interfaces;
using Plugins.GameService.Utils.RealTimeUtil.Models.UnitySerializerModels.ExtendedPrimitives;
using Plugins.GameService.Utils.RealTimeUtil.Models.UnitySerializerModels.Miscs;
using Plugins.GameService.Utils.RealTimeUtil.Models.UnitySerializerModels.Primitives;
using Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer.Abstracts;
using Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer.Helpers;
using Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer.Utils;

namespace Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer
{
    public static class GsSerializer
    {
        
        public static class TypeRegistry
        {
            internal static void Init()
            {
                // Register Primitive Types
                TypeUtil.RegisterNewType(new Vector2Serializer());
                TypeUtil.RegisterNewType(new Vector3Serializer());
                TypeUtil.RegisterNewType(new Vector4Serializer());
                TypeUtil.RegisterNewType(new QuaternionSerializer());
                TypeUtil.RegisterNewType(new ColorSerializer());
                TypeUtil.RegisterNewType(new Color32Serializer());
                TypeUtil.RegisterNewType(new Matrix4X4Serializer());

                
                // Register Extended Primitive Types
                TypeUtil.RegisterNewType(new RectSerializer());
                TypeUtil.RegisterNewType(new RaySerializer());
                TypeUtil.RegisterNewType(new Ray2DSerializer());
                TypeUtil.RegisterNewType(new RangeIntSerializer());
                TypeUtil.RegisterNewType(new PlaneSerializer());
                TypeUtil.RegisterNewType(new BoundsSerializer());
                
                // Register Misc Types
                #if UNITY_2017_2_OR_NEWER
                TypeUtil.RegisterNewType(new Vector2IntSerializer());
                TypeUtil.RegisterNewType(new Vector3IntSerializer());
                #endif

            }

            internal static void Dispose()
            {
                TypeUtil.Dispose();
            }

            public static void RegisterSerializer<T>(ObjectSerializer<T> serializer)
            {
                TypeUtil.RegisterNewType(serializer);
            }
        }
        
        internal static class Function
        {
            internal static byte[] SerializeParams(params object[] data)
            {
                try
                {
                    var stream = TypeUtil.GetWriteStreamForParams(data);
                    return SerializerUtil.Serialize(stream);
                }
                catch (Exception)
                {
                    return null;
                }
            }

            internal static object[] DeserializeParams(byte[] buffer)
            {
                var objects = new List<object>();
                var readStream = SerializerUtil.Deserialize(buffer);
                while (readStream.CanRead())
                    objects.Add(readStream.ReadNext());

                return objects.ToArray();
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