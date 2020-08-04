// <copyright file="Vector2ArraySerializer.cs" company="Firoozeh Technology LTD">
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


using FiroozehGameService.Utils.Serializer.Abstracts;
using FiroozehGameService.Utils.Serializer.Helpers;
using UnityEngine;

namespace Plugins.GameService.Utils.RealTimeUtil.Models.UnitySerializerModels.Primitives
{
    internal class Vector2ArraySerializer : ObjectSerializer<Vector2[]>
    {
        private const short Base = 2;
        
        protected override void WriteObject(Vector2[] obj, GsWriteStream writeStream)
        {
            var data = new float[obj.Length * Base];
            for (var i = 0; i < obj.Length; i++)
            {
                data[Base * i]     = obj[i].x;
                data[Base * i + 1] = obj[i].y;
            }
            writeStream.WriteNext(data);
        }

        protected override Vector2[] ReadObject(GsReadStream readStream)
        {
            var readData = (float[]) readStream.ReadNext();
            var data = new Vector2[readData.Length / Base];
            
            for (var i = 0; i < readData.Length; i += Base)
                data[i / Base] = new Vector2(readData[i],readData[i + 1]);
            
            return data;
        }
    }
}