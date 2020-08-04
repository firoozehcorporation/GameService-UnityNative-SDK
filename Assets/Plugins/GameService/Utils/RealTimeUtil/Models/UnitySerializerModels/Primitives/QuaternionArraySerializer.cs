// <copyright file="QuaternionArraySerializer.cs" company="Firoozeh Technology LTD">
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
    internal class QuaternionArraySerializer : ObjectSerializer<Quaternion[]>
    {
        private const short Base = 4;
        
        protected override void WriteObject(Quaternion[] obj, GsWriteStream writeStream)
        {
            var data = new float[obj.Length * Base];
            for (var i = 0; i < obj.Length; i++)
            {
                data[Base * i]     = obj[i].x;
                data[Base * i + 1] = obj[i].y;
                data[Base * i + 2] = obj[i].z;
                data[Base * i + 3] = obj[i].w;
            }
            writeStream.WriteNext(data);
        }

        protected override Quaternion[] ReadObject(GsReadStream readStream)
        {
            var readData = (float[]) readStream.ReadNext();
            var data = new Quaternion[readData.Length / Base];
            
            for (var i = 0; i < readData.Length; i += Base)
                data[i / Base] = new Quaternion(readData[i],readData[i + 1],readData[i + 2],readData[i + 3]);
            
            return data;
        }
    }
}