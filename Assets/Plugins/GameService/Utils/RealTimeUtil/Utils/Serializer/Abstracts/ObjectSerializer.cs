// <copyright file="ObjectSerializer.cs" company="Firoozeh Technology LTD">
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
using Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer.Helpers;

namespace Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer.Abstracts
{
    public abstract class ObjectSerializer<T> : BaseSerializer
    {
        
        internal override bool CanSerializeModel(object obj)
            => obj.GetType() == typeof(T);
        
        internal override bool CanSerializeModel(Type type)
            => type == typeof(T);
        
        internal override void SerializeObject(object obj,GsWriteStream writeStream) 
            => WriteObject((T) obj,writeStream);
        
        internal override object DeserializeObject(GsReadStream readStream)
            => ReadObject(readStream);


        protected abstract void WriteObject(T obj,GsWriteStream writeStream);

        protected abstract T ReadObject(GsReadStream readStream);
    }
}