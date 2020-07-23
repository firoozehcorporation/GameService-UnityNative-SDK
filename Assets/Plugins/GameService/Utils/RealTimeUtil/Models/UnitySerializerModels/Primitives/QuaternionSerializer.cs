// <copyright file="QuaternionSerializer.cs" company="Firoozeh Technology LTD">
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


using Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer.Abstracts;
using Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer.Helpers;
using UnityEngine;

namespace Plugins.GameService.Utils.RealTimeUtil.Models.UnitySerializerModels.Primitives
{
    internal class QuaternionSerializer : ObjectSerializer<Quaternion>
    {
        protected override void WriteObject(Quaternion obj, GsWriteStream writeStream)
        {
            writeStream.WriteNext(obj.x);
            writeStream.WriteNext(obj.y);
            writeStream.WriteNext(obj.z);
            writeStream.WriteNext(obj.w);
        }

        protected override Quaternion ReadObject(GsReadStream readStream)
        {
            var x = (float) readStream.ReadNext();
            var y = (float) readStream.ReadNext();
            var z = (float) readStream.ReadNext();
            var w = (float) readStream.ReadNext();
            
            return new Quaternion(x,y,z,w);
        }
    }
}