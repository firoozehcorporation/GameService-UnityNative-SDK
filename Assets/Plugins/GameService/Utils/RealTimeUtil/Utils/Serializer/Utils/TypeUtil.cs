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


using System;
using System.Collections.Generic;
using FiroozehGameService.Models;
using Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer.Abstracts;
using Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer.Helpers;

namespace Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer.Utils
{
    public static class TypeUtil
    {
        
        private static readonly Dictionary<int,BaseSerializer> ObjectsCache = new Dictionary<int, BaseSerializer>();
        private static readonly Dictionary<int,Type> HashToType = new Dictionary<int, Type>();
        private static readonly Dictionary<Type,int> TypeToHash = new Dictionary<Type,int>();

        internal static void RegisterNewType<T>(ObjectSerializer<T> serializer)
        {
            var type = typeof(T);
            if(TypeToHash.ContainsKey(type))
                throw new GameServiceException("The Type " + type + " is Exist!");

            var typeHash = HashUtil.GetHashFromType(type);
            if(HashToType.ContainsKey(typeHash))
                throw new GameServiceException("The Type " + type + " Hash is Exist!");
            
            TypeToHash.Add(type,typeHash);
            HashToType.Add(typeHash,type);
            ObjectsCache.Add(typeHash,serializer);
        }

        
        public static Tuple<int,GsWriteStream> GetWriteStream(object obj)
        {
            var type = obj.GetType();
            
            if(!TypeToHash.ContainsKey(type))
                throw new GameServiceException("The Type " + type + " is Not Registered as New Type!");
            
            var serializer = ObjectsCache[TypeToHash[type]];
            
            if(!serializer.CanSerializeModel(obj))
                throw new GameServiceException("The Type " + type + " Not Serializable!");

            
            var writeStream = new GsWriteStream();
            serializer.SerializeObject(obj,writeStream);
            return Tuple.Create(TypeToHash[type],writeStream);
        }

        
        public static object GetFinalObject(int hash,GsReadStream readStream)
        {
            if(!HashToType.ContainsKey(hash))
                throw new GameServiceException("Type With Hash " + hash + " is Not Registered!");
            
            var serializer = ObjectsCache[hash];
            if(!serializer.CanSerializeModel(HashToType[hash]))
                throw new GameServiceException("Type With Hash " + hash + " is Not Serializable!");
            
            return serializer.DeserializeObject(readStream);
        }
        
        
    }
}