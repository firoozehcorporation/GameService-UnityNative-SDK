// <copyright file="ObjectUtil.cs" company="Firoozeh Technology LTD">
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
using System.Reflection;
using FiroozehGameService.Models;
using Plugins.GameService.Utils.RealTimeUtil.Classes;
using Plugins.GameService.Utils.RealTimeUtil.Classes.Attributes;
using Plugins.GameService.Utils.RealTimeUtil.Classes.Handlers;
using UnityEditor;
using UnityEngine;

namespace Plugins.GameService.Utils.RealTimeUtil.Utils
{
    internal static class ObjectUtil
    {
        
        private static Dictionary<Type, List<MethodInfo>> _runableCache;
        private static Dictionary<Type, MonoBehaviour> _runableCacheMono;
        private static Dictionary<byte, GsLiveRtObserver> _observerCache;

        internal static void Init()
        {
            _runableCache = new Dictionary<Type, List<MethodInfo>>();
            _runableCacheMono = new Dictionary<Type, MonoBehaviour>();
            _observerCache = new Dictionary<byte, GsLiveRtObserver>();

            UpdateFunctions();
        }
        
        private static void UpdateFunctions()
        {
            
            var monoBehaviours = MonoBehaviourHandler.MonoBehaviours;
            var extractedMethods = TypeCache.GetMethodsWithAttribute<GsLiveFunction>();
            var methods = extractedMethods
                .Select(methodInfo => methodInfo)
                .ToList();
            
            foreach (var monoBehaviour in monoBehaviours)
            {
                
                if (monoBehaviour == null)
                {
                    Debug.LogError("ERROR You have missing MonoBehaviours on your GameObjects!");
                    continue;
                }

                var type = monoBehaviour.GetType();

                var methodsOfTypeInCache = _runableCache.TryGetValue(type, out _);
                if (methodsOfTypeInCache) continue;
                
                var methodInfos = methods.FindAll(m => m.DeclaringType == type);
                _runableCache.Add(type,methodInfos);
                _runableCacheMono.Add(type,monoBehaviour);
            }
        }

        
        
        internal static bool HaveFunctions(Type from)
        {
            if(_runableCache.Count == 0) return false;
            _runableCache.TryGetValue(from, out var methodInfos);
            if (methodInfos != null)
                return methodInfos.Count > 0;
            return false;
        }

        
        private static Tuple<Type,List<MethodInfo>> GetFunctions(string fullName)
        {
            if(_runableCache.Count == 0) return null;

           var data = _runableCache
               .FirstOrDefault(rc => rc.Key.FullName == fullName);

           if (data.Key == null || data.Value == null)
               return null;

           return Tuple.Create(data.Key, data.Value);
        }
        

        
        internal static Tuple<MonoBehaviour,MethodInfo> GetFunction(string methodName,string fullName,bool haveBuffer)
        {
            UpdateFunctions();

            var functions = GetFunctions(fullName);
            
            if(functions?.Item2 == null || functions.Item2?.Count == 0)
                throw new GameServiceException("this Type " + fullName + " Have No Runable Methods");

            var parameterLen = haveBuffer ? 1 : 0 ;
            var method = functions.Item2.FirstOrDefault(m => m.Name == methodName && m.GetParameters().Length == parameterLen);
            if(method == null)
                throw new GameServiceException("Function With Name " + methodName + " is Not Exist.You Must Set GsLiveFunction Attribute For Your Function");

            
            _runableCacheMono.TryGetValue(functions.Item1, out var monoBehaviour);
            if(monoBehaviour == null)
                throw new GameServiceException("no monoBehaviour Exist for Function With Name " + methodName);

            
            return Tuple.Create(monoBehaviour,method);
        }


        internal static void RegisterObserver(GsLiveRtObserver observer)
        {
            if(_observerCache.ContainsKey(observer.id))
                throw new GameServiceException("Observer Id Must Be Unique");
            _observerCache.Add(observer.id,observer);
        }
        
        
        internal static GsLiveRtObserver GetGsLiveObserver(byte id)
        {
            _observerCache.TryGetValue(id,out var observer);
            return observer;
        }
    }
}