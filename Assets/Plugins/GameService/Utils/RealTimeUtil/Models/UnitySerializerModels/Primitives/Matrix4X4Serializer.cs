// <copyright file="Matrix4x4Serializer.cs" company="Firoozeh Technology LTD">
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
    internal class Matrix4X4Serializer : ObjectSerializer<Matrix4x4>
    {
        private const short ColumnCount = 4;

        protected override void WriteObject(Matrix4x4 obj, GsWriteStream writeStream)
        {
            var data = new float[ColumnCount * ColumnCount];
            for (var i = 0; i < ColumnCount * ColumnCount; i++) data[i] = obj[i];
            writeStream.WriteNext(data);
        }

        protected override Matrix4x4 ReadObject(GsReadStream readStream)
        {
            var data = (float[]) readStream.ReadNext();
            var mat = new Matrix4x4();
            
            for (var i = 0; i < data.Length; i++)
                mat[i] = data[i];
            
            return mat;
        }
    }
}