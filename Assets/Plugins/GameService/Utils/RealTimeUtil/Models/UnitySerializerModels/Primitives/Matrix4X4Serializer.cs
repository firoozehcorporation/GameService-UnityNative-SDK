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
            // NOTE : Must Register Type Vector4 Before this
            for (var i = 0; i < ColumnCount; i++)
                writeStream.WriteNext(obj.GetColumn(i));
        }

        protected override Matrix4x4 ReadObject(GsReadStream readStream)
        {
            var columns = new Vector4[ColumnCount];
            
            // NOTE : Must Register Type Vector4 Before this
            for (var i = 0; i < ColumnCount; i++)
                columns[i] = (Vector4) readStream.ReadNext();
            
            return new Matrix4x4(columns[0],columns[1],columns[2],columns[3]);
        }
    }
}