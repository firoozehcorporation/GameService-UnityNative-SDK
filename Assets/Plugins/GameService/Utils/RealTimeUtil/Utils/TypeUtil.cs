// <copyright file="TypeUtil.cs" company="Firoozeh Technology LTD">
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


using FiroozehGameService.Utils.Serializer;
using Plugins.GameService.Utils.RealTimeUtil.Models.UnitySerializerModels.ExtendedPrimitives;
using Plugins.GameService.Utils.RealTimeUtil.Models.UnitySerializerModels.Miscs;
using Plugins.GameService.Utils.RealTimeUtil.Models.UnitySerializerModels.Primitives;

namespace Plugins.GameService.Utils.RealTimeUtil.Utils
{
    internal static class TypeUtil
    {
        internal static void Init()
        {
            // Register Primitive Types
            GsSerializer.TypeRegistry.RegisterSerializer(new Vector2Serializer());
            GsSerializer.TypeRegistry.RegisterSerializer(new Vector3Serializer());
            GsSerializer.TypeRegistry.RegisterSerializer(new Vector4Serializer());
            GsSerializer.TypeRegistry.RegisterSerializer(new QuaternionSerializer());
            GsSerializer.TypeRegistry.RegisterSerializer(new ColorSerializer());
            GsSerializer.TypeRegistry.RegisterSerializer(new Color32Serializer());
            GsSerializer.TypeRegistry.RegisterSerializer(new Matrix4X4Serializer());
            
            
            
            // Register Primitive Array Types
            GsSerializer.TypeRegistry.RegisterSerializer(new Vector2ArraySerializer());
            GsSerializer.TypeRegistry.RegisterSerializer(new Vector3ArraySerializer());
            GsSerializer.TypeRegistry.RegisterSerializer(new Vector4ArraySerializer());
            GsSerializer.TypeRegistry.RegisterSerializer(new QuaternionArraySerializer());


            // Register Extended Primitive Types
            GsSerializer.TypeRegistry.RegisterSerializer(new RectSerializer());
            GsSerializer.TypeRegistry.RegisterSerializer(new RaySerializer());
            GsSerializer.TypeRegistry.RegisterSerializer(new Ray2DSerializer());
            GsSerializer.TypeRegistry.RegisterSerializer(new RangeIntSerializer());
            GsSerializer.TypeRegistry.RegisterSerializer(new PlaneSerializer());
            GsSerializer.TypeRegistry.RegisterSerializer(new BoundsSerializer());

            // Register Misc Types
            #if UNITY_2017_2_OR_NEWER
            GsSerializer.TypeRegistry.RegisterSerializer(new Vector2IntSerializer());
            GsSerializer.TypeRegistry.RegisterSerializer(new Vector3IntSerializer());
            
            GsSerializer.TypeRegistry.RegisterSerializer(new Vector2IntArraySerializer());
            GsSerializer.TypeRegistry.RegisterSerializer(new Vector3IntArraySerializer());
            #endif
        }

        internal static void Dispose()
        {
            GsSerializer.TypeRegistry.Dispose();
        }
    }
}