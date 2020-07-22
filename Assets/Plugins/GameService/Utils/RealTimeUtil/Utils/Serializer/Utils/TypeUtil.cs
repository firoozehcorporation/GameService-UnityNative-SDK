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
using System.Linq;
using FiroozehGameService.Models;
using Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer.Abstracts;
using Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer.Helpers;

namespace Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer.Utils
{
    public static class TypeUtil
    {
        private static readonly Dictionary<Type,BaseSerializer> ObjectsCache = new Dictionary<Type, BaseSerializer>();
        private static readonly List<Tuple<ushort,Type>> Types = new List<Tuple<ushort,Type>>();

        internal static void RegisterNewType<T>(ObjectSerializer<T> serializer)
        {
            var type = typeof(T);
            if(Types.Any(t => t.Item2 == type))
                throw new GameServiceException("The Type " + type + " is Exist!");
            
            Types.Add(Tuple.Create((ushort)Types.Count,type));
            ObjectsCache.Add(type,serializer);
        }

        
        public static Tuple<ushort,GsWriteStream> GetWriteStream(object obj)
        {
            var type = obj.GetType();
            var typeCache = Types.FirstOrDefault(t => t.Item2 == type);
            if(typeCache == null)
                throw new GameServiceException("The Type " + type + " is Not Registered as New Type!");

            if(!ObjectsCache.ContainsKey(type))
                throw new GameServiceException("The Type " + typeCache.Item2 + " is Not Registered as New Type!");

            var serializer = ObjectsCache[type];
            if(!serializer.CanSerializeModel(obj))
                throw new GameServiceException("The Type " + typeCache.Item2 + " Not Serializable!");

            
            var writeStream = new GsWriteStream();
            serializer.SerializeObject(obj,writeStream);
            return Tuple.Create(typeCache.Item1,writeStream);
        }

        
        public static object GetFinalObject(ushort id,GsReadStream readStream)
        {
            var typeCache = Types.FirstOrDefault(t => t.Item1 == id);
            if(typeCache == null)
                throw new GameServiceException("The Type is Not Registered as New Type!");
           
            if(!ObjectsCache.ContainsKey(typeCache.Item2))
                throw new GameServiceException("The Type " + typeCache.Item2 + " is Not Registered as New Type!");
            
            var serializer = ObjectsCache[typeCache.Item2];
            
            if(!serializer.CanSerializeModel(typeCache.Item2))
                throw new GameServiceException("The Type " + typeCache.Item2 + " Not Serializable!");

            return serializer.DeserializeObject(readStream);
        }
        
        
    }
}