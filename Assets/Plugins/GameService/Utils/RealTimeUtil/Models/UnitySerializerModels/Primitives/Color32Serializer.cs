// <copyright file="Color32Serializer.cs" company="Firoozeh Technology LTD">
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
    internal class Color32Serializer : ObjectSerializer<Color32>
    {
        protected override void WriteObject(Color32 obj, GsWriteStream writeStream)
        {
            writeStream.WriteNext(new[]{obj.r,obj.g,obj.b,obj.a});
        }

        protected override Color32 ReadObject(GsReadStream readStream)
        {
            var data = (byte[]) readStream.ReadNext();
            return new Color32(data[0],data[1],data[2],data[3]);
        }
    }
}