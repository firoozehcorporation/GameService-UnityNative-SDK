// <copyright file="Vector2Serializer.cs" company="Firoozeh Technology LTD">
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
    internal class Vector2Serializer : ObjectSerializer<Vector2>
    {
        protected override void WriteObject(Vector2 obj, GsWriteStream writeStream)
        {
            writeStream.WriteNext(new[]{obj.x,obj.y});
        }

        protected override Vector2 ReadObject(GsReadStream readStream)
        {
            var data = (float[]) readStream.ReadNext();
            return new Vector2(data[0],data[1]);
        }
    }
}